namespace DVLDManagePeople_PresentationLayer
{
    partial class frmLocalDrivingLicenseAppInfo
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
            this.ucPersonInfo1 = new DVLDManagePeople_PresentationLayer.UCPersonInfo();
            this.ucApplicationBasicInfo1 = new DVLDManagePeople_PresentationLayer.ucApplicationBasicInfo();
            this.button1 = new Guna.UI2.WinForms.Guna2Button();
            this.lblFormTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.SuspendLayout();
            // 
            // ucPersonInfo1
            // 
            this.ucPersonInfo1.Location = new System.Drawing.Point(12, 58);
            this.ucPersonInfo1.Name = "ucPersonInfo1";
            this.ucPersonInfo1.Size = new System.Drawing.Size(975, 504);
            this.ucPersonInfo1.TabIndex = 9;
            // 
            // ucApplicationBasicInfo1
            // 
            this.ucApplicationBasicInfo1.Location = new System.Drawing.Point(12, 568);
            this.ucApplicationBasicInfo1.Name = "ucApplicationBasicInfo1";
            this.ucApplicationBasicInfo1.Size = new System.Drawing.Size(975, 444);
            this.ucApplicationBasicInfo1.TabIndex = 12;
            this.ucApplicationBasicInfo1.Load += new System.EventHandler(this.ucApplicationBasicInfo1_Load);
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
            this.button1.Location = new System.Drawing.Point(818, 1007);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 36);
            this.button1.TabIndex = 31;
            this.button1.Text = "Close";
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblFormTitle.Font = new System.Drawing.Font("Times New Roman", 20F);
            this.lblFormTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.lblFormTitle.Location = new System.Drawing.Point(371, 12);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(216, 39);
            this.lblFormTitle.TabIndex = 45;
            this.lblFormTitle.Text = "Application Info";
            // 
            // frmLocalDrivingLicenseAppInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(18)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(991, 1055);
            this.Controls.Add(this.lblFormTitle);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ucApplicationBasicInfo1);
            this.Controls.Add(this.ucPersonInfo1);
            this.Name = "frmLocalDrivingLicenseAppInfo";
            this.Text = "frmLocalDrivingLicenseAppInfo";
            this.Load += new System.EventHandler(this.frmLocalDrivingLicenseAppInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private UCPersonInfo ucPersonInfo1;
        private ucApplicationBasicInfo ucApplicationBasicInfo1;
        private Guna.UI2.WinForms.Guna2Button button1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblFormTitle;
    }
}