namespace DVLDManagePeople_PresentationLayer
{
    partial class frmUserInfo
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
            this.ucLoginInfo1 = new DVLDManagePeople_PresentationLayer.UCLoginInfo();
            this.guna2GradientPanel1 = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.guna2GradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucPersonInfo1
            // 
            this.ucPersonInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPersonInfo1.Location = new System.Drawing.Point(0, 0);
            this.ucPersonInfo1.Name = "ucPersonInfo1";
            this.ucPersonInfo1.Size = new System.Drawing.Size(975, 580);
            this.ucPersonInfo1.TabIndex = 0;
            // 
            // ucLoginInfo1
            // 
            this.ucLoginInfo1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucLoginInfo1.Location = new System.Drawing.Point(0, 580);
            this.ucLoginInfo1.Name = "ucLoginInfo1";
            this.ucLoginInfo1.Size = new System.Drawing.Size(975, 89);
            this.ucLoginInfo1.TabIndex = 7;
            // 
            // guna2GradientPanel1
            // 
            this.guna2GradientPanel1.Controls.Add(this.ucPersonInfo1);
            this.guna2GradientPanel1.Controls.Add(this.ucLoginInfo1);
            this.guna2GradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2GradientPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.guna2GradientPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(21)))), ((int)(((byte)(23)))));
            this.guna2GradientPanel1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.guna2GradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2GradientPanel1.Name = "guna2GradientPanel1";
            this.guna2GradientPanel1.Size = new System.Drawing.Size(975, 669);
            this.guna2GradientPanel1.TabIndex = 24;
            // 
            // frmUserInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 669);
            this.Controls.Add(this.guna2GradientPanel1);
            this.Name = "frmUserInfo";
            this.Text = "frmUserInfo";
            this.Load += new System.EventHandler(this.frmUserInfo_Load);
            this.guna2GradientPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UCPersonInfo ucPersonInfo1;
        private UCLoginInfo ucLoginInfo1;
        private Guna.UI2.WinForms.Guna2GradientPanel guna2GradientPanel1;
    }
}