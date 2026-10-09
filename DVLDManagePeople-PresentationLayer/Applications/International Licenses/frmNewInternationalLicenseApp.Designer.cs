namespace DVLDManagePeople_PresentationLayer
{
    partial class frmNewInternationalLicenseApp
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
            this.lblFormTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.picShield = new FontAwesome.Sharp.IconPictureBox();
            this.btnIssue = new Guna.UI2.WinForms.Guna2Button();
            this.btnEditPerson = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.ucInternationalDrivingLicenseAppInfo1 = new DVLDManagePeople_PresentationLayer.ucInternationalDrivingLicenseAppInfo();
            this.ucLicenseInfo1 = new DVLDManagePeople_PresentationLayer.ucLicenseInfo();
            this.ucLicenseSearchFilter1 = new DVLDManagePeople_PresentationLayer.ucLicenseSearchFilter();
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFormTitle.Font = new System.Drawing.Font("Times New Roman", 20F);
            this.lblFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.lblFormTitle.Location = new System.Drawing.Point(251, 30);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(436, 39);
            this.lblFormTitle.TabIndex = 45;
            this.lblFormTitle.Text = "International License Application";
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
            this.btnClose.Location = new System.Drawing.Point(12, 1010);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(169, 36);
            this.btnClose.TabIndex = 46;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // picShield
            // 
            this.picShield.BackColor = System.Drawing.Color.Transparent;
            this.picShield.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconChar = FontAwesome.Sharp.IconChar.IdCard;
            this.picShield.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.picShield.IconSize = 103;
            this.picShield.Location = new System.Drawing.Point(769, 21);
            this.picShield.Name = "picShield";
            this.picShield.Size = new System.Drawing.Size(161, 103);
            this.picShield.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picShield.TabIndex = 47;
            this.picShield.TabStop = false;
            // 
            // btnIssue
            // 
            this.btnIssue.BackColor = System.Drawing.Color.Transparent;
            this.btnIssue.BorderRadius = 10;
            this.btnIssue.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnIssue.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnIssue.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnIssue.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnIssue.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.btnIssue.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnIssue.ForeColor = System.Drawing.Color.Black;
            this.btnIssue.Image = global::DVLDManagePeople_PresentationLayer.Properties.Resources.driving_license;
            this.btnIssue.Location = new System.Drawing.Point(769, 1010);
            this.btnIssue.Name = "btnIssue";
            this.btnIssue.Size = new System.Drawing.Size(160, 38);
            this.btnIssue.TabIndex = 63;
            this.btnIssue.Text = "Issue License";
            // 
            // btnEditPerson
            // 
            this.btnEditPerson.BackColor = System.Drawing.Color.Transparent;
            this.btnEditPerson.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.btnEditPerson.BorderRadius = 6;
            this.btnEditPerson.BorderThickness = 1;
            this.btnEditPerson.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEditPerson.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEditPerson.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEditPerson.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEditPerson.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.btnEditPerson.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnEditPerson.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.btnEditPerson.Location = new System.Drawing.Point(717, 386);
            this.btnEditPerson.Name = "btnEditPerson";
            this.btnEditPerson.Size = new System.Drawing.Size(201, 23);
            this.btnEditPerson.TabIndex = 64;
            this.btnEditPerson.Text = "SHOW LICENSE INFO";
            this.btnEditPerson.Click += new System.EventHandler(this.btnEditPerson_Click);
            // 
            // guna2Button1
            // 
            this.guna2Button1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Button1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.guna2Button1.BorderRadius = 6;
            this.guna2Button1.BorderThickness = 1;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(50)))), ((int)(((byte)(55)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.guna2Button1.Location = new System.Drawing.Point(355, 1019);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(207, 24);
            this.guna2Button1.TabIndex = 65;
            this.guna2Button1.Text = "SHOW LICENSE HISTORY ";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // ucInternationalDrivingLicenseAppInfo1
            // 
            this.ucInternationalDrivingLicenseAppInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucInternationalDrivingLicenseAppInfo1.Location = new System.Drawing.Point(12, 754);
            this.ucInternationalDrivingLicenseAppInfo1.Name = "ucInternationalDrivingLicenseAppInfo1";
            this.ucInternationalDrivingLicenseAppInfo1.Size = new System.Drawing.Size(918, 250);
            this.ucInternationalDrivingLicenseAppInfo1.TabIndex = 0;
            this.ucInternationalDrivingLicenseAppInfo1.Load += new System.EventHandler(this.ucInternationalDrivingLicenseAppInfo1_Load);
            // 
            // ucLicenseInfo1
            // 
            this.ucLicenseInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucLicenseInfo1.Location = new System.Drawing.Point(12, 139);
            this.ucLicenseInfo1.Name = "ucLicenseInfo1";
            this.ucLicenseInfo1.Size = new System.Drawing.Size(918, 609);
            this.ucLicenseInfo1.TabIndex = 0;
            // 
            // ucLicenseSearchFilter1
            // 
            this.ucLicenseSearchFilter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucLicenseSearchFilter1.Location = new System.Drawing.Point(12, 75);
            this.ucLicenseSearchFilter1.Name = "ucLicenseSearchFilter1";
            this.ucLicenseSearchFilter1.Size = new System.Drawing.Size(448, 58);
            this.ucLicenseSearchFilter1.TabIndex = 8;
            this.ucLicenseSearchFilter1.Load += new System.EventHandler(this.ucLicenseSearchFilter1_Load);
            // 
            // frmNewInternationalLicenseApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(945, 1055);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.btnEditPerson);
            this.Controls.Add(this.btnIssue);
            this.Controls.Add(this.picShield);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblFormTitle);
            this.Controls.Add(this.ucInternationalDrivingLicenseAppInfo1);
            this.Controls.Add(this.ucLicenseInfo1);
            this.Controls.Add(this.ucLicenseSearchFilter1);
            this.Name = "frmNewInternationalLicenseApp";
            this.Text = "frmNewInternationalLicenseApp";
            this.Load += new System.EventHandler(this.frmNewInternationalLicenseApp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ucLicenseSearchFilter ucLicenseSearchFilter1;
        private ucLicenseInfo ucLicenseInfo1;
        private ucInternationalDrivingLicenseAppInfo ucInternationalDrivingLicenseAppInfo1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblFormTitle;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private FontAwesome.Sharp.IconPictureBox picShield;
        private Guna.UI2.WinForms.Guna2Button btnIssue;
        private Guna.UI2.WinForms.Guna2Button btnEditPerson;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}