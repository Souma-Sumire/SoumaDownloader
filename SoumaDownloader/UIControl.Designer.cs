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
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelUserDirectory = new System.Windows.Forms.Label();
            this.textUserDir = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnOpenDir = new System.Windows.Forms.Button();
            this.btnFindDir = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnInvert = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colLastModified = new System.Windows.Forms.ColumnHeader();
            this.colDesc = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.textLastUpdateTime = new System.Windows.Forms.Label();
            this.VersionInfo = new System.Windows.Forms.Label();
            this.retry = new System.Windows.Forms.Button();
            this.linkGithub = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();

            // labelUserDirectory
            this.labelUserDirectory.AutoSize = true;
            this.labelUserDirectory.Location = new System.Drawing.Point(0, 8);
            this.labelUserDirectory.Margin = new System.Windows.Forms.Padding(0);
            this.labelUserDirectory.Name = "labelUserDirectory";
            this.labelUserDirectory.TabIndex = 0;
            this.labelUserDirectory.Text = "本地文件夹路径";

            // textUserDir
            this.textUserDir.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textUserDir.Location = new System.Drawing.Point(0, 28);
            this.textUserDir.Margin = new System.Windows.Forms.Padding(0);
            this.textUserDir.Name = "textUserDir";
            this.textUserDir.Size = new System.Drawing.Size(690, 25);
            this.textUserDir.TabIndex = 1;

            // btnBrowse
            this.btnBrowse.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowse.Location = new System.Drawing.Point(695, 28);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(95, 25);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "浏览...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);

            // btnOpenDir
            this.btnOpenDir.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenDir.Location = new System.Drawing.Point(795, 28);
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.Size = new System.Drawing.Size(95, 25);
            this.btnOpenDir.TabIndex = 3;
            this.btnOpenDir.Text = "打开目录";
            this.btnOpenDir.UseVisualStyleBackColor = true;
            this.btnOpenDir.Click += new System.EventHandler(this.BtnOpenDirectory_Click);

            // btnFindDir
            this.btnFindDir.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFindDir.Location = new System.Drawing.Point(895, 28);
            this.btnFindDir.Name = "btnFindDir";
            this.btnFindDir.Size = new System.Drawing.Size(95, 25);
            this.btnFindDir.TabIndex = 4;
            this.btnFindDir.Text = "自动检测";
            this.btnFindDir.UseVisualStyleBackColor = true;
            this.btnFindDir.Click += new System.EventHandler(this.BtnFindDir_Click);

            // btnSelectAll
            this.btnSelectAll.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectAll.Location = new System.Drawing.Point(0, 60);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(65, 24);
            this.btnSelectAll.TabIndex = 5;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.Visible = false;
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.SelectAllButton_Click);

            // btnDeselectAll
            this.btnDeselectAll.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeselectAll.Location = new System.Drawing.Point(70, 60);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(80, 24);
            this.btnDeselectAll.TabIndex = 6;
            this.btnDeselectAll.Text = "全部取消";
            this.btnDeselectAll.Visible = false;
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.DeselectAllButton_Click);

            // btnInvert
            this.btnInvert.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInvert.Location = new System.Drawing.Point(155, 60);
            this.btnInvert.Name = "btnInvert";
            this.btnInvert.Size = new System.Drawing.Size(55, 24);
            this.btnInvert.TabIndex = 7;
            this.btnInvert.Text = "反选";
            this.btnInvert.Visible = false;
            this.btnInvert.UseVisualStyleBackColor = true;
            this.btnInvert.Click += new System.EventHandler(this.BtnInvert_Click);

            // label2
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(220, 64);
            this.label2.Name = "label2";
            this.label2.TabIndex = 8;
            this.label2.Text = "已勾选的文件将被下载，未勾选的文件将被删除";
            this.label2.Visible = false;

            // colName
            this.colName.Text = "文件名";
            this.colName.Width = 290;

            // colStatus
            this.colStatus.Text = "状态";
            this.colStatus.Width = 75;

            // colLastModified
            this.colLastModified.Text = "最后更新";
            this.colLastModified.Width = 100;

            // colDesc
            this.colDesc.Text = "说明";
            this.colDesc.Width = 350;

            // listViewFiles
            this.listViewFiles.CheckBoxes = true;
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colName, this.colStatus, this.colLastModified, this.colDesc });
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.GridLines = true;
            this.listViewFiles.Location = new System.Drawing.Point(0, 88);
            this.listViewFiles.Margin = new System.Windows.Forms.Padding(0);
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(1000, 615);
            this.listViewFiles.TabIndex = 9;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            this.listViewFiles.Visible = false;
            this.listViewFiles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ListViewFiles_ItemCheck);

            // checkBoxAutoUpdate
            this.checkBoxAutoUpdate.AutoSize = true;
            this.checkBoxAutoUpdate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxAutoUpdate.Location = new System.Drawing.Point(0, 713);
            this.checkBoxAutoUpdate.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            this.checkBoxAutoUpdate.TabIndex = 10;
            this.checkBoxAutoUpdate.Text = "每次启动ACT时自动更新已有文件";
            this.checkBoxAutoUpdate.Visible = false;
            this.checkBoxAutoUpdate.UseVisualStyleBackColor = true;

            // btnDownload
            this.btnDownload.Enabled = false;
            this.btnDownload.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.Location = new System.Drawing.Point(280, 710);
            this.btnDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(718, 28);
            this.btnDownload.TabIndex = 11;
            this.btnDownload.Text = "开始下崽";
            this.btnDownload.Visible = false;
            this.btnDownload.UseVisualStyleBackColor = true;

            // textLastUpdateTime
            this.textLastUpdateTime.AutoSize = true;
            this.textLastUpdateTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textLastUpdateTime.Location = new System.Drawing.Point(0, 748);
            this.textLastUpdateTime.Margin = new System.Windows.Forms.Padding(0);
            this.textLastUpdateTime.Name = "textLastUpdateTime";
            this.textLastUpdateTime.TabIndex = 12;
            this.textLastUpdateTime.Text = "还没有下崽记录";
            this.textLastUpdateTime.Visible = false;

            // VersionInfo
            this.VersionInfo.AutoSize = true;
            this.VersionInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VersionInfo.Location = new System.Drawing.Point(870, 748);
            this.VersionInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.VersionInfo.Name = "VersionInfo";
            this.VersionInfo.TabIndex = 13;
            this.VersionInfo.Text = "版本 0.0.0.0";
            this.VersionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // retry
            this.retry.Location = new System.Drawing.Point(0, 88);
            this.retry.Margin = new System.Windows.Forms.Padding(0);
            this.retry.Name = "retry";
            this.retry.Size = new System.Drawing.Size(1000, 615);
            this.retry.TabIndex = 0;
            this.retry.Text = "请求列表失败，点击重试。";
            this.retry.UseVisualStyleBackColor = true;

            // linkGithub
            this.linkGithub.AutoSize = true;
            this.linkGithub.Location = new System.Drawing.Point(0, 772);
            this.linkGithub.Name = "linkGithub";
            this.linkGithub.TabIndex = 14;
            this.linkGithub.TabStop = true;
            this.linkGithub.Text = "GitHub: raidboss-user-js-public";
            this.linkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkGithub_LinkClicked);

            // toolTip1
            this.toolTip1.AutomaticDelay = 0;

            // UIControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.retry);
            this.Controls.Add(this.listViewFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnInvert);
            this.Controls.Add(this.btnDeselectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnFindDir);
            this.Controls.Add(this.btnOpenDir);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.textUserDir);
            this.Controls.Add(this.labelUserDirectory);
            this.Controls.Add(this.checkBoxAutoUpdate);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.textLastUpdateTime);
            this.Controls.Add(this.VersionInfo);
            this.Controls.Add(this.linkGithub);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UIControl";
            this.Size = new System.Drawing.Size(1006, 810);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        public System.Windows.Forms.TextBox textUserDir;
        public System.Windows.Forms.Button btnOpenDir;
        public System.Windows.Forms.Button btnBrowse;
        public System.Windows.Forms.Button btnFindDir;
        public System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colLastModified;
        private System.Windows.Forms.ColumnHeader colDesc;
        private System.Windows.Forms.ColumnHeader colStatus;
        public System.Windows.Forms.Button btnDownload;
        public System.Windows.Forms.Button btnDeselectAll;
        public System.Windows.Forms.Button btnSelectAll;
        public System.Windows.Forms.Button btnInvert;
        public System.Windows.Forms.Label VersionInfo;
        public System.Windows.Forms.Label labelUserDirectory;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.CheckBox checkBoxAutoUpdate;
        public System.Windows.Forms.Label textLastUpdateTime;
        public System.Windows.Forms.Button retry;
        public System.Windows.Forms.LinkLabel linkGithub;
    }
}
