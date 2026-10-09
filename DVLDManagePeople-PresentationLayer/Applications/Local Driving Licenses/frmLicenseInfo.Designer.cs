namespace DVLDManagePeople_PresentationLayer
{
    partial class frmLicenseInfo
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
            this.ucLicenseInfo1 = new DVLDManagePeople_PresentationLayer.ucLicenseInfo();
            this.frmTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnCLose = new Guna.UI2.WinForms.Guna2Button();
            this.picShield = new FontAwesome.Sharp.IconPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).BeginInit();
            this.SuspendLayout();
            // 
            // ucLicenseInfo1
            // 
            this.ucLicenseInfo1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(15)))), ((int)(((byte)(16)))));
            this.ucLicenseInfo1.Location = new System.Drawing.Point(12, 131);
            this.ucLicenseInfo1.Name = "ucLicenseInfo1";
            this.ucLicenseInfo1.Size = new System.Drawing.Size(916, 732);
            this.ucLicenseInfo1.TabIndex = 0;
            // 
            // frmTitle
            // 
            this.frmTitle.BackColor = System.Drawing.Color.Transparent;
            this.frmTitle.Font = new System.Drawing.Font("Georgia", 20F);
            this.frmTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.frmTitle.Location = new System.Drawing.Point(282, 12);
            this.frmTitle.Name = "frmTitle";
            this.frmTitle.Padding = new System.Windows.Forms.Padding(0, 0, 37, 0);
            this.frmTitle.Size = new System.Drawing.Size(324, 40);
            this.frmTitle.TabIndex = 3;
            this.frmTitle.Text = "Driver License Info";
            this.frmTitle.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnCLose
            // 
            this.btnCLose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.btnCLose.BorderRadius = 6;
            this.btnCLose.BorderThickness = 1;
            this.btnCLose.CustomBorderColor = System.Drawing.Color.Silver;
            this.btnCLose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCLose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCLose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCLose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCLose.FillColor = System.Drawing.Color.Black;
            this.btnCLose.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCLose.ForeColor = System.Drawing.Color.White;
            this.btnCLose.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnCLose.Location = new System.Drawing.Point(764, 876);
            this.btnCLose.Name = "btnCLose";
            this.btnCLose.Size = new System.Drawing.Size(169, 36);
            this.btnCLose.TabIndex = 46;
            this.btnCLose.Text = "Close";
            this.btnCLose.Click += new System.EventHandler(this.btnCLose_Click);
            // 
            // picShield
            // 
            this.picShield.BackColor = System.Drawing.Color.Transparent;
            this.picShield.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconChar = FontAwesome.Sharp.IconChar.IdCard;
            this.picShield.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.picShield.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.picShield.IconSize = 72;
            this.picShield.Location = new System.Drawing.Point(344, 53);
            this.picShield.Name = "picShield";
            this.picShield.Size = new System.Drawing.Size(124, 72);
            this.picShield.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picShield.TabIndex = 47;
            this.picShield.TabStop = false;
            // 
            // frmLicenseInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(18)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(945, 924);
            this.Controls.Add(this.picShield);
            this.Controls.Add(this.btnCLose);
            this.Controls.Add(this.frmTitle);
            this.Controls.Add(this.ucLicenseInfo1);
            this.Name = "frmLicenseInfo";
            this.Text = "frmLicenseInfo";
            this.Load += new System.EventHandler(this.frmLicenseInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picShield)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ucLicenseInfo ucLicenseInfo1;
        private Guna.UI2.WinForms.Guna2HtmlLabel frmTitle;
        private Guna.UI2.WinForms.Guna2Button btnCLose;
        private FontAwesome.Sharp.IconPictureBox picShield;
    }
}