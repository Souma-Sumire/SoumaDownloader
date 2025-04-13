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
        #region Fields

        private string phpContent;
        private bool showMessage = true;
        private readonly string baseUrl = "https://souma.diemoe.net/raidboss/";
        private readonly string phpUrl = "https://souma.diemoe.net/list_files2.php";
        private readonly string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\SoumaDownloader.config.xml");
        private readonly Dictionary<string, string[]> fileData = new Dictionary<string, string[]>();

        public string filePath;
        public string jsonContent;
        internal UIControl PluginUI;
        private Label lblStatus;
        private SettingsSerializer xmlSettings;

        #endregion

        #region Plugin Entry

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            lblStatus = pluginStatusText;
            lblStatus.Text = "准备好下崽了(˃˂)";

            PluginUI = new UIControl { ParentClass = this };
            pluginScreenSpace.Controls.Add(PluginUI);
            pluginScreenSpace.Text = "Souma下崽器";
            PluginUI.Dock = DockStyle.Fill;

            xmlSettings = new SettingsSerializer(this);
            LoadSettings();

            if (string.IsNullOrEmpty(PluginUI.textUserDir.Text))
                AutoConfigureCactbotPath();

            PluginUI.textUserDir.Text = Path.GetFullPath(PluginUI.textUserDir.Text);
            PluginUI.VersionInfo.Text = $"版本 {Assembly.GetExecutingAssembly().GetName().Version}";

            PluginUI.btnDownload.Click += ManualUpdate;
            PluginUI.textUserDir.TextChanged += TxtUserDir_TextChanged;
            PluginUI.retry.Click += Retry_Clicked;


            _ = Check();
            if (PluginUI.checkBoxAutoUpdate.Checked)
            {
                _ = Task.Run(async () =>
                {
                    showMessage = false;
                    await ProcessAsync();
                    showMessage = true;
                });
            }
        }

        public void DeInitPlugin()
        {
            SaveSettings();
            lblStatus.Text = "插件已退出";
        }

        #endregion

        #region UI Events

        private void TxtUserDir_TextChanged(object sender, EventArgs e) => UpdateCheckbox();

        private void Retry_Clicked(object sender, EventArgs e)
        {
            _ = Check();
        }

        private void ManualUpdate(object sender, EventArgs e)
        {
            PluginUI.btnDownload.Enabled = false;
            DialogResult result = MessageBox.Show("未被勾选的js文件将会被删除，确认继续吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                _ = ProcessAsync();
            else
                PluginUI.btnDownload.Enabled = true;
        }

        #endregion

        #region Core Process

        private async Task ProcessAsync()
        {
            await Check();

            string userDir = PluginUI.textUserDir.Text;
            string backupPath = Path.Combine(Path.GetTempPath(), "cactbot_backup");

            try
            {
                if (Directory.Exists(backupPath)) Directory.Delete(backupPath, true);
                Directory.CreateDirectory(backupPath);
                BackupFiles(userDir, backupPath);

                var existingFiles = Directory.GetFiles(userDir, "*.js", SearchOption.AllDirectories)
                                             .Select(f => Path.GetFileNameWithoutExtension(f));
                var filesToKeep = PluginUI.checkedListBox1.Items.Cast<string>();
                var filesToDelete = existingFiles.Except(filesToKeep);

                foreach (var file in filesToDelete)
                    File.Delete(Path.Combine(userDir, file + ".js"));

                for (int i = 0; i < PluginUI.checkedListBox1.Items.Count; i++)
                {
                    string key = PluginUI.checkedListBox1.Items[i].ToString();
                    string[] info = fileData[key];
                    string fileName = info[0];
                    string filePath = Path.Combine(userDir, fileName);
                    string fileUrl = $"{baseUrl}{fileName}";

                    if (PluginUI.checkedListBox1.GetItemChecked(i))
                        Download(fileUrl, filePath);
                    else if (File.Exists(filePath))
                        File.Delete(filePath);
                }

                Directory.Delete(backupPath, true);
                PluginUI.textLastUpdateTime.Text = "最近一次更新于：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (showMessage)
                    MessageBox.Show("下载完成！记得刷新 Raidboss 悬浮窗以加载。");

                SaveSettings();
            }
            catch (Exception ex)
            {
                RestoreFiles(backupPath, userDir);
                if (showMessage)
                    MessageBox.Show("下载过程中发生错误：" + ex);
            }
            finally
            {
                PluginUI.btnDownload.Enabled = true;
            }
        }

        private async Task Check()
        {
            if (fileData.Count == 0)
            {
                try
                {
                    using (Stream stream = GetStream(phpUrl))
                    using (StreamReader reader = new StreamReader(stream))
                        phpContent = await reader.ReadToEndAsync();

                    UpdateFileData();
                    UpdateCheckbox();
                    PluginUI.btnDownload.Enabled = true;
                    PluginUI.retry.Hide();
                    PluginUI.checkedListBox1.Show();
                    PluginUI.btnSelectAll.Show();
                    PluginUI.btnDeselectAll.Show();
                    PluginUI.btnDownload.Show();
                    PluginUI.checkBoxAutoUpdate.Show();
                    PluginUI.textLastUpdateTime.Show();
                }
                catch (Exception ex)
                {
                    if (showMessage)
                        MessageBox.Show("获取上游列表时发生错误。" + ex);
                }
            }

            if (fileData.Count == 0 && showMessage)
            {
                MessageBox.Show("列表为空。请稍后重试。");
                return;
            }

            if (string.IsNullOrEmpty(PluginUI.textUserDir.Text))
                AutoConfigureCactbotPath();

            if (!Directory.Exists(PluginUI.textUserDir.Text))
            {
                try
                {
                    Directory.CreateDirectory(PluginUI.textUserDir.Text);
                }
                catch (Exception ex)
                {
                    if (showMessage)
                        MessageBox.Show("创建目录失败。" + ex);
                }
            }
        }

        #endregion

        #region Helper Methods

        public void AutoConfigureCactbotPath()
        {
            string settingsPath = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config", "RainbowMage.OverlayPlugin.config.json");

            if (!File.Exists(settingsPath))
            {
                PluginUI.textUserDir.Text = "自动设置失败，未找到OverlayPlugin.config.json。";
                return;
            }

            try
            {
                string json = File.ReadAllText(settingsPath);
                string path = JObject.Parse(json)
                    .SelectToken("EventSourceConfigs.CactbotESConfig.OverlayData.options.general.CactbotUserDirectory")?.Value<string>();

                if (!string.IsNullOrEmpty(path))
                    PluginUI.textUserDir.Text = Path.Combine(path, "raidboss", "Souma");
                else
                {
                    var cactbot = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(x => x.pluginObj?.GetType().ToString() == "RainbowMage.OverlayPlugin.PluginLoader");
                    string cactbotDir = Path.GetDirectoryName(cactbot.pluginFile.FullName);
                    string pluginsDir = Directory.GetParent(cactbotDir).FullName;

                    string found = new[] { "cactbot-offline", "cactbot" }
                        .Select(dir => Path.Combine(pluginsDir, dir, "user"))
                        .FirstOrDefault(Directory.Exists);

                    PluginUI.textUserDir.Text = found != null
                        ? Path.Combine(found, "raidboss", "Souma")
                        : "自动设置失败，未找到user目录。";
                }
            }
            catch (Exception ex)
            {
                PluginUI.textUserDir.Text = "自动设置失败，无法读取OverlayPlugin.config.json：" + ex.Message;
            }
        }

        public Stream GetStream(string url, int timeout = 15000)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html,application/xhtml+xml,application/xml;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = timeout;
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.KeepAlive = true;

            var response = (HttpWebResponse)request.GetResponse();

            if (new[] { HttpStatusCode.NotFound, HttpStatusCode.ServiceUnavailable, HttpStatusCode.Unauthorized,
                        HttpStatusCode.GatewayTimeout, HttpStatusCode.BadGateway, HttpStatusCode.BadRequest, HttpStatusCode.Forbidden }
                .Contains(response.StatusCode))
                return null;

            return response.GetResponseStream();
        }

        public void Download(string url, string path)
        {
            using (Stream stream = GetStream(url, 15000))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using (FileStream file = File.Create(path))
                {
                    stream.CopyTo(file);
                }
            }
        }

        private void BackupFiles(string sourcePath, string backupPath)
        {
            foreach (var file in new DirectoryInfo(sourcePath).GetFiles())
                file.CopyTo(Path.Combine(backupPath, file.Name));
        }

        private void RestoreFiles(string backupPath, string destPath)
        {
            foreach (var file in new DirectoryInfo(backupPath).GetFiles())
                file.CopyTo(Path.Combine(destPath, file.Name), true);
        }

        private void UpdateFileData()
        {
            fileData.Clear();
            foreach (Match match in Regex.Matches(phpContent, @"<a href=""([^""]*)"">([^<]*)<\/a><span>([^<]*)<\/span>"))
            {
                string name = match.Groups[2].Value;
                string full = match.Groups[1].Value.Replace(baseUrl, "");
                fileData[name] = new[] { full, match.Groups[3].Value };
            }
        }

        private void UpdateCheckbox()
        {
            PluginUI.checkedListBox1.Items.Clear();
            foreach (var pair in fileData)
            {
                bool isChecked = pair.Key.Contains("必装") ||
                    File.Exists(Path.Combine(PluginUI.textUserDir.Text, pair.Value[0]));
                PluginUI.checkedListBox1.Items.Add(pair.Key, isChecked);
            }
        }

        #endregion

        #region Settings

        private void LoadSettings()
        {
            xmlSettings.AddControlSetting("userDir", PluginUI.textUserDir);
            xmlSettings.AddControlSetting("autoUpdate", PluginUI.checkBoxAutoUpdate);

            if (!File.Exists(settingsFile)) return;

            FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlTextReader reader = new XmlTextReader(fs);
            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "SettingsSerializer")
                        xmlSettings.ImportFromXml(reader);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error loading settings: " + ex.Message;
            }
            finally
            {
                reader.Close();
                fs.Close();
            }
        }

        private void SaveSettings()
        {
            FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter writer = new XmlTextWriter(fs, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 1,
                IndentChar = '\t'
            };

            writer.WriteStartDocument(true);
            writer.WriteStartElement("Config");
            writer.WriteStartElement("SettingsSerializer");
            xmlSettings.ExportToXml(writer);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();

            writer.Close();
            fs.Close();
        }

        #endregion
    }
}
