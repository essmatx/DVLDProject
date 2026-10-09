namespace DVLDManagePeople_PresentationLayer
{
    partial class frmReNewLocalDrivingLicense
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
            this.llShowNewLicenseInfo = new System.Windows.Forms.LinkLabel();
            this.llShowLicenseHistory = new System.Windows.Forms.LinkLabel();
            this.ucLicenseSearchFilter1 = new DVLDManagePeople_PresentationLayer.ucLicenseSearchFilter();
            this.ucRenewLocalDrivingLicense1 = new DVLDManagePeople_PresentationLayer.ucRenewLocalDrivingLicense();
            this.ucLicenseInfo1 = new DVLDManagePeople_PresentationLayer.ucLicenseInfo();
            this.btnRenew = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.guna2HtmlLabel5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.picShield = new FontAwesome.Sharp.IconPictureBox();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // llShowNewLicenseInfo
            // 
            this.llShowNewLicenseInfo.AutoSize = true;
            this.llShowNewLicenseInfo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.llShowNewLicenseInfo.Location = new System.Drawing.Point(574, 1020);
            this.llShowNewLicenseInfo.Name = "llShowNewLicenseInfo";
            this.llShowNewLicenseInfo.Size = new System.Drawing.Size(147, 16);
            this.llShowNewLicenseInfo.TabIndex = 6;
            this.llShowNewLicenseInfo.TabStop = true;
            this.llShowNewLicenseInfo.Text = "Show New Licnese Info ";
            this.llShowNewLicenseInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowNewLicenseInfo_LinkClicked);
            // 
            // llShowLicenseHistory
            // 
            this.llShowLicenseHistory.AutoSize = true;
            this.llShowLicenseHistory.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.llShowLicenseHistory.Location = new System.Drawing.Point(316, 1020);
            this.llShowLicenseHistory.Name = "llShowLicenseHistory";
            this.llShowLicenseHistory.Size = new System.Drawing.Size(135, 16);
            this.llShowLicenseHistory.TabIndex = 7;
            this.llShowLicenseHistory.TabStop = true;
            this.llShowLicenseHistory.Text = "Show Licnese History";
            this.llShowLicenseHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowLicenseHistory_LinkClicked);
            // 
            // ucLicenseSearchFilter1
            // 
            this.ucLicenseSearchFilter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ucLicenseSearchFilter1.Location = new System.Drawing.Point(12, 72);
            this.ucLicenseSearchFilter1.Name = "ucLicenseSearchFilter1";
            this.ucLicenseSearchFilter1.Size = new System.Drawing.Size(459, 57);
            this.ucLicenseSearchFilter1.TabIndex = 8;
            this.ucLicenseSearchFilter1.Load += new System.EventHandler(this.ucLicenseSearchFilter1_Load);
            // 
            // ucRenewLocalDrivingLicense1
            // 
            this.ucRenewLocalDrivingLicense1.Location = new System.Drawing.Point(12, 612);
            this.ucRenewLocalDrivingLicense1.Name = "ucRenewLocalDrivingLicense1";
            this.ucRenewLocalDrivingLicense1.Size = new System.Drawing.Size(1063, 380);
            this.ucRenewLocalDrivingLicense1.TabIndex = 0;
            // 
            // ucLicenseInfo1
            // 
            this.ucLicenseInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucLicenseInfo1.Location = new System.Drawing.Point(12, 135);
            this.ucLicenseInfo1.Name = "ucLicenseInfo1";
            this.ucLicenseInfo1.Size = new System.Drawing.Size(1063, 471);
            this.ucLicenseInfo1.TabIndex = 0;
            // 
            // btnRenew
            // 
            this.btnRenew.BackColor = System.Drawing.Color.Transparent;
            this.btnRenew.BorderRadius = 10;
            this.btnRenew.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnRenew.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnRenew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnRenew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnRenew.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.btnRenew.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnRenew.ForeColor = System.Drawing.Color.Black;
            this.btnRenew.Image = global::DVLDManagePeople_PresentationLayer.Properties.Resources.driving_license;
            this.btnRenew.Location = new System.Drawing.Point(915, 998);
            this.btnRenew.Name = "btnRenew";
            this.btnRenew.Size = new System.Drawing.Size(160, 38);
            this.btnRenew.TabIndex = 65;
            this.btnRenew.Text = "Renew License";
            this.btnRenew.Click += new System.EventHandler(this.btnRenew_Click);
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
            this.btnClose.Location = new System.Drawing.Point(12, 1000);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(169, 36);
            this.btnClose.TabIndex = 116;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // guna2HtmlLabel5
            // 
            this.guna2HtmlLabel5.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel5.Font = new System.Drawing.Font("Georgia", 20F);
            this.guna2HtmlLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.guna2HtmlLabel5.Location = new System.Drawing.Point(342, 12);
            this.guna2HtmlLabel5.Name = "guna2HtmlLabel5";
            this.guna2HtmlLabel5.Padding = new System.Windows.Forms.Padding(0, 0, 37, 0);
            this.guna2HtmlLabel5.Size = new System.Drawing.Size(420, 40);
            this.guna2HtmlLabel5.TabIndex = 117;
            this.guna2HtmlLabel5.Text = "Renw License Application";
            this.guna2HtmlLabel5.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            // 
            // picShield
            // 
            this.picShield.BackColor = System.Drawing.Color.Transparent;
            this.picShield.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconChar = FontAwesome.Sharp.IconChar.IdCard;
            this.picShield.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.picShield.IconSize = 78;
            this.picShield.Location = new System.Drawing.Point(934, 21);
            this.picShield.Name = "picShield";
            this.picShield.Size = new System.Drawing.Size(141, 78);
            this.picShield.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picShield.TabIndex = 118;
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
            this.iconPictureBox1.Location = new System.Drawing.Point(778, 387);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(90, 90);
            this.iconPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPictureBox1.TabIndex = 119;
            this.iconPictureBox1.TabStop = false;
            // 
            // frmReNewLocalDrivingLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(1082, 1055);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.picShield);
            this.Controls.Add(this.guna2HtmlLabel5);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRenew);
            this.Controls.Add(this.ucLicenseInfo1);
            this.Controls.Add(this.ucRenewLocalDrivingLicense1);
            this.Controls.Add(this.ucLicenseSearchFilter1);
            this.Controls.Add(this.llShowLicenseHistory);
            this.Controls.Add(this.llShowNewLicenseInfo);
            this.Name = "frmReNewLocalDrivingLicense";
            this.Text = "frmReNewLocalDrivingLicense";
            this.Load += new System.EventHandler(this.frmReNewLocalDrivingLicense_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ucRenewLocalDrivingLicense ucRenewLocalDrivingLicense1;
        private System.Windows.Forms.LinkLabel llShowNewLicenseInfo;
        private System.Windows.Forms.LinkLabel llShowLicenseHistory;
        private ucLicenseSearchFilter ucLicenseSearchFilter1;
        private ucLicenseInfo ucLicenseInfo1;
        private Guna.UI2.WinForms.Guna2Button btnRenew;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel5;
        private FontAwesome.Sharp.IconPictureBox picShield;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
    }
}