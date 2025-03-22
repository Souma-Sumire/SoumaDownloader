using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using SoumaDownloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ACT_Plugin_Souma_Downloader
{
    public class SoumaDownloader : IActPluginV1
    {
        public string filePath;
        public string jsonContent;
        internal UIControl PluginUI;
        Label lblStatus;    // 在ACT的插件选项卡中显示的状态标签
        readonly string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\SoumaDownloader.config.xml");
        SettingsSerializer xmlSettings;
        string phpContent;
        readonly Dictionary<string, string[]> fileData = new Dictionary<string, string[]>();

        readonly string baseUrl = $"https://souma.diemoe.net/raidboss/";
        readonly string phpUrl = "https://souma.diemoe.net/list_files2.php";

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            lblStatus = pluginStatusText;   // 将状态标签的引用传递给我们的本地变量。

            PluginUI = new UIControl
            {
                ParentClass = this
            };

            pluginScreenSpace.Controls.Add(PluginUI); // 将此UserControl添加到ACT提供的选项卡中。
            pluginScreenSpace.Text = "Souma下崽器";
            PluginUI.Dock = DockStyle.Fill; // 将UserControl扩展到填充选项卡的客户端空间。
            lblStatus.Text = "准备好下崽了(˃˂)";

            xmlSettings = new SettingsSerializer(this); // 创建一个新的设置序列化器并将其传递给该实例。
            LoadSettings();
            if (PluginUI.textUserDir.Text == "") AutoConfigureCactbotPath();
            PluginUI.textUserDir.Text = Path.GetFullPath(PluginUI.textUserDir.Text);
            PluginUI.VersionInfo.Text = $"版本 {Assembly.GetExecutingAssembly().GetName().Version}";

            PluginUI.btnDownload.Click += DownloadSelected;
            PluginUI.textUserDir.TextChanged += TxtUserDir_TextChanged;
            PluginUI.checkedListBox1.MouseMove += CheckedListBox1_MouseMove;

            var parentTabControl = FindParentTabControl(pluginScreenSpace);
            if (parentTabControl != null)
            {
                void handler(object sender, TabControlEventArgs e)
                {
                    if (e.TabPage == pluginScreenSpace)
                    {
                        StartTask();
                        if (fileData.Count != 0)
                        {
                            parentTabControl.Selected -= handler;
                        }
                    }
                }
                parentTabControl.Selected += handler;
            }
        }

        private TabControl FindParentTabControl(Control control)
        {
            while (control != null)
            {
                if (control.Parent is TabControl tabControl)
                {
                    return tabControl;
                }
                control = control.Parent;
            }
            return null;
        }

        public void DeInitPlugin()
        {
            SaveSettings();
            lblStatus.Text = "插件已退出";
        }

        public async void StartTask()
        {
            await UpdateList();
            CheckUserDir();
        }

        private void TxtUserDir_TextChanged(object sender, EventArgs e)
        {
            UpdateCheckedList();
        }

        public void AutoConfigureCactbotPath()
        {
            string settingsPath = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config", "RainbowMage.OverlayPlugin.config.json");

            if (!File.Exists(settingsPath))
            {
                PluginUI.textUserDir.Text = "自动设置失败，未找到OverlayPlugin.config.json。";
                return;
            }

            string jsonContent = null;
            try
            {
                jsonContent = File.ReadAllText(settingsPath);
            }
            catch (Exception ex)
            {
                PluginUI.textUserDir.Text = "自动设置失败，无法读取OverlayPlugin.config.json：" + ex.Message;
                return;
            }

            string propertyValue = JObject.Parse(jsonContent)
                .SelectToken("EventSourceConfigs.CactbotESConfig.OverlayData.options.general.CactbotUserDirectory")?.Value<string>();

            if (!string.IsNullOrEmpty(propertyValue))
                PluginUI.textUserDir.Text = Path.Combine(propertyValue, "raidboss", "Souma");
            else
            {
                var cactbot = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.pluginObj?.GetType().ToString() == "RainbowMage.OverlayPlugin.PluginLoader");
                string cactbotDirectory = Path.GetDirectoryName(cactbot.pluginFile.FullName);
                string pluginsDirectory = Directory.GetParent(cactbotDirectory).FullName;

                string[] searchPaths = { "cactbot-offline", "cactbot" };
                string foundPath = searchPaths
                    .Select(searchPath => Path.Combine(pluginsDirectory, searchPath, "user"))
                    .FirstOrDefault(fullPath => Directory.Exists(fullPath));

                if (foundPath == null)
                {
                    PluginUI.textUserDir.Text = "自动设置失败，未找到user目录。";
                    return;
                }
                PluginUI.textUserDir.Text = Path.Combine(foundPath, "raidboss", "Souma");
            }
        }

        private void CheckUserDir()
        {
            if (string.IsNullOrEmpty(PluginUI.textUserDir.Text)) AutoConfigureCactbotPath();
            if (Directory.Exists(PluginUI.textUserDir.Text)) return;
            try
            {
                // 创建目录
                Directory.CreateDirectory(PluginUI.textUserDir.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create cactbotUserDirectory: " + ex.Message);
                return;
            }
        }

        private void Process()
        {
            string userDir = PluginUI.textUserDir.Text;
            // 备份原文件的目录
            string backupPath = Path.Combine(Path.GetTempPath(), "cactbot_backup");

            try
            {
                // 删除之前的备份目录
                if (Directory.Exists(backupPath)) Directory.Delete(backupPath, true);

                // 创建备份目录
                Directory.CreateDirectory(backupPath);

                // 备份原文件
                BackupFiles(userDir, backupPath);

                string[] files = Directory.GetFiles(userDir, "*.js", SearchOption.AllDirectories);

                var filesWithoutExtension = files.Select(file => Path.GetFileNameWithoutExtension(file));
                var itemsToBeDeleted = filesWithoutExtension.Except(PluginUI.checkedListBox1.Items.Cast<string>());

                // 删除未勾选
                foreach (var file in itemsToBeDeleted) File.Delete(Path.Combine(userDir, file + ".js"));

                for (int i = 0; i < PluginUI.checkedListBox1.Items.Count; i++)
                {
                    string key = PluginUI.checkedListBox1.Items[i].ToString();
                    string fileFullName = fileData.FirstOrDefault(x => x.Key == key).Value[0];
                    bool isChecked = PluginUI.checkedListBox1.GetItemChecked(i);
                    string fileUrl = $"{baseUrl}{fileFullName}";
                    string filePath = Path.Combine(userDir, fileFullName);
                    if (isChecked)
                    {
                        Download(fileUrl, filePath);
                    }
                    else
                    {
                        // 如果项目未被勾选，且文件存在，则删除文件
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }

                // 下载完成后，删除备份目录
                Directory.Delete(backupPath, true);

                // 保存成功更新时间
                MessageBox.Show("下载完成！记得刷新 Raidboss 悬浮窗以加载。");

                SaveSettings();
            }
            catch (Exception ex)
            {
                // 发生异常，恢复备份文件
                RestoreFiles(backupPath, userDir);
                MessageBox.Show("下载过程中发生错误：" + ex);
            }
            finally
            {
                // 启用下载按钮
                PluginUI.btnDownload.Enabled = true;
            }
        }

        private async void DownloadSelected(object sender, EventArgs e)
        {
            PluginUI.btnDownload.Enabled = false;
            CheckUserDir();
            if (fileData.Count == 0)
            {
                bool fetchSuccessful = await UpdateList();
                if (!fetchSuccessful)
                {
                    MessageBox.Show("无法获取列表。请稍后重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (fileData.Count == 0)
            {
                MessageBox.Show("列表为空。请稍后重试。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult result = MessageBox.Show("未被勾选的js文件将会被删除，确认继续吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            // 用户点击了“是”
            if (result == DialogResult.Yes)
            {
                Process();
            }
            else
            {
                PluginUI.btnDownload.Enabled = true;
            }
        }

        private void BackupFiles(string sourcePath, string backupPath)
        {
            DirectoryInfo directory = new DirectoryInfo(sourcePath);
            foreach (FileInfo file in directory.GetFiles())
            {
                string destinationFile = Path.Combine(backupPath, file.Name);
                file.CopyTo(destinationFile);
            }
        }

        private void RestoreFiles(string backupPath, string destinationPath)
        {
            DirectoryInfo directory = new DirectoryInfo(backupPath);
            foreach (FileInfo file in directory.GetFiles())
            {
                string destinationFile = Path.Combine(destinationPath, file.Name);
                file.CopyTo(destinationFile, true);
            }
        }

        private async Task<bool> UpdateList()
        {
            try
            {
                using (Stream stream = GetStream(phpUrl))
                using (StreamReader reader = new StreamReader(stream))
                {
                    phpContent = await reader.ReadToEndAsync();
                }
                UpdateFileData();
                UpdateCheckedList();
                PluginUI.btnDownload.Enabled = true;
                return true; // Fetch was successful
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误： " + ex, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Fetch failed
            }
        }

        public Stream GetStream(string url, int Timeout = 15000)
        {
            // 设置安全协议
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            // 设置请求的内容类型，表明接受HTML、XHTML和XML格式的数据
            request.ContentType = "text/html,application/xhtml+xml,application/xml;charset=UTF-8";

            // 设置用户代理（User-Agent）
            request.UserAgent = null;

            // 设置请求超时时间
            request.Timeout = Timeout;
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.KeepAlive = true;

            // 发送请求并获取服务器的响应
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.NotFound || // 404 - 资源未找到
                response.StatusCode == HttpStatusCode.ServiceUnavailable || // 503 - 服务器不可用
                response.StatusCode == HttpStatusCode.Unauthorized || // 401 - 未授权
                response.StatusCode == HttpStatusCode.GatewayTimeout || // 504 - 网关超时
                response.StatusCode == HttpStatusCode.BadGateway || // 502 - 错误网关
                response.StatusCode == HttpStatusCode.BadRequest || // 400 - 错误请求
                response.StatusCode == HttpStatusCode.Forbidden) // 403 - 禁止访问
            {
                return null;
            }

            return response.GetResponseStream();
        }


        public void Download(string url, string path)
        {
            Stream temp = GetStream(url, 15000);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream tempFile = File.Create(path);
            temp.CopyTo(tempFile);
            tempFile.Dispose();
            temp.Dispose();
        }

        private void UpdateFileData()
        {
            string pattern = @"<a href=""([^""]*)"">([^<]*)<\/a><span>([^<]*)<\/span>";
            MatchCollection matches = Regex.Matches(phpContent, pattern);
            fileData.Clear();

            // 提取解析结果并保存到字典中
            foreach (Match match in matches)
            {
                string fileName = match.Groups[2].Value;
                string fileFullName = match.Groups[1].Value.Replace(baseUrl, "");
                string fileModified = match.Groups[3].Value;
                string[] fileInfo = { fileFullName, fileModified };
                fileData.Add(fileName, fileInfo);
            }
        }

        private void UpdateCheckedList()
        {
            PluginUI.checkedListBox1.Items.Clear();
            foreach (KeyValuePair<string, string[]> pair in fileData)
            {
                string fileName = pair.Key;
                string fileFullName = pair.Value[0];
                PluginUI.checkedListBox1.Items.Add(fileName, fileName.Contains("必装") || File.Exists(Path.Combine(PluginUI.textUserDir.Text, fileFullName)));
            }
        }


        private void CheckedListBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var index = PluginUI.checkedListBox1.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                var key = PluginUI.checkedListBox1.Items[index].ToString();
                string tipText = fileData[key][1];
                if (PluginUI.toolTip1.GetToolTip(PluginUI.checkedListBox1) != key)
                {
                    PluginUI.toolTip1.SetToolTip(PluginUI.checkedListBox1, $"上游更新时间：{tipText}");
                }
            }
            else
            {
                PluginUI.toolTip1.SetToolTip(PluginUI.checkedListBox1, "");
            }
        }

        #region Save&Load

        void LoadSettings()
        {
            xmlSettings.AddControlSetting("userDir", PluginUI.textUserDir);
            if (File.Exists(settingsFile))
            {
                FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlTextReader xReader = new XmlTextReader(fs);

                try
                {
                    while (xReader.Read())
                    {
                        if (xReader.NodeType == XmlNodeType.Element)
                        {
                            if (xReader.LocalName == "SettingsSerializer")
                            {
                                xmlSettings.ImportFromXml(xReader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error loading settings: " + ex.Message;
                }
                xReader.Close();
            }
        }
        void SaveSettings()
        {
            FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter xWriter = new XmlTextWriter(fs, Encoding.UTF8)
            {
                Formatting = System.Xml.Formatting.Indented,
                Indentation = 1,
                IndentChar = '\t'
            };
            xWriter.WriteStartDocument(true);
            xWriter.WriteStartElement("Config");    // <Config>
            xWriter.WriteStartElement("SettingsSerializer");    // <Config><SettingsSerializer>
            xmlSettings.ExportToXml(xWriter);   // Fill the SettingsSerializer XML
            xWriter.WriteEndElement();  // </SettingsSerializer>
            xWriter.WriteEndElement();  // </Config>
            xWriter.WriteEndDocument(); // 处理一些杂项工作（如果有的话）。
            xWriter.Flush();    // 将文件缓冲区刷新到磁盘上。
            xWriter.Close();
        }

        #endregion
    }
}

