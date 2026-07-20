using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SoumaDownloader
{
    public partial class UIControl : UserControl
    {
        public ACT_Plugin_Souma_Downloader.SoumaDownloader ParentClass;

        public UIControl()
        {
            InitializeComponent();
            if (comboPluginProxy.SelectedIndex < 0)
                comboPluginProxy.SelectedIndex = 0;
        }

        private async void BtnCheckPluginUpdate_Click(object sender, EventArgs e)
        {
            string proxy = comboPluginProxy.SelectedItem?.ToString() ?? "GitHub直连";
            await ACT_Plugin_Souma_Downloader.PluginUpdater.CheckAndUpdateAsync(proxy, true, btnCheckPluginUpdate, VersionInfo);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                string current = ParentClass.PluginUI.textUserDir.Text;
                if (Directory.Exists(current))
                    dialog.SelectedPath = current;

                if (dialog.ShowDialog() == DialogResult.OK)
                    ParentClass.PluginUI.textUserDir.Text = dialog.SelectedPath;
            }
        }

        private void BtnOpenDirectory_Click(object sender, EventArgs e)
        {
            string userPath = ParentClass.PluginUI.textUserDir.Text;

            try
            {
                string parentParentDir = Path.GetDirectoryName(Path.GetDirectoryName(userPath));
                if (string.IsNullOrEmpty(userPath) || userPath.Contains("自动设置失败") ||
                    (!Directory.Exists(parentParentDir) && !Directory.Exists(userPath)))
                    throw new Exception("无效的用户目录！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(userPath))
            {
                try { Directory.CreateDirectory(userPath); }
                catch (Exception ex)
                {
                    MessageBox.Show("无法创建 cactbot 用户目录：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (Directory.Exists(userPath))
            {
                try { System.Diagnostics.Process.Start("explorer.exe", userPath); }
                catch (Exception ex)
                {
                    MessageBox.Show("无法打开 cactbot 用户目录：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("cactbot 用户目录不存在。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnFindDir_Click(object sender, EventArgs e)
        {
            ParentClass.AutoConfigureCactbotPath();

            if (string.IsNullOrEmpty(ParentClass.PluginUI.textUserDir.Text) ||
                ParentClass.PluginUI.textUserDir.Text.Contains("自动设置失败"))
                MessageBox.Show("无效的用户目录！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void LinkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Souma-Sumire/raidboss-user-js-public");
        }

        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            listViewFiles.BeginUpdate();
            foreach (ListViewItem item in listViewFiles.Items)
                item.Checked = true;
            listViewFiles.EndUpdate();
        }

        private void DeselectAllButton_Click(object sender, EventArgs e)
        {
            listViewFiles.BeginUpdate();
            foreach (ListViewItem item in listViewFiles.Items)
            {
                if (!item.Text.Contains("必装"))
                    item.Checked = false;
            }
            listViewFiles.EndUpdate();
        }

        private void BtnInvert_Click(object sender, EventArgs e)
        {
            listViewFiles.BeginUpdate();
            foreach (ListViewItem item in listViewFiles.Items)
            {
                if (!item.Text.Contains("必装"))
                    item.Checked = !item.Checked;
            }
            listViewFiles.EndUpdate();
        }

        private void ListViewFiles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // 必装文件不允许取消勾选
            if (listViewFiles.Items[e.Index].Text.Contains("必装"))
                e.NewValue = CheckState.Checked;
        }
    }
}
