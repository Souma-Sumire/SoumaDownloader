using System;
using System.Data;
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
        }
        private void BtnOpenDirectory_Click(object sender, EventArgs e)
        {
            string userPath = ParentClass.PluginUI.textUserDir.Text;

            try
            {
                string parentParentDir = Path.GetDirectoryName(Path.GetDirectoryName(userPath));
                if ((string.IsNullOrEmpty(userPath) || userPath.Contains("自动设置失败") || !Directory.Exists(parentParentDir) && !Directory.Exists(userPath)))
                {
                    throw new Exception("无效的用户目录！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(userPath))
            {
                try
                {
                    Directory.CreateDirectory(userPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("无法创建 cactbot 用户目录：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (Directory.Exists(userPath))
            {
                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", userPath);
                }
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



        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            foreach (var item in checkedListBox1.Items.Cast<string>().Where(item => !item.Contains("必装")).ToList())
            {
                checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(item), true);
            }
        }

        private void DeselectAllButton_Click(object sender, EventArgs e)
        {
            foreach (var item in checkedListBox1.Items.Cast<string>().Where(item => !item.Contains("必装")).ToList())
            {
                checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(item), false);
            }
        }

        private void InvertSelectionButton_Click(object sender, EventArgs e)
        {
            foreach (var item in checkedListBox1.Items.Cast<string>().Where(item => !item.Contains("必装")).ToList())
            {
                checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(item), !checkedListBox1.GetItemChecked(checkedListBox1.Items.IndexOf(item)));
            }
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string item = checkedListBox1.Items[e.Index].ToString();
            if (item.Contains("必装"))
            {
                e.NewValue = CheckState.Checked;
            }
        }

        private void BtnFindDir_Click(object sender, EventArgs e)
        {
            ParentClass.AutoConfigureCactbotPath();

            if (string.IsNullOrEmpty(ParentClass.PluginUI.textUserDir.Text) || ParentClass.PluginUI.textUserDir.Text.Contains("自动设置失败"))
            {
                MessageBox.Show("无效的用户目录！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
