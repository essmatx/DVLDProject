namespace DVLDManagePeople_PresentationLayer
{
    partial class frmAddUpdatePerson
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
            this.lblAddNewEddite = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.iconPictureBox3 = new FontAwesome.Sharp.IconPictureBox();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2GradientPanel1 = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.lblPersonID = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.ucAddPerson1 = new DVLDManagePeople_PresentationLayer.ucAddPerson();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).BeginInit();
            this.guna2GradientPanel1.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblAddNewEddite
            // 
            this.lblAddNewEddite.BackColor = System.Drawing.Color.Transparent;
            this.lblAddNewEddite.Font = new System.Drawing.Font("Georgia", 25F);
            this.lblAddNewEddite.ForeColor = System.Drawing.Color.White;
            this.lblAddNewEddite.Location = new System.Drawing.Point(45, 100);
            this.lblAddNewEddite.Name = "lblAddNewEddite";
            this.lblAddNewEddite.Size = new System.Drawing.Size(370, 50);
            this.lblAddNewEddite.TabIndex = 5;
            this.lblAddNewEddite.Text = "Person  Information";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Georgia", 9F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.label11.Location = new System.Drawing.Point(92, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(181, 18);
            this.label11.TabIndex = 6;
            this.label11.Text = "CITIZEN DATA NODE";
            // 
            // iconPictureBox3
            // 
            this.iconPictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.iconPictureBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.iconPictureBox3.IconChar = FontAwesome.Sharp.IconChar.Shield;
            this.iconPictureBox3.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.iconPictureBox3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPictureBox3.IconSize = 30;
            this.iconPictureBox3.Location = new System.Drawing.Point(36, 40);
            this.iconPictureBox3.Name = "iconPictureBox3";
            this.iconPictureBox3.Size = new System.Drawing.Size(30, 31);
            this.iconPictureBox3.TabIndex = 26;
            this.iconPictureBox3.TabStop = false;
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(140)))), ((int)(((byte)(90)))));
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(45, 156);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(536, 25);
            this.guna2HtmlLabel1.TabIndex = 27;
            this.guna2HtmlLabel1.Text = "Editing official registry record for high-security identification clearance";
            // 
            // guna2GradientPanel1
            // 
            this.guna2GradientPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.guna2GradientPanel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.guna2GradientPanel1.Controls.Add(this.guna2Panel1);
            this.guna2GradientPanel1.Controls.Add(this.ucAddPerson1);
            this.guna2GradientPanel1.Controls.Add(this.lblAddNewEddite);
            this.guna2GradientPanel1.Controls.Add(this.iconPictureBox3);
            this.guna2GradientPanel1.Controls.Add(this.label11);
            this.guna2GradientPanel1.Controls.Add(this.guna2HtmlLabel1);
            this.guna2GradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2GradientPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(28)))), ((int)(((byte)(29)))));
            this.guna2GradientPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(11)))), ((int)(((byte)(12)))), ((int)(((byte)(14)))));
            this.guna2GradientPanel1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.guna2GradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2GradientPanel1.Name = "guna2GradientPanel1";
            this.guna2GradientPanel1.ShadowDecoration.Color = System.Drawing.Color.DarkGoldenrod;
            this.guna2GradientPanel1.ShadowDecoration.Enabled = true;
            this.guna2GradientPanel1.Size = new System.Drawing.Size(1312, 950);
            this.guna2GradientPanel1.TabIndex = 39;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(79)))), ((int)(((byte)(60)))));
            this.guna2Panel1.BorderRadius = 4;
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.lblPersonID);
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel2);
            this.guna2Panel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(35)))), ((int)(((byte)(37)))));
            this.guna2Panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(168)))), ((int)(((byte)(101)))));
            this.guna2Panel1.Location = new System.Drawing.Point(436, 239);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(132, 31);
            this.guna2Panel1.TabIndex = 29;
            // 
            // lblPersonID
            // 
            this.lblPersonID.BackColor = System.Drawing.Color.Transparent;
            this.lblPersonID.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPersonID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(168)))), ((int)(((byte)(101)))));
            this.lblPersonID.Location = new System.Drawing.Point(49, 8);
            this.lblPersonID.Name = "lblPersonID";
            this.lblPersonID.Size = new System.Drawing.Size(65, 18);
            this.lblPersonID.TabIndex = 15;
            this.lblPersonID.Text = "AE-7729-X";
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Georgia", 7.8F);
            this.guna2HtmlLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(168)))), ((int)(((byte)(101)))));
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(18, 8);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(25, 18);
            this.guna2HtmlLabel2.TabIndex = 14;
            this.guna2HtmlLabel2.Text = "ID :";
            // 
            // ucAddPerson1
            // 
            this.ucAddPerson1.BackColor = System.Drawing.Color.Transparent;
            this.ucAddPerson1.DateOfBirthValue = new System.DateTime(2008, 8, 18, 0, 0, 0, 0);
            this.ucAddPerson1.Location = new System.Drawing.Point(-4, 199);
            this.ucAddPerson1.Name = "ucAddPerson1";
            this.ucAddPerson1.Size = new System.Drawing.Size(1319, 710);
            this.ucAddPerson1.TabIndex = 28;
            // 
            // frmAddUpdatePerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1312, 950);
            this.Controls.Add(this.guna2GradientPanel1);
            this.Name = "frmAddUpdatePerson";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.frmAddUpdatePerson_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox3)).EndInit();
            this.guna2GradientPanel1.ResumeLayout(false);
            this.guna2GradientPanel1.PerformLayout();
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2HtmlLabel lblAddNewEddite;
        private System.Windows.Forms.Label label11;
        private FontAwesome.Sharp.IconPictureBox iconPictureBox3;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2GradientPanel guna2GradientPanel1;
        private ucAddPerson ucAddPerson1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblPersonID;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
    }
}