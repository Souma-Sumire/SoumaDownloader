using Advanced_Combat_Tracker;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoumaDownloader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
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
        public HttpClient client = new HttpClient();
        Label lblStatus;    // 在ACT的插件选项卡中显示的状态标签
        readonly string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\SoumaDownloader.config.xml");
        SettingsSerializer xmlSettings;

        #region IActPluginV1 Members

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
            if (PluginUI.txtUserDir.Text.Length == 0) AutoConfigureCactbotPath();
            PluginUI.txtUserDir.Text = PluginUI.txtUserDir.Text.Replace("\\\\", "\\"); // xml保存时会出现双斜线 不知道怎么正确解决 暂时无脑替换双斜线B
            PluginUI.VersionInfo.Text = $"版本 {Assembly.GetExecutingAssembly().GetName().Version}";
            PluginUI.btnDownload.Click += DownloadSelected;
            PluginUI.btnFetch.Click += BtnFetch_Click;
        }

        public void DeInitPlugin()
        {
            SaveSettings();
            lblStatus.Text = "插件已退出";
        }

        #endregion

        public void AutoConfigureCactbotPath()
        {
            string settingsPath = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config", "RainbowMage.OverlayPlugin.config.json");

            if (!File.Exists(settingsPath))
            {
                PluginUI.txtUserDir.Text = "自动设置失败，未找到OverlayPlugin.config.json。";
                return;
            }

            string jsonContent = null;
            try
            {
                jsonContent = File.ReadAllText(settingsPath);
            }
            catch (Exception ex)
            {
                PluginUI.txtUserDir.Text = "自动设置失败，无法读取OverlayPlugin.config.json：" + ex.Message;
                return;
            }

            string propertyValue = JObject.Parse(jsonContent)
                .SelectToken("EventSourceConfigs.CactbotESConfig.OverlayData.options.general.CactbotUserDirectory")?.Value<string>();

            if (!string.IsNullOrEmpty(propertyValue)) propertyValue = Path.Combine(propertyValue, "raidboss", "Souma");
            else
            {
                var cactbot = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.pluginObj?.GetType().ToString() == "RainbowMage.OverlayPlugin.PluginLoader");
                string cactbotDirectory = Path.GetDirectoryName(cactbot.pluginFile.FullName);
                string pluginsDirectory = Directory.GetParent(cactbotDirectory).FullName;

                string[] searchPaths = { "cactbot-offline", "cactbot" };
                string foundPath = null;

                foreach (string searchPath in searchPaths)
                {
                    string fullPath = Path.Combine(pluginsDirectory, searchPath, "user");
                    if (Directory.Exists(fullPath))
                    {
                        foundPath = fullPath;
                        break;
                    }
                }

                if (foundPath == null)
                {
                    PluginUI.txtUserDir.Text = "自动设置失败，未找到user目录。";
                    return;
                }
                propertyValue = Path.Combine(foundPath, "raidboss", "Souma");
            }
            PluginUI.txtUserDir.Text = propertyValue;
        }

        private async void DownloadSelected(object sender, EventArgs e)
        {
            string userDir = PluginUI.txtUserDir.Text;

            if (string.IsNullOrEmpty(userDir))
            {
                MessageBox.Show("用户目录为空！");
                return;
            }

            if (!Directory.Exists(userDir))
            {
                AutoConfigureCactbotPath();

                userDir = PluginUI.txtUserDir.Text;
                if (!Directory.Exists(userDir))
                {
                    MessageBox.Show("路径不存在！");
                    return;
                }
            }

            try
            {
                // 创建目录
                Directory.CreateDirectory(userDir);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to create cactbotUserDirectory: " + ex.Message);
                return;
            }

            // 禁用下载按钮
            PluginUI.btnDownload.Enabled = false;

            string diemoePath = Path.Combine(Directory.GetParent(userDir).FullName, "呆萌整合");
            if (Directory.Exists(diemoePath))
            {
                DialogResult diemoe = MessageBox.Show("检测到呆萌整合自带的JS文件，无法兼容，需要帮你删除呆萌整合自带的JS文件吗？", "冲突处理", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (diemoe == DialogResult.Yes)
                {
                    Directory.Delete(diemoePath, true);
                }
            }

            DialogResult result = MessageBox.Show("开始下载吗？", "下载确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // 用户点击了“是”
            if (result == DialogResult.Yes)
            {
                // 备份原文件的目录
                string backupPath = Path.Combine(Path.GetTempPath(), "cactbot_backup");

                try
                {
                    // 删除之前的备份目录
                    if (Directory.Exists(backupPath))
                    {
                        Directory.Delete(backupPath, true);
                    }

                    // 创建备份目录
                    Directory.CreateDirectory(backupPath);

                    // 备份原文件
                    BackupFiles(userDir, backupPath);

                    // 获取选中的下载链接
                    var selectedItems = PluginUI.checkedListBox1.CheckedItems.Cast<string>().ToList();

                    // 下载选中的文件
                    foreach (string downloadLink in selectedItems)
                    {
                        bool downloadSuccess = await DownloadFileAsync(client, "https://souma.diemoe.net/raidboss/" + downloadLink, userDir);

                        if (!downloadSuccess)
                        {
                            // 下载失败，恢复备份文件
                            RestoreFiles(backupPath, userDir);
                            MessageBox.Show("下载失败！已恢复原始文件。");
                            return;
                        }
                    }

                    // 下载完成后，删除备份目录
                    Directory.Delete(backupPath, true);

                    MessageBox.Show("下载完成！记得刷新Raidboss悬浮窗以加载。");

                    SaveSettings();
                }
                catch (Exception ex)
                {
                    // 发生异常，恢复备份文件
                    RestoreFiles(backupPath, userDir);
                    MessageBox.Show("下载过程中发生错误：" + ex.Message);
                }
                finally
                {
                    // 启用下载按钮
                    PluginUI.btnDownload.Enabled = true;
                }
            }
            PluginUI.btnDownload.Enabled = true;
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

        private async void BtnFetch_Click(object sender, EventArgs e)
        {
            try
            {
                string phpUrl = "https://souma.diemoe.net/list_files.php";
                HttpResponseMessage response = await client.GetAsync(phpUrl);
                response.EnsureSuccessStatusCode();
                string phpContent = await response.Content.ReadAsStringAsync();

                // 保存选中的项目名称
                List<string> selectedItems = new List<string>();
                foreach (var selectedItem in PluginUI.checkedListBox1.CheckedItems)
                {
                    selectedItems.Add(selectedItem.ToString());
                }

                PluginUI.checkedListBox1.Items.Clear();

                foreach (string downloadLink in ExtractDownloadLinks(phpContent))
                {
                    PluginUI.checkedListBox1.Items.Add(downloadLink);
                }

                // 默认选中必装
                for (int i = 0; i < PluginUI.checkedListBox1.Items.Count; i++)
                {
                    string itemName = PluginUI.checkedListBox1.Items[i].ToString();
                    if (itemName.Contains("必装"))
                    {
                        PluginUI.checkedListBox1.SetItemChecked(i, true);
                    }
                }

                // 重新选中之前选中的项
                for (int i = 0; i < PluginUI.checkedListBox1.Items.Count; i++)
                {
                    if (selectedItems.Contains(PluginUI.checkedListBox1.Items[i].ToString()))
                    {
                        PluginUI.checkedListBox1.SetItemChecked(i, true);
                    }
                }

                PluginUI.btnDownload.Enabled = true;

                // 保存成功更新时间
                string updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                PluginUI.textlLastUpdateTime.Text = updateTime;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误： " + ex.Message);
            }
        }

        static string[] ExtractDownloadLinks(string phpContent)
        {
            const string linkStartTag = "<a href=\"";
            const string linkEndTag = "\">";
            const string linkPrefix = "https://souma.diemoe.net/raidboss/";

            int startIndex = 0;
            int endIndex = 0;
            var downloadLinksList = new List<string>();
            while (true)
            {
                startIndex = phpContent.IndexOf(linkStartTag, endIndex);
                if (startIndex == -1)
                    break;

                startIndex += linkStartTag.Length;
                endIndex = phpContent.IndexOf(linkEndTag, startIndex);
                string linkSubstring = phpContent.Substring(startIndex, endIndex - startIndex);
                linkSubstring = linkSubstring.Replace(linkPrefix, ""); // 移除链接中的前缀部分
                downloadLinksList.Add(linkSubstring);
            }
            return downloadLinksList.ToArray();
        }

        static async Task<bool> DownloadFileAsync(HttpClient client, string downloadLink, string filePath)
        {
            // 构建目标文件的完整路径
            string fileName = Path.GetFileName(downloadLink);
            string fileSavePath = Path.Combine(filePath, fileName);

            try
            {
                // 发送GET请求下载文件
                HttpResponseMessage response = await client.GetAsync(downloadLink);
                response.EnsureSuccessStatusCode();

                // 将下载的文件保存到指定路径
                using (FileStream fileStream = new FileStream(fileSavePath, FileMode.Create, FileAccess.Write))
                {
                    await response.Content.CopyToAsync(fileStream);
                }

                return true; // 下载成功，返回 true
            }
            catch (Exception ex)
            {
                // 下载过程中发生异常，可以在此处进行错误处理
                Console.WriteLine("下载文件时出现错误：" + ex.Message);
                return false; // 下载失败，返回 false
            }
        }


        #region Save&Load

        void LoadSettings()
        {
            // 添加您希望保存状态的任何控件。
            xmlSettings.AddControlSetting("userDir", PluginUI.txtUserDir);
            xmlSettings.AddControlSetting("checkedList", PluginUI.checkedListBox1);
            xmlSettings.AddControlSetting("lastUpdateTime", PluginUI.textlLastUpdateTime);
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

