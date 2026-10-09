namespace DVLDManagePeople_PresentationLayer
{
    partial class frmInternationalDrivingLicenseInfo
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
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.button1 = new Guna.UI2.WinForms.Guna2Button();
            this.picShield = new FontAwesome.Sharp.IconPictureBox();
            this.ucInternationalLicenseInfo1 = new DVLDManagePeople_PresentationLayer.ucInternationalLicenseInfo();
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Times New Roman", 36F);
            this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(63, 33);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(796, 70);
            this.guna2HtmlLabel1.TabIndex = 11;
            this.guna2HtmlLabel1.Text = "Driver International License Info ";
            // 
            // button1
            // 
            this.button1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.button1.BorderRadius = 6;
            this.button1.BorderThickness = 1;
            this.button1.CustomBorderColor = System.Drawing.Color.Silver;
            this.button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.button1.FillColor = System.Drawing.Color.Black;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.HoverState.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(785, 789);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 36);
            this.button1.TabIndex = 31;
            this.button1.Text = "Close";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // picShield
            // 
            this.picShield.BackColor = System.Drawing.Color.Transparent;
            this.picShield.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconChar = FontAwesome.Sharp.IconChar.IdCard;
            this.picShield.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.picShield.IconSize = 90;
            this.picShield.Location = new System.Drawing.Point(406, 125);
            this.picShield.Name = "picShield";
            this.picShield.Size = new System.Drawing.Size(90, 90);
            this.picShield.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picShield.TabIndex = 32;
            this.picShield.TabStop = false;
            // 
            // ucInternationalLicenseInfo1
            // 
            this.ucInternationalLicenseInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucInternationalLicenseInfo1.Location = new System.Drawing.Point(12, 221);
            this.ucInternationalLicenseInfo1.Name = "ucInternationalLicenseInfo1";
            this.ucInternationalLicenseInfo1.Size = new System.Drawing.Size(952, 556);
            this.ucInternationalLicenseInfo1.TabIndex = 0;
            // 
            // frmInternationalDrivingLicenseInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ClientSize = new System.Drawing.Size(977, 837);
            this.Controls.Add(this.picShield);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.guna2HtmlLabel1);
            this.Controls.Add(this.ucInternationalLicenseInfo1);
            this.Name = "frmInternationalDrivingLicenseInfo";
            this.Text = "frmInternationalDrivingLicenseInfo";
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ucInternationalLicenseInfo ucInternationalLicenseInfo1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2Button button1;
        private FontAwesome.Sharp.IconPictureBox picShield;
    }
}