
namespace Client
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ImageLabel = new System.Windows.Forms.Label();
            this.detailButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.backBtn = new System.Windows.Forms.Button();
            this.getFileBtn = new System.Windows.Forms.Button();
            this.filenameBox = new System.Windows.Forms.TextBox();
            this.deleteBtn = new System.Windows.Forms.Button();
            this.downloadBtn = new System.Windows.Forms.Button();
            this.renameBtn = new System.Windows.Forms.Button();
            this.DetailLabel = new System.Windows.Forms.Label();
            this.uploadBtn = new System.Windows.Forms.Button();
            this.fileListBox = new System.Windows.Forms.ListBox();
            this.closeBtn = new System.Windows.Forms.Button();
            this.line1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ImageLabel
            // 
            this.ImageLabel.Location = new System.Drawing.Point(180, 266);
            this.ImageLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ImageLabel.Name = "ImageLabel";
            this.ImageLabel.Size = new System.Drawing.Size(195, 88);
            this.ImageLabel.TabIndex = 44;
            // 
            // detailButton
            // 
            this.detailButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(111)))), ((int)(((byte)(255)))));
            this.detailButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.detailButton.FlatAppearance.BorderSize = 0;
            this.detailButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.detailButton.Font = new System.Drawing.Font("等线", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.detailButton.ForeColor = System.Drawing.SystemColors.Window;
            this.detailButton.Location = new System.Drawing.Point(10, 361);
            this.detailButton.Margin = new System.Windows.Forms.Padding(2);
            this.detailButton.Name = "detailButton";
            this.detailButton.Size = new System.Drawing.Size(49, 28);
            this.detailButton.TabIndex = 42;
            this.detailButton.Text = "细节";
            this.detailButton.UseVisualStyleBackColor = false;
            this.detailButton.Click += new System.EventHandler(this.detailButton_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.titleLabel.Location = new System.Drawing.Point(182, 58);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(381, 32);
            this.titleLabel.TabIndex = 41;
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.label5.Location = new System.Drawing.Point(180, 382);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(270, 1);
            this.label5.TabIndex = 40;
            // 
            // InfoLabel
            // 
            this.InfoLabel.Enabled = false;
            this.InfoLabel.Font = new System.Drawing.Font("等线", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InfoLabel.Location = new System.Drawing.Point(380, 12);
            this.InfoLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Padding = new System.Windows.Forms.Padding(0, 0, 4, 0);
            this.InfoLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.InfoLabel.Size = new System.Drawing.Size(158, 16);
            this.InfoLabel.TabIndex = 39;
            this.InfoLabel.Text = "Welcome";
            this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // backBtn
            // 
            this.backBtn.BackColor = System.Drawing.SystemColors.Control;
            this.backBtn.BackgroundImage = global::Client.Properties.Resources.back;
            this.backBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.backBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.backBtn.FlatAppearance.BorderSize = 0;
            this.backBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.backBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.backBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backBtn.Location = new System.Drawing.Point(6, 13);
            this.backBtn.Margin = new System.Windows.Forms.Padding(2);
            this.backBtn.Name = "backBtn";
            this.backBtn.Size = new System.Drawing.Size(15, 16);
            this.backBtn.TabIndex = 37;
            this.backBtn.UseVisualStyleBackColor = false;
            this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            // 
            // getFileBtn
            // 
            this.getFileBtn.BackColor = System.Drawing.Color.White;
            this.getFileBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.getFileBtn.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.getFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.getFileBtn.Font = new System.Drawing.Font("等线", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.getFileBtn.ForeColor = System.Drawing.Color.Black;
            this.getFileBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.getFileBtn.Location = new System.Drawing.Point(278, 8);
            this.getFileBtn.Margin = new System.Windows.Forms.Padding(2);
            this.getFileBtn.Name = "getFileBtn";
            this.getFileBtn.Size = new System.Drawing.Size(80, 28);
            this.getFileBtn.TabIndex = 36;
            this.getFileBtn.Text = "查看所有";
            this.getFileBtn.UseVisualStyleBackColor = false;
            this.getFileBtn.Click += new System.EventHandler(this.getFileBtn_Click);
            // 
            // filenameBox
            // 
            this.filenameBox.BackColor = System.Drawing.Color.White;
            this.filenameBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filenameBox.Font = new System.Drawing.Font("等线 Light", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filenameBox.Location = new System.Drawing.Point(180, 366);
            this.filenameBox.Margin = new System.Windows.Forms.Padding(2);
            this.filenameBox.Name = "filenameBox";
            this.filenameBox.Size = new System.Drawing.Size(270, 16);
            this.filenameBox.TabIndex = 35;
            // 
            // deleteBtn
            // 
            this.deleteBtn.BackColor = System.Drawing.Color.Silver;
            this.deleteBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deleteBtn.FlatAppearance.BorderSize = 0;
            this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteBtn.Font = new System.Drawing.Font("等线", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.deleteBtn.ForeColor = System.Drawing.SystemColors.Window;
            this.deleteBtn.Location = new System.Drawing.Point(114, 361);
            this.deleteBtn.Margin = new System.Windows.Forms.Padding(2);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(49, 28);
            this.deleteBtn.TabIndex = 33;
            this.deleteBtn.Text = "删除";
            this.deleteBtn.UseVisualStyleBackColor = false;
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // downloadBtn
            // 
            this.downloadBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(111)))), ((int)(((byte)(255)))));
            this.downloadBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.downloadBtn.FlatAppearance.BorderSize = 0;
            this.downloadBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.downloadBtn.Font = new System.Drawing.Font("等线", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.downloadBtn.ForeColor = System.Drawing.SystemColors.Window;
            this.downloadBtn.Location = new System.Drawing.Point(62, 361);
            this.downloadBtn.Margin = new System.Windows.Forms.Padding(2);
            this.downloadBtn.Name = "downloadBtn";
            this.downloadBtn.Size = new System.Drawing.Size(49, 28);
            this.downloadBtn.TabIndex = 32;
            this.downloadBtn.Text = "下载";
            this.downloadBtn.UseVisualStyleBackColor = false;
            this.downloadBtn.Click += new System.EventHandler(this.downloadBtn_Click);
            // 
            // renameBtn
            // 
            this.renameBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(111)))), ((int)(((byte)(255)))));
            this.renameBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.renameBtn.FlatAppearance.BorderSize = 0;
            this.renameBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.renameBtn.Font = new System.Drawing.Font("等线", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.renameBtn.ForeColor = System.Drawing.SystemColors.Window;
            this.renameBtn.Location = new System.Drawing.Point(494, 361);
            this.renameBtn.Margin = new System.Windows.Forms.Padding(2);
            this.renameBtn.Name = "renameBtn";
            this.renameBtn.Size = new System.Drawing.Size(68, 28);
            this.renameBtn.TabIndex = 31;
            this.renameBtn.Text = "重命名";
            this.renameBtn.UseVisualStyleBackColor = false;
            this.renameBtn.Click += new System.EventHandler(this.renameBtn_Click);
            // 
            // DetailLabel
            // 
            this.DetailLabel.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.DetailLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.DetailLabel.Location = new System.Drawing.Point(182, 100);
            this.DetailLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.DetailLabel.Name = "DetailLabel";
            this.DetailLabel.Size = new System.Drawing.Size(380, 139);
            this.DetailLabel.TabIndex = 30;
            // 
            // uploadBtn
            // 
            this.uploadBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(111)))), ((int)(((byte)(255)))));
            this.uploadBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uploadBtn.FlatAppearance.BorderSize = 0;
            this.uploadBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uploadBtn.Font = new System.Drawing.Font("等线", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uploadBtn.ForeColor = System.Drawing.SystemColors.Window;
            this.uploadBtn.Location = new System.Drawing.Point(178, 8);
            this.uploadBtn.Margin = new System.Windows.Forms.Padding(2);
            this.uploadBtn.Name = "uploadBtn";
            this.uploadBtn.Size = new System.Drawing.Size(82, 28);
            this.uploadBtn.TabIndex = 29;
            this.uploadBtn.Text = "上传文件";
            this.uploadBtn.UseVisualStyleBackColor = false;
            this.uploadBtn.Click += new System.EventHandler(this.uploadBtn_Click);
            // 
            // fileListBox
            // 
            this.fileListBox.BackColor = System.Drawing.SystemColors.Control;
            this.fileListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fileListBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.fileListBox.Font = new System.Drawing.Font("等线", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.fileListBox.FormattingEnabled = true;
            this.fileListBox.IntegralHeight = false;
            this.fileListBox.ItemHeight = 16;
            this.fileListBox.Location = new System.Drawing.Point(2, 50);
            this.fileListBox.Margin = new System.Windows.Forms.Padding(8);
            this.fileListBox.Name = "fileListBox";
            this.fileListBox.Size = new System.Drawing.Size(169, 304);
            this.fileListBox.TabIndex = 27;
            this.fileListBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.fileListBox_DrawItem);
            this.fileListBox.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.fileListBox_MeasureItem);
            this.fileListBox.SelectedIndexChanged += new System.EventHandler(this.fileListBox_SelectedIndexChanged);
            // 
            // closeBtn
            // 
            this.closeBtn.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.closeBtn.BackgroundImage = global::Client.Properties.Resources.close;
            this.closeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.closeBtn.Cursor = System.Windows.Forms.Cursors.Default;
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.closeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Location = new System.Drawing.Point(547, 12);
            this.closeBtn.Margin = new System.Windows.Forms.Padding(2);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(15, 16);
            this.closeBtn.TabIndex = 26;
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            this.closeBtn.MouseEnter += new System.EventHandler(this.closeBtn_MouseEnter);
            this.closeBtn.MouseLeave += new System.EventHandler(this.closeBtn_MouseLeave);
            // 
            // line1
            // 
            this.line1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.line1.Location = new System.Drawing.Point(174, 43);
            this.line1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.line1.Name = "line1";
            this.line1.Size = new System.Drawing.Size(394, 1);
            this.line1.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Enabled = false;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("等线", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(2, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label4.Size = new System.Drawing.Size(169, 44);
            this.label4.TabIndex = 34;
            this.label4.Text = "我的文件";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(2, 43);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(169, 18);
            this.label3.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(2, 354);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 47);
            this.label1.TabIndex = 43;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(570, 400);
            this.Controls.Add(this.ImageLabel);
            this.Controls.Add(this.detailButton);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.InfoLabel);
            this.Controls.Add(this.backBtn);
            this.Controls.Add(this.getFileBtn);
            this.Controls.Add(this.filenameBox);
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.downloadBtn);
            this.Controls.Add(this.renameBtn);
            this.Controls.Add(this.DetailLabel);
            this.Controls.Add(this.uploadBtn);
            this.Controls.Add(this.fileListBox);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.line1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainWindow";
            this.Text = "主页";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWindow_FormClosed);
            this.VisibleChanged += new System.EventHandler(this.MainWindow_VisibleChanged);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ImageLabel;
        private System.Windows.Forms.Button detailButton;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button backBtn;
        private System.Windows.Forms.Button getFileBtn;
        private System.Windows.Forms.TextBox filenameBox;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button downloadBtn;
        private System.Windows.Forms.Button renameBtn;
        private System.Windows.Forms.Label DetailLabel;
        private System.Windows.Forms.Button uploadBtn;
        private System.Windows.Forms.ListBox fileListBox;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Label line1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
    }
}