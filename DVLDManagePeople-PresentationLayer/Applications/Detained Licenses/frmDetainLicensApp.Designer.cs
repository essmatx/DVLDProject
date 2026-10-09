namespace DVLDManagePeople_PresentationLayer
{
    partial class frmDetainLicensApp
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
            this.llShowLicenseinfo = new System.Windows.Forms.LinkLabel();
            this.llShowLicenseHistory = new System.Windows.Forms.LinkLabel();
            this.guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.ucLicenseInfo1 = new DVLDManagePeople_PresentationLayer.ucLicenseInfo();
            this.ucDetainLicenseInfo1 = new DVLDManagePeople_PresentationLayer.ucDetainLicenseInfo();
            this.ucLicenseSearchFilter1 = new DVLDManagePeople_PresentationLayer.ucLicenseSearchFilter();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.pictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.btnDetain = new Guna.UI2.WinForms.Guna2Button();
            this.picShield = new FontAwesome.Sharp.IconPictureBox();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // llShowLicenseinfo
            // 
            this.llShowLicenseinfo.AutoSize = true;
            this.llShowLicenseinfo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.llShowLicenseinfo.Location = new System.Drawing.Point(561, 894);
            this.llShowLicenseinfo.Name = "llShowLicenseinfo";
            this.llShowLicenseinfo.Size = new System.Drawing.Size(114, 16);
            this.llShowLicenseinfo.TabIndex = 6;
            this.llShowLicenseinfo.TabStop = true;
            this.llShowLicenseinfo.Text = "Show License Info";
            this.llShowLicenseinfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowLicenseinfo_LinkClicked);
            // 
            // llShowLicenseHistory
            // 
            this.llShowLicenseHistory.AutoSize = true;
            this.llShowLicenseHistory.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.llShowLicenseHistory.Location = new System.Drawing.Point(283, 894);
            this.llShowLicenseHistory.Name = "llShowLicenseHistory";
            this.llShowLicenseHistory.Size = new System.Drawing.Size(135, 16);
            this.llShowLicenseHistory.TabIndex = 7;
            this.llShowLicenseHistory.TabStop = true;
            this.llShowLicenseHistory.Text = "Show License History";
            this.llShowLicenseHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowLicenseHistory_LinkClicked);
            // 
            // guna2HtmlLabel5
            // 
            this.guna2HtmlLabel5.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel5.Font = new System.Drawing.Font("Georgia", 20F);
            this.guna2HtmlLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.guna2HtmlLabel5.Location = new System.Drawing.Point(319, 21);
            this.guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            this.guna2HtmlLabel5.Padding = new System.Windows.Forms.Padding(0, 0, 37, 0);
            this.guna2HtmlLabel5.Size = new System.Drawing.Size(247, 40);
            this.guna2HtmlLabel5.TabIndex = 113;
            this.guna2HtmlLabel5.Text = "Detain license";
            this.guna2HtmlLabel5.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ucLicenseInfo1
            // 
            this.ucLicenseInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucLicenseInfo1.Location = new System.Drawing.Point(22, 178);
            this.ucLicenseInfo1.Name = "ucLicenseInfo1";
            this.ucLicenseInfo1.Size = new System.Drawing.Size(918, 396);
            this.ucLicenseInfo1.TabIndex = 0;
            // 
            // ucDetainLicenseInfo1
            // 
            this.ucDetainLicenseInfo1.Location = new System.Drawing.Point(22, 580);
            this.ucDetainLicenseInfo1.Name = "ucDetainLicenseInfo1";
            this.ucDetainLicenseInfo1.Size = new System.Drawing.Size(918, 288);
            this.ucDetainLicenseInfo1.TabIndex = 0;
            // 
            // ucLicenseSearchFilter1
            // 
            this.ucLicenseSearchFilter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ucLicenseSearchFilter1.Location = new System.Drawing.Point(22, 108);
            this.ucLicenseSearchFilter1.Name = "ucLicenseSearchFilter1";
            this.ucLicenseSearchFilter1.Size = new System.Drawing.Size(437, 64);
            this.ucLicenseSearchFilter1.TabIndex = 1;
            this.ucLicenseSearchFilter1.Load += new System.EventHandler(this.ucLicenseSearchFilter1_Load);
            // 
            // btnClose
            // 
            this.btnClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.btnClose.BorderRadius = 6;
            this.btnClose.BorderThickness = 1;
            this.btnClose.CustomBorderColor = System.Drawing.Color.Silver;
            this.btnClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClose.FillColor = System.Drawing.Color.Black;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(22, 894);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(169, 36);
            this.btnClose.TabIndex = 115;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BorderRadius = 10;
            this.pictureBox1.Image = global::DVLDManagePeople_PresentationLayer.Properties.Resources.ChatGPT_Image_Aug_16__2026__01_19_13_AM;
            this.pictureBox1.ImageRotate = 0F;
            this.pictureBox1.Location = new System.Drawing.Point(734, 620);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(168, 166);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 116;
            this.pictureBox1.TabStop = false;
            // 
            // btnDetain
            // 
            this.btnDetain.BorderRadius = 10;
            this.btnDetain.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDetain.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDetain.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDetain.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDetain.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.btnDetain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnDetain.ForeColor = System.Drawing.Color.Black;
            this.btnDetain.Image = global::DVLDManagePeople_PresentationLayer.Properties.Resources.Gemini_Generated_Image_ks9laaks9laaks9l;
            this.btnDetain.Location = new System.Drawing.Point(784, 892);
            this.btnDetain.Name = "btnDetain";
            this.btnDetain.Size = new System.Drawing.Size(156, 38);
            this.btnDetain.TabIndex = 114;
            this.btnDetain.Text = "Detain License";
            this.btnDetain.Click += new System.EventHandler(this.btnDetain_Click);
            // 
            // picShield
            // 
            this.picShield.BackColor = System.Drawing.Color.Transparent;
            this.picShield.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconChar = FontAwesome.Sharp.IconChar.IdCard;
            this.picShield.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.picShield.IconSize = 78;
            this.picShield.Location = new System.Drawing.Point(771, 21);
            this.picShield.Name = "picShield";
            this.picShield.Size = new System.Drawing.Size(169, 78);
            this.picShield.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picShield.TabIndex = 117;
            this.picShield.TabStop = false;
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.IdCardAlt;
            this.iconPictureBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 90;
            this.iconPictureBox1.Location = new System.Drawing.Point(784, 436);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(90, 90);
            this.iconPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPictureBox1.TabIndex = 118;
            this.iconPictureBox1.TabStop = false;
            // 
            // frmDetainLicensApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(955, 944);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.picShield);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDetain);
            this.Controls.Add(this.guna2HtmlLabel5);
            this.Controls.Add(this.ucLicenseInfo1);
            this.Controls.Add(this.ucDetainLicenseInfo1);
            this.Controls.Add(this.llShowLicenseHistory);
            this.Controls.Add(this.llShowLicenseinfo);
            this.Controls.Add(this.ucLicenseSearchFilter1);
            this.Name = "frmDetainLicensApp";
            this.Text = "frmDetainLicensApp";
            this.Load += new System.EventHandler(this.frmDetainLicensApp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ucLicenseSearchFilter ucLicenseSearchFilter1;
        private ucDetainLicenseInfo ucDetainLicenseInfo1;
        private System.Windows.Forms.LinkLabel llShowLicenseinfo;
        private System.Windows.Forms.LinkLabel llShowLicenseHistory;
        private ucLicenseInfo ucLicenseInfo1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel5;
        private Guna.UI2.WinForms.Guna2Button btnDetain;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2PictureBox pictureBox1;
        private FontAwesome.Sharp.IconPictureBox picShield;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
    }
}