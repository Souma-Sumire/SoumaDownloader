using System.Windows.Forms;

namespace SoumaDownloader
{
    partial class UIControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textUserDir = new System.Windows.Forms.TextBox();
            this.btnOpenDir = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.VersionInfo = new System.Windows.Forms.Label();
            this.labelUserDirectory = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.textLastUpdateTime = new System.Windows.Forms.Label();
            this.retry = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textUserDir
            // 
            this.textUserDir.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textUserDir.Location = new System.Drawing.Point(0, 32);
            this.textUserDir.Margin = new System.Windows.Forms.Padding(0);
            this.textUserDir.Name = "textUserDir";
            this.textUserDir.Size = new System.Drawing.Size(854, 25);
            this.textUserDir.TabIndex = 2;
            // 
            // btnOpenDir
            // 
            this.btnOpenDir.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenDir.Location = new System.Drawing.Point(865, 32);
            this.btnOpenDir.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.Size = new System.Drawing.Size(114, 29);
            this.btnOpenDir.TabIndex = 3;
            this.btnOpenDir.Text = "打开目录";
            this.btnOpenDir.UseVisualStyleBackColor = true;
            this.btnOpenDir.Click += new System.EventHandler(this.BtnOpenDirectory_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CausesValidation = false;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.ColumnWidth = 371;
            this.checkedListBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(2, 85);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(854, 344);
            this.checkedListBox1.TabIndex = 5;
            this.checkedListBox1.Visible = false;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBox1_ItemCheck);
            // 
            // btnDownload
            // 
            this.btnDownload.Enabled = false;
            this.btnDownload.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.Location = new System.Drawing.Point(0, 437);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(854, 29);
            this.btnDownload.TabIndex = 8;
            this.btnDownload.Text = "开始下崽";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Visible = false;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectAll.Location = new System.Drawing.Point(865, 85);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(114, 29);
            this.btnSelectAll.TabIndex = 6;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Visible = false;
            this.btnSelectAll.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeselectAll.Location = new System.Drawing.Point(865, 122);
            this.btnDeselectAll.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(114, 29);
            this.btnDeselectAll.TabIndex = 7;
            this.btnDeselectAll.Text = "全部取消";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Visible = false;
            this.btnDeselectAll.Click += new System.EventHandler(this.DeselectAllButton_Click);
            // 
            // VersionInfo
            // 
            this.VersionInfo.AutoSize = true;
            this.VersionInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VersionInfo.Location = new System.Drawing.Point(753, 477);
            this.VersionInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.VersionInfo.Name = "VersionInfo";
            this.VersionInfo.Size = new System.Drawing.Size(101, 15);
            this.VersionInfo.TabIndex = 10;
            this.VersionInfo.Text = "版本 0.0.0.0";
            this.VersionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelUserDirectory
            // 
            this.labelUserDirectory.AutoSize = true;
            this.labelUserDirectory.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelUserDirectory.Location = new System.Drawing.Point(0, 13);
            this.labelUserDirectory.Margin = new System.Windows.Forms.Padding(0);
            this.labelUserDirectory.Name = "labelUserDirectory";
            this.labelUserDirectory.Size = new System.Drawing.Size(112, 15);
            this.labelUserDirectory.TabIndex = 1;
            this.labelUserDirectory.Text = "本地文件夹路径";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(0, 67);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "勾选需要的文件";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            // 
            // checkBoxAutoUpdate
            // 
            this.checkBoxAutoUpdate.AutoSize = true;
            this.checkBoxAutoUpdate.Location = new System.Drawing.Point(3, 473);
            this.checkBoxAutoUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            this.checkBoxAutoUpdate.Size = new System.Drawing.Size(248, 19);
            this.checkBoxAutoUpdate.TabIndex = 9;
            this.checkBoxAutoUpdate.Text = "每次启动ACT时自动更新已有文件";
            this.checkBoxAutoUpdate.UseVisualStyleBackColor = true;
            this.checkBoxAutoUpdate.Visible = false;
            // 
            // textLastUpdateTime
            // 
            this.textLastUpdateTime.AutoSize = true;
            this.textLastUpdateTime.Location = new System.Drawing.Point(0, 495);
            this.textLastUpdateTime.Margin = new System.Windows.Forms.Padding(0);
            this.textLastUpdateTime.Name = "textLastUpdateTime";
            this.textLastUpdateTime.Size = new System.Drawing.Size(112, 15);
            this.textLastUpdateTime.TabIndex = 11;
            this.textLastUpdateTime.Text = "还没有下崽记录";
            this.textLastUpdateTime.Visible = false;
            // 
            // retry
            // 
            this.retry.Location = new System.Drawing.Point(0, 85);
            this.retry.Margin = new System.Windows.Forms.Padding(0);
            this.retry.Name = "retry";
            this.retry.Size = new System.Drawing.Size(856, 345);
            this.retry.TabIndex = 0;
            this.retry.Text = "请求列表失败，点击重试。";
            this.retry.UseVisualStyleBackColor = true;
            // 
            // UIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.retry);
            this.Controls.Add(this.textLastUpdateTime);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.btnOpenDir);
            this.Controls.Add(this.VersionInfo);
            this.Controls.Add(this.textUserDir);
            this.Controls.Add(this.labelUserDirectory);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.checkBoxAutoUpdate);
            this.Controls.Add(this.btnDeselectAll);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UIControl";
            this.Size = new System.Drawing.Size(1006, 610);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textUserDir;
        public System.Windows.Forms.Button btnOpenDir;
        public System.Windows.Forms.CheckedListBox checkedListBox1;
        public System.Windows.Forms.Button btnDownload;
        public System.Windows.Forms.Button btnDeselectAll;
        public System.Windows.Forms.Button btnSelectAll;
        public Label VersionInfo;
        public Label labelUserDirectory;
        public Label label2;
        public ToolTip toolTip1;
        public CheckBox checkBoxAutoUpdate;
        public Label textLastUpdateTime;
        public Button retry;
    }
}
