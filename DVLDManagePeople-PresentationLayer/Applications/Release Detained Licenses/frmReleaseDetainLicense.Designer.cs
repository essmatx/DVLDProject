namespace DVLDManagePeople_PresentationLayer
{
    partial class frmReleaseDetainLicense
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
            this.ucLicenseSearchFilter1 = new DVLDManagePeople_PresentationLayer.ucLicenseSearchFilter();
            this.ucLicenseInfo1 = new DVLDManagePeople_PresentationLayer.ucLicenseInfo();
            this.ucShowDetainInfo1 = new DVLDManagePeople_PresentationLayer.ucShowDetainInfo();
            this.llShowLicenseInfo = new System.Windows.Forms.LinkLabel();
            this.llShowLicenseHistory = new System.Windows.Forms.LinkLabel();
            this.guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.pictureBox1 = new Guna.UI2.WinForms.Guna2PictureBox();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.picShield = new FontAwesome.Sharp.IconPictureBox();
            this.btnRelease = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).BeginInit();
            this.SuspendLayout();
            // 
            // ucLicenseSearchFilter1
            // 
            this.ucLicenseSearchFilter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ucLicenseSearchFilter1.Location = new System.Drawing.Point(12, 67);
            this.ucLicenseSearchFilter1.Name = "ucLicenseSearchFilter1";
            this.ucLicenseSearchFilter1.Size = new System.Drawing.Size(450, 67);
            this.ucLicenseSearchFilter1.TabIndex = 1;
            // 
            // ucLicenseInfo1
            // 
            this.ucLicenseInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucLicenseInfo1.Location = new System.Drawing.Point(12, 140);
            this.ucLicenseInfo1.Name = "ucLicenseInfo1";
            this.ucLicenseInfo1.Size = new System.Drawing.Size(911, 611);
            this.ucLicenseInfo1.TabIndex = 0;
            // 
            // ucShowDetainInfo1
            // 
            this.ucShowDetainInfo1.Location = new System.Drawing.Point(10, 757);
            this.ucShowDetainInfo1.Name = "ucShowDetainInfo1";
            this.ucShowDetainInfo1.Size = new System.Drawing.Size(911, 247);
            this.ucShowDetainInfo1.TabIndex = 0;
            // 
            // llShowLicenseInfo
            // 
            this.llShowLicenseInfo.AutoSize = true;
            this.llShowLicenseInfo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.llShowLicenseInfo.Location = new System.Drawing.Point(509, 1020);
            this.llShowLicenseInfo.Name = "llShowLicenseInfo";
            this.llShowLicenseInfo.Size = new System.Drawing.Size(114, 16);
            this.llShowLicenseInfo.TabIndex = 6;
            this.llShowLicenseInfo.TabStop = true;
            this.llShowLicenseInfo.Text = "Show License Info";
            this.llShowLicenseInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowLicenseInfo_LinkClicked);
            // 
            // llShowLicenseHistory
            // 
            this.llShowLicenseHistory.AutoSize = true;
            this.llShowLicenseHistory.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.llShowLicenseHistory.Location = new System.Drawing.Point(229, 1020);
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
            this.guna2HtmlLabel5.Location = new System.Drawing.Point(266, 12);
            this.guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            this.guna2HtmlLabel5.Padding = new System.Windows.Forms.Padding(0, 0, 37, 0);
            this.guna2HtmlLabel5.Size = new System.Drawing.Size(401, 40);
            this.guna2HtmlLabel5.TabIndex = 114;
            this.guna2HtmlLabel5.Text = "Release Detained license";
            this.guna2HtmlLabel5.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
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
            this.btnClose.Location = new System.Drawing.Point(12, 1012);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(169, 36);
            this.btnClose.TabIndex = 116;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BorderRadius = 10;
            this.pictureBox1.Image = global::DVLDManagePeople_PresentationLayer.Properties.Resources.Gemini_Generated_Image_b1fjkkb1fjkkb1fj;
            this.pictureBox1.ImageRotate = 0F;
            this.pictureBox1.Location = new System.Drawing.Point(810, 772);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(90, 84);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 120;
            this.pictureBox1.TabStop = false;
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.IdCardAlt;
            this.iconPictureBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 90;
            this.iconPictureBox1.Location = new System.Drawing.Point(810, 389);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(90, 90);
            this.iconPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPictureBox1.TabIndex = 119;
            this.iconPictureBox1.TabStop = false;
            // 
            // picShield
            // 
            this.picShield.BackColor = System.Drawing.Color.Transparent;
            this.picShield.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconChar = FontAwesome.Sharp.IconChar.IdCard;
            this.picShield.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.picShield.IconSize = 62;
            this.picShield.Location = new System.Drawing.Point(810, 21);
            this.picShield.Name = "picShield";
            this.picShield.Size = new System.Drawing.Size(111, 62);
            this.picShield.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picShield.TabIndex = 118;
            this.picShield.TabStop = false;
            // 
            // btnRelease
            // 
            this.btnRelease.BackColor = System.Drawing.Color.Transparent;
            this.btnRelease.BorderRadius = 10;
            this.btnRelease.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRelease.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRelease.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRelease.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRelease.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.btnRelease.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRelease.ForeColor = System.Drawing.Color.Black;
            this.btnRelease.Image = global::DVLDManagePeople_PresentationLayer.Properties.Resources.driving_license;
            this.btnRelease.Location = new System.Drawing.Point(761, 1010);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(160, 38);
            this.btnRelease.TabIndex = 115;
            this.btnRelease.Text = "Relaese License";
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // frmReleaseDetainLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(933, 1055);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.picShield);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRelease);
            this.Controls.Add(this.guna2HtmlLabel5);
            this.Controls.Add(this.ucShowDetainInfo1);
            this.Controls.Add(this.ucLicenseInfo1);
            this.Controls.Add(this.llShowLicenseHistory);
            this.Controls.Add(this.llShowLicenseInfo);
            this.Controls.Add(this.ucLicenseSearchFilter1);
            this.Name = "frmReleaseDetainLicense";
            this.Text = "frmReleaseDetainLicense";
            this.Load += new System.EventHandler(this.frmReleaseDetainLicense_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ucLicenseSearchFilter ucLicenseSearchFilter1;
        private ucLicenseInfo ucLicenseInfo1;
        private ucShowDetainInfo ucShowDetainInfo1;
        private System.Windows.Forms.LinkLabel llShowLicenseInfo;
        private System.Windows.Forms.LinkLabel llShowLicenseHistory;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel5;
        private Guna.UI2.WinForms.Guna2Button btnRelease;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private FontAwesome.Sharp.IconPictureBox picShield;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private Guna.UI2.WinForms.Guna2PictureBox pictureBox1;
    }
}