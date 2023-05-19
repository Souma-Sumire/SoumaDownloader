﻿using System.Drawing;
using System;
using System.Net.Http;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Linq;

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
            this.txtUserDir = new System.Windows.Forms.TextBox();
            this.btnOpenDir = new System.Windows.Forms.Button();
            this.btnFetch = new System.Windows.Forms.Button();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnToggleSelection = new System.Windows.Forms.Button();
            this.VersionInfo = new System.Windows.Forms.Label();
            this.labelUserDirectory = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFindDir = new System.Windows.Forms.Button();
            this.lblLastUpdateTime = new System.Windows.Forms.Label();
            this.textLastUpdateTime = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBoxOverwrite = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textLastFetchTime = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // txtUserDir
            // 
            this.txtUserDir.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserDir.Location = new System.Drawing.Point(96, 24);
            this.txtUserDir.Name = "txtUserDir";
            this.txtUserDir.Size = new System.Drawing.Size(440, 21);
            this.txtUserDir.TabIndex = 0;
            // 
            // btnOpenDir
            // 
            this.btnOpenDir.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOpenDir.Location = new System.Drawing.Point(542, 22);
            this.btnOpenDir.Name = "btnOpenDir";
            this.btnOpenDir.Size = new System.Drawing.Size(87, 23);
            this.btnOpenDir.TabIndex = 1;
            this.btnOpenDir.Text = "打开目录";
            this.btnOpenDir.UseVisualStyleBackColor = true;
            this.btnOpenDir.Click += new System.EventHandler(this.BtnOpenDirectory_Click);
            // 
            // btnFetch
            // 
            this.btnFetch.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFetch.Location = new System.Drawing.Point(5, 137);
            this.btnFetch.Name = "btnFetch";
            this.btnFetch.Size = new System.Drawing.Size(87, 23);
            this.btnFetch.TabIndex = 2;
            this.btnFetch.Text = "刷新上游列表";
            this.btnFetch.UseVisualStyleBackColor = true;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CausesValidation = false;
            this.checkedListBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(5, 164);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(531, 292);
            this.checkedListBox1.TabIndex = 3;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.CheckedListBox1_ItemCheck);
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDownload.Location = new System.Drawing.Point(5, 462);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(87, 30);
            this.btnDownload.TabIndex = 7;
            this.btnDownload.Text = "更新本地文件";
            this.btnDownload.UseVisualStyleBackColor = true;
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectAll.Location = new System.Drawing.Point(542, 164);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(87, 23);
            this.btnSelectAll.TabIndex = 4;
            this.btnSelectAll.Text = "全选";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.SelectAllButton_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeselectAll.Location = new System.Drawing.Point(542, 193);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(87, 23);
            this.btnDeselectAll.TabIndex = 5;
            this.btnDeselectAll.Text = "全部取消";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.DeselectAllButton_Click);
            // 
            // btnToggleSelection
            // 
            this.btnToggleSelection.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnToggleSelection.Location = new System.Drawing.Point(542, 222);
            this.btnToggleSelection.Name = "btnToggleSelection";
            this.btnToggleSelection.Size = new System.Drawing.Size(87, 23);
            this.btnToggleSelection.TabIndex = 6;
            this.btnToggleSelection.Text = "反选";
            this.btnToggleSelection.UseVisualStyleBackColor = true;
            this.btnToggleSelection.Click += new System.EventHandler(this.InvertSelectionButton_Click);
            // 
            // VersionInfo
            // 
            this.VersionInfo.AutoSize = true;
            this.VersionInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VersionInfo.Location = new System.Drawing.Point(542, 459);
            this.VersionInfo.Name = "VersionInfo";
            this.VersionInfo.Size = new System.Drawing.Size(77, 12);
            this.VersionInfo.TabIndex = 15;
            this.VersionInfo.Text = "版本 0.0.0.0";
            this.VersionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelUserDirectory
            // 
            this.labelUserDirectory.AutoSize = true;
            this.labelUserDirectory.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelUserDirectory.Location = new System.Drawing.Point(0, 4);
            this.labelUserDirectory.Name = "labelUserDirectory";
            this.labelUserDirectory.Size = new System.Drawing.Size(287, 12);
            this.labelUserDirectory.TabIndex = 9;
            this.labelUserDirectory.Text = "填写下载目录（...\\cactbot\\user\\raidboss\\Souma）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(2, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "选择需要的文件";
            // 
            // btnFindDir
            // 
            this.btnFindDir.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFindDir.Location = new System.Drawing.Point(3, 24);
            this.btnFindDir.Name = "btnFindDir";
            this.btnFindDir.Size = new System.Drawing.Size(87, 23);
            this.btnFindDir.TabIndex = 17;
            this.btnFindDir.Text = "自动识别";
            this.btnFindDir.UseVisualStyleBackColor = true;
            this.btnFindDir.Click += new System.EventHandler(this.BtnFindDir_Click);
            // 
            // lblLastUpdateTime
            // 
            this.lblLastUpdateTime.AutoSize = true;
            this.lblLastUpdateTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblLastUpdateTime.Location = new System.Drawing.Point(330, 462);
            this.lblLastUpdateTime.Name = "lblLastUpdateTime";
            this.lblLastUpdateTime.Size = new System.Drawing.Size(65, 12);
            this.lblLastUpdateTime.TabIndex = 18;
            this.lblLastUpdateTime.Text = "上次更新：";
            // 
            // textLastUpdateTime
            // 
            this.textLastUpdateTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textLastUpdateTime.Location = new System.Drawing.Point(394, 459);
            this.textLastUpdateTime.Name = "textLastUpdateTime";
            this.textLastUpdateTime.ReadOnly = true;
            this.textLastUpdateTime.Size = new System.Drawing.Size(136, 21);
            this.textLastUpdateTime.TabIndex = 19;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(5, 75);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(531, 21);
            this.textBox1.TabIndex = 20;
            this.textBox1.Text = "https://souma.diemoe.net/ff14-overlay-vite/#/cactbotRuntime";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(3, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 21;
            this.label1.Text = "并在悬浮窗插件中添加此链接";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLabel1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel1.Location = new System.Drawing.Point(578, 444);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(41, 12);
            this.linkLabel1.TabIndex = 22;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Github";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // checkBoxOverwrite
            // 
            this.checkBoxOverwrite.AutoSize = true;
            this.checkBoxOverwrite.Checked = true;
            this.checkBoxOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOverwrite.Location = new System.Drawing.Point(98, 471);
            this.checkBoxOverwrite.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxOverwrite.Name = "checkBoxOverwrite";
            this.checkBoxOverwrite.Size = new System.Drawing.Size(156, 16);
            this.checkBoxOverwrite.TabIndex = 23;
            this.checkBoxOverwrite.Text = "覆盖式更新（推荐勾选）";
            this.checkBoxOverwrite.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(336, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 24;
            this.label3.Text = "上次刷新：";
            // 
            // textLastFetchTime
            // 
            this.textLastFetchTime.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textLastFetchTime.Location = new System.Drawing.Point(400, 137);
            this.textLastFetchTime.Name = "textLastFetchTime";
            this.textLastFetchTime.ReadOnly = true;
            this.textLastFetchTime.Size = new System.Drawing.Size(136, 21);
            this.textLastFetchTime.TabIndex = 25;
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 0;
            // 
            // UIControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textLastFetchTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkBoxOverwrite);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textLastUpdateTime);
            this.Controls.Add(this.lblLastUpdateTime);
            this.Controls.Add(this.btnFindDir);
            this.Controls.Add(this.VersionInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.btnFetch);
            this.Controls.Add(this.btnOpenDir);
            this.Controls.Add(this.txtUserDir);
            this.Controls.Add(this.labelUserDirectory);
            this.Controls.Add(this.btnToggleSelection);
            this.Controls.Add(this.btnDeselectAll);
            this.Controls.Add(this.btnSelectAll);
            this.Name = "UIControl";
            this.Size = new System.Drawing.Size(1176, 771);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtUserDir;
        public System.Windows.Forms.Button btnOpenDir;
        public System.Windows.Forms.Button btnFetch;
        public System.Windows.Forms.CheckedListBox checkedListBox1;
        public System.Windows.Forms.Button btnDownload;
        public System.Windows.Forms.Button btnDeselectAll;
        public System.Windows.Forms.Button btnToggleSelection;
        public System.Windows.Forms.Button btnSelectAll;
        public Label VersionInfo;
        public Label labelUserDirectory;
        public Label label2;
        public Button btnFindDir;
        private Label lblLastUpdateTime;
        public TextBox textLastUpdateTime;
        private TextBox textBox1;
        private Label label1;
        private LinkLabel linkLabel1;
        public CheckBox checkBoxOverwrite;
        private Label label3;
        public TextBox textLastFetchTime;
        public ToolTip toolTip1;
    }
}
