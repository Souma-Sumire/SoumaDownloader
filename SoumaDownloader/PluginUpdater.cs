using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACT_Plugin_Souma_Downloader
{
    public static class PluginUpdater
    {
        private const string ApiLatestRelease = "https://api.github.com/repos/Souma-Sumire/SoumaDownloader/releases/latest";

        public static async Task CheckAndUpdateAsync(string proxyPrefix, bool isManualCheck, Button checkBtn = null, Label statusLbl = null)
        {
            try
            {
                if (checkBtn != null)
                {
                    checkBtn.Enabled = false;
                    checkBtn.Text = "检查中...";
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string requestUrl = ApiLatestRelease;
                if (!string.IsNullOrWhiteSpace(proxyPrefix) && proxyPrefix != "GitHub直连")
                {
                    string prefix = proxyPrefix.Trim();
                    if (!prefix.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                        !prefix.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        prefix = "https://" + prefix;
                    }
                    prefix = prefix.TrimEnd('/') + "/";
                    requestUrl = prefix + ApiLatestRelease;
                }

                string jsonResponse = string.Empty;
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                    request.Method = "GET";
                    request.UserAgent = "SoumaDownloader-AutoUpdater";
                    request.Timeout = 15000;
                    request.AutomaticDecompression = DecompressionMethods.GZip;

                    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonResponse = await reader.ReadToEndAsync();
                    }
                }
                catch (WebException wex) when (wex.Response is HttpWebResponse resp && resp.StatusCode == HttpStatusCode.Forbidden && requestUrl != ApiLatestRelease)
                {
                    // 若代理站限制 api.github.com 域名返回 403，降级退回 GitHub 原生 API
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiLatestRelease);
                    request.Method = "GET";
                    request.UserAgent = "SoumaDownloader-AutoUpdater";
                    request.Timeout = 15000;
                    request.AutomaticDecompression = DecompressionMethods.GZip;

                    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        jsonResponse = await reader.ReadToEndAsync();
                    }
                }

                JObject releaseObj = JObject.Parse(jsonResponse);
                string tagName = releaseObj["tag_name"]?.ToString();
                if (string.IsNullOrEmpty(tagName))
                {
                    if (statusLbl != null) statusLbl.Text = "获取版本失败";
                    return;
                }

                string versionString = tagName.TrimStart('v', 'V');
                if (!Version.TryParse(versionString, out Version latestVersion))
                {
                    if (statusLbl != null) statusLbl.Text = "版本解析失败";
                    return;
                }

                Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;

                if (latestVersion <= currentVersion)
                {
                    if (statusLbl != null) statusLbl.Text = $"v{currentVersion}";
                    if (isManualCheck)
                    {
                        MessageBox.Show($"当前已是最新版本 (v{currentVersion})", "检查更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    return;
                }

                JArray assets = releaseObj["assets"] as JArray;
                JToken dllAsset = assets?.FirstOrDefault(a => string.Equals(a["name"]?.ToString(), "SoumaDownloader.dll", StringComparison.OrdinalIgnoreCase));

                if (dllAsset == null)
                {
                    if (statusLbl != null) statusLbl.Text = "未找到 DLL 文件";
                    return;
                }

                string downloadUrl = dllAsset["browser_download_url"]?.ToString();
                if (string.IsNullOrEmpty(downloadUrl))
                {
                    if (statusLbl != null) statusLbl.Text = "下载链接为空";
                    return;
                }

                if (!string.IsNullOrWhiteSpace(proxyPrefix) && proxyPrefix != "GitHub直连")
                {
                    string prefix = proxyPrefix.Trim();
                    if (!prefix.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                        !prefix.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        prefix = "https://" + prefix;
                    }
                    prefix = prefix.TrimEnd('/') + "/";
                    downloadUrl = prefix + downloadUrl;
                }

                string body = releaseObj["body"]?.ToString() ?? string.Empty;
                DialogResult dialogResult = MessageBox.Show(
                    $"发现新版本 v{latestVersion}（当前 v{currentVersion}）。\n\n更新内容：\n{body}\n\n是否立即更新？",
                    "发现新版本",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (dialogResult != DialogResult.Yes)
                {
                    if (statusLbl != null) statusLbl.Text = $"v{currentVersion}";
                    return;
                }

                await PerformDownloadAndReplaceAsync(downloadUrl, checkBtn, statusLbl);
            }
            catch (Exception ex)
            {
                if (statusLbl != null) statusLbl.Text = "检查失败: " + ex.Message;
                if (checkBtn != null) checkBtn.Text = "重试";
            }
            finally
            {
                if (checkBtn != null && checkBtn.Text != "已更新")
                {
                    checkBtn.Enabled = true;
                    if (checkBtn.Text == "检查中...")
                    {
                        checkBtn.Text = "检查更新";
                    }
                }
            }
        }

        private static async Task PerformDownloadAndReplaceAsync(string downloadUrl, Button checkBtn, Label statusLbl)
        {
            string currentDllPath = GetCurrentPluginDllPath();
            if (string.IsNullOrEmpty(currentDllPath))
            {
                if (statusLbl != null) statusLbl.Text = "寻找 DLL 路径失败";
                return;
            }

            string tempFilePath = currentDllPath + ".tmp";
            string oldFilePath = currentDllPath + ".old";

            try
            {
                if (checkBtn != null) checkBtn.Text = "下载中...";
                if (statusLbl != null) statusLbl.Text = "正在下载 DLL...";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(downloadUrl);
                request.Method = "GET";
                request.UserAgent = "SoumaDownloader-AutoUpdater";
                request.Timeout = 30000;
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                using (FileStream fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    long totalBytes = response.ContentLength;
                    byte[] buffer = new byte[8192];
                    long totalRead = 0;
                    int read;

                    while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fs.WriteAsync(buffer, 0, read);
                        totalRead += read;
                        if (totalBytes > 0 && checkBtn != null)
                        {
                            int percent = (int)(totalRead * 100 / totalBytes);
                            checkBtn.Text = $"{percent}%";
                        }
                    }
                }

                if (checkBtn != null) checkBtn.Text = "覆盖中...";
                if (statusLbl != null) statusLbl.Text = "正在覆盖插件...";

                try
                {
                    File.Copy(tempFilePath, currentDllPath, true);
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                }
                catch (IOException)
                {
                    if (File.Exists(oldFilePath))
                    {
                        try { File.Delete(oldFilePath); } catch { }
                    }
                    File.Move(currentDllPath, oldFilePath);
                    File.Move(tempFilePath, currentDllPath);
                }

                if (checkBtn != null)
                {
                    checkBtn.Text = "已更新";
                    checkBtn.Enabled = false;
                }
                if (statusLbl != null) statusLbl.Text = "更新成功!重启ACT生效";
            }
            catch (Exception)
            {
                if (File.Exists(tempFilePath))
                {
                    try { File.Delete(tempFilePath); } catch { }
                }
                if (checkBtn != null)
                {
                    checkBtn.Text = "检查更新";
                    checkBtn.Enabled = true;
                }
                if (statusLbl != null) statusLbl.Text = "下载覆盖失败";
            }
        }

        private static string GetCurrentPluginDllPath()
        {
            try
            {
                // 1. 从 ACT 插件列表中精确匹配
                if (ActGlobals.oFormActMain?.ActPlugins != null)
                {
                    var plugin = ActGlobals.oFormActMain.ActPlugins.FirstOrDefault(p =>
                        p.pluginObj is SoumaDownloader ||
                        (p.pluginFile != null && string.Equals(p.pluginFile.Name, "SoumaDownloader.dll", StringComparison.OrdinalIgnoreCase))
                    );

                    if (plugin?.pluginFile != null && File.Exists(plugin.pluginFile.FullName))
                    {
                        return plugin.pluginFile.FullName;
                    }
                }
            }
            catch { }

            try
            {
                // 2. 从 Assembly.CodeBase 获取
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                if (!string.IsNullOrEmpty(codeBase))
                {
                    UriBuilder uri = new UriBuilder(codeBase);
                    string path = Uri.UnescapeDataString(uri.Path);
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }
            catch { }

            try
            {
                // 3. 从 Assembly.Location 获取
                string location = Assembly.GetExecutingAssembly().Location;
                if (!string.IsNullOrEmpty(location) && File.Exists(location))
                {
                    return location;
                }
            }
            catch { }

            try
            {
                // 4. 应用程序基础目录降级匹配
                string baseDirDll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SoumaDownloader.dll");
                if (File.Exists(baseDirDll))
                {
                    return baseDirDll;
                }
            }
            catch { }

            return null;
        }

        public static void CleanOldBackupFiles()
        {
            try
            {
                string currentDllPath = GetCurrentPluginDllPath();
                if (!string.IsNullOrEmpty(currentDllPath))
                {
                    string oldFilePath = currentDllPath + ".old";
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }
            }
            catch
            {
                // 静默清除
            }
        }
    }
}
