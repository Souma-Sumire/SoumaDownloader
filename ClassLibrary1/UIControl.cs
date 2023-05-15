using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
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
            if (!Directory.Exists(ParentClass.PluginUI.txtUserDir.Text))
            {
                try
                {
                    Directory.CreateDirectory(ParentClass.PluginUI.txtUserDir.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to create cactbotUserDirectory: " + ex.Message);
                    return;
                }
            }

            if (Directory.Exists(ParentClass.PluginUI.txtUserDir.Text))
            {
                try
                {
                    System.Diagnostics.Process.Start("explorer.exe", ParentClass.PluginUI.txtUserDir.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to open cactbotUserDirectory: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("cactbotUserDirectory does not exist.");
            }
        }


        private void SelectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string itemName = checkedListBox1.Items[i].ToString();
                if (!itemName.Contains("必装"))
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
        }

        private void DeselectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string itemName = checkedListBox1.Items[i].ToString();
                if (!itemName.Contains("必装"))
                {
                    checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        private void InvertSelectionButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                string itemName = checkedListBox1.Items[i].ToString();
                if (!itemName.Contains("必装"))
                {
                    checkedListBox1.SetItemChecked(i, !checkedListBox1.GetItemChecked(i));
                }
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

        }
    }
}
