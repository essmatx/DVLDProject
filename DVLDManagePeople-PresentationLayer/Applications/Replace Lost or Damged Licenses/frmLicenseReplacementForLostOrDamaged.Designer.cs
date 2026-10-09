namespace DVLDManagePeople_PresentationLayer
{
    partial class frmLicenseReplacementForLostOrDamaged
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
            this.rbLostLicense = new System.Windows.Forms.RadioButton();
            this.rbDamagedLicense = new System.Windows.Forms.RadioButton();
            this.ucLicenseInfo1 = new DVLDManagePeople_PresentationLayer.ucLicenseInfo();
            this.ucApplicationInforLicenseReplacement1 = new DVLDManagePeople_PresentationLayer.ucApplicationInforLicenseReplacement();
            this.llShowLicenseInfo = new System.Windows.Forms.LinkLabel();
            this.llShowLicenseHistory = new System.Windows.Forms.LinkLabel();
            this.ucLicenseSearchFilter1 = new DVLDManagePeople_PresentationLayer.ucLicenseSearchFilter();
            this.ucLicenseInfo2 = new DVLDManagePeople_PresentationLayer.ucLicenseInfo();
            this.ucApplicationInforLicenseReplacement2 = new DVLDManagePeople_PresentationLayer.ucApplicationInforLicenseReplacement();
            this.lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnIssueReplacement = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
            this.iconPictureBox2 = new FontAwesome.Sharp.IconPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // rbLostLicense
            // 
            this.rbLostLicense.AutoSize = true;
            this.rbLostLicense.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLostLicense.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.rbLostLicense.Location = new System.Drawing.Point(6, 74);
            this.rbLostLicense.Name = "rbLostLicense";
            this.rbLostLicense.Size = new System.Drawing.Size(122, 27);
            this.rbLostLicense.TabIndex = 1;
            this.rbLostLicense.TabStop = true;
            this.rbLostLicense.Text = "Lost License";
            this.rbLostLicense.UseVisualStyleBackColor = true;
            this.rbLostLicense.CheckedChanged += new System.EventHandler(this.rbLostLicense_CheckedChanged);
            // 
            // rbDamagedLicense
            // 
            this.rbDamagedLicense.AutoSize = true;
            this.rbDamagedLicense.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDamagedLicense.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.rbDamagedLicense.Location = new System.Drawing.Point(791, 74);
            this.rbDamagedLicense.Name = "rbDamagedLicense";
            this.rbDamagedLicense.Size = new System.Drawing.Size(165, 27);
            this.rbDamagedLicense.TabIndex = 0;
            this.rbDamagedLicense.TabStop = true;
            this.rbDamagedLicense.Text = "Damaged License";
            this.rbDamagedLicense.UseVisualStyleBackColor = true;
            this.rbDamagedLicense.CheckedChanged += new System.EventHandler(this.rbDamagedLicense_CheckedChanged);
            // 
            // ucLicenseInfo1
            // 
            this.ucLicenseInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucLicenseInfo1.Location = new System.Drawing.Point(10, 33);
            this.ucLicenseInfo1.Name = "ucLicenseInfo1";
            this.ucLicenseInfo1.Size = new System.Drawing.Size(919, 381);
            this.ucLicenseInfo1.TabIndex = 0;
            // 
            // ucApplicationInforLicenseReplacement1
            // 
            this.ucApplicationInforLicenseReplacement1.Location = new System.Drawing.Point(10, 21);
            this.ucApplicationInforLicenseReplacement1.Name = "ucApplicationInforLicenseReplacement1";
            this.ucApplicationInforLicenseReplacement1.Size = new System.Drawing.Size(931, 146);
            this.ucApplicationInforLicenseReplacement1.TabIndex = 0;
            // 
            // llShowLicenseInfo
            // 
            this.llShowLicenseInfo.AutoSize = true;
            this.llShowLicenseInfo.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.llShowLicenseInfo.Location = new System.Drawing.Point(518, 1030);
            this.llShowLicenseInfo.Name = "llShowLicenseInfo";
            this.llShowLicenseInfo.Size = new System.Drawing.Size(114, 16);
            this.llShowLicenseInfo.TabIndex = 7;
            this.llShowLicenseInfo.TabStop = true;
            this.llShowLicenseInfo.Text = "Show License Info";
            this.llShowLicenseInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowLicenseInfo_LinkClicked);
            // 
            // llShowLicenseHistory
            // 
            this.llShowLicenseHistory.AutoSize = true;
            this.llShowLicenseHistory.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.llShowLicenseHistory.Location = new System.Drawing.Point(254, 1030);
            this.llShowLicenseHistory.Name = "llShowLicenseHistory";
            this.llShowLicenseHistory.Size = new System.Drawing.Size(135, 16);
            this.llShowLicenseHistory.TabIndex = 8;
            this.llShowLicenseHistory.TabStop = true;
            this.llShowLicenseHistory.Text = "Show License History";
            this.llShowLicenseHistory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llShowLicenseHistory_LinkClicked);
            // 
            // ucLicenseSearchFilter1
            // 
            this.ucLicenseSearchFilter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucLicenseSearchFilter1.Location = new System.Drawing.Point(36, 177);
            this.ucLicenseSearchFilter1.Name = "ucLicenseSearchFilter1";
            this.ucLicenseSearchFilter1.Size = new System.Drawing.Size(435, 80);
            this.ucLicenseSearchFilter1.TabIndex = 9;
            // 
            // ucLicenseInfo2
            // 
            this.ucLicenseInfo2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucLicenseInfo2.Location = new System.Drawing.Point(6, 107);
            this.ucLicenseInfo2.Name = "ucLicenseInfo2";
            this.ucLicenseInfo2.Size = new System.Drawing.Size(956, 618);
            this.ucLicenseInfo2.TabIndex = 0;
            // 
            // ucApplicationInforLicenseReplacement2
            // 
            this.ucApplicationInforLicenseReplacement2.Location = new System.Drawing.Point(12, 731);
            this.ucApplicationInforLicenseReplacement2.Name = "ucApplicationInforLicenseReplacement2";
            this.ucApplicationInforLicenseReplacement2.Size = new System.Drawing.Size(950, 279);
            this.ucApplicationInforLicenseReplacement2.TabIndex = 9;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Georgia", 20F);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.lblTitle.Location = new System.Drawing.Point(389, 22);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 0, 37, 0);
            this.lblTitle.Size = new System.Drawing.Size(106, 40);
            this.lblTitle.TabIndex = 118;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnIssueReplacement
            // 
            this.btnIssueReplacement.BackColor = System.Drawing.Color.Transparent;
            this.btnIssueReplacement.BorderRadius = 10;
            this.btnIssueReplacement.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnIssueReplacement.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnIssueReplacement.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnIssueReplacement.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnIssueReplacement.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.btnIssueReplacement.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnIssueReplacement.ForeColor = System.Drawing.Color.Black;
            this.btnIssueReplacement.Image = global::DVLDManagePeople_PresentationLayer.Properties.Resources.driving_license;
            this.btnIssueReplacement.Location = new System.Drawing.Point(802, 1015);
            this.btnIssueReplacement.Name = "btnIssueReplacement";
            this.btnIssueReplacement.Size = new System.Drawing.Size(160, 31);
            this.btnIssueReplacement.TabIndex = 119;
            this.btnIssueReplacement.Text = "Issue License";
            this.btnIssueReplacement.Click += new System.EventHandler(this.btnIssueReplacement_Click);
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
            this.btnClose.Location = new System.Drawing.Point(12, 1016);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(169, 30);
            this.btnClose.TabIndex = 120;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // iconPictureBox1
            // 
            this.iconPictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.IdCardAlt;
            this.iconPictureBox1.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox1.IconSize = 90;
            this.iconPictureBox1.Location = new System.Drawing.Point(772, 394);
            this.iconPictureBox1.Name = "iconPictureBox1";
            this.iconPictureBox1.Size = new System.Drawing.Size(90, 90);
            this.iconPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPictureBox1.TabIndex = 121;
            this.iconPictureBox1.TabStop = false;
            // 
            // iconPictureBox2
            // 
            this.iconPictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.iconPictureBox2.IconChar = FontAwesome.Sharp.IconChar.IdCard;
            this.iconPictureBox2.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.iconPictureBox2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox2.IconSize = 74;
            this.iconPictureBox2.Location = new System.Drawing.Point(777, 764);
            this.iconPictureBox2.Name = "iconPictureBox2";
            this.iconPictureBox2.Size = new System.Drawing.Size(85, 74);
            this.iconPictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.iconPictureBox2.TabIndex = 123;
            this.iconPictureBox2.TabStop = false;
            // 
            // frmLicenseReplacementForLostOrDamaged
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.ClientSize = new System.Drawing.Size(968, 1055);
            this.Controls.Add(this.iconPictureBox2);
            this.Controls.Add(this.iconPictureBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnIssueReplacement);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.rbDamagedLicense);
            this.Controls.Add(this.rbLostLicense);
            this.Controls.Add(this.ucApplicationInforLicenseReplacement2);
            this.Controls.Add(this.ucLicenseInfo2);
            this.Controls.Add(this.llShowLicenseHistory);
            this.Controls.Add(this.llShowLicenseInfo);
            this.Name = "frmLicenseReplacementForLostOrDamaged";
            this.Text = "frmLicenseReplacementForLostOrDamaged";
            this.Load += new System.EventHandler(this.frmLicenseReplacementForLostOrDamaged_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton rbLostLicense;
        private System.Windows.Forms.RadioButton rbDamagedLicense;
        private ucLicenseInfo ucLicenseInfo1;
        private ucApplicationInforLicenseReplacement ucApplicationInforLicenseReplacement1;
        private System.Windows.Forms.LinkLabel llShowLicenseInfo;
        private System.Windows.Forms.LinkLabel llShowLicenseHistory;
        private ucLicenseSearchFilter ucLicenseSearchFilter1;
        private ucLicenseInfo ucLicenseInfo2;
        private ucApplicationInforLicenseReplacement ucApplicationInforLicenseReplacement2;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTitle;
        private Guna.UI2.WinForms.Guna2Button btnIssueReplacement;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox1;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox2;
    }
}