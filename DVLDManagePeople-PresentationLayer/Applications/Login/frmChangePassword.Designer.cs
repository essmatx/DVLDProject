namespace DVLDManagePeople_PresentationLayer
{
    partial class frmChangePassword
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
            this.components = new System.ComponentModel.Container();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.ucLoginInfo1 = new DVLDManagePeople_PresentationLayer.UCLoginInfo();
            this.ucPersonInfo1 = new DVLDManagePeople_PresentationLayer.UCPersonInfo();
            this.guna2GradientPanel1 = new Guna.UI2.WinForms.Guna2GradientPanel();
            this.btnClsoe = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.txtConfirmPass = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtNewPass = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtCurrentPass = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblConfirmPass = new System.Windows.Forms.Label();
            this.lblNewPass = new System.Windows.Forms.Label();
            this.lblCurrentPass = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.guna2GradientPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // ucLoginInfo1
            // 
            this.ucLoginInfo1.Location = new System.Drawing.Point(12, 510);
            this.ucLoginInfo1.Name = "ucLoginInfo1";
            this.ucLoginInfo1.Size = new System.Drawing.Size(902, 83);
            this.ucLoginInfo1.TabIndex = 1;
            this.ucLoginInfo1.Load += new System.EventHandler(this.ucLoginInfo1_Load);
            // 
            // ucPersonInfo1
            // 
            this.ucPersonInfo1.Location = new System.Drawing.Point(3, 12);
            this.ucPersonInfo1.Name = "ucPersonInfo1";
            this.ucPersonInfo1.Size = new System.Drawing.Size(911, 502);
            this.ucPersonInfo1.TabIndex = 0;
            // 
            // guna2GradientPanel1
            // 
            this.guna2GradientPanel1.Controls.Add(this.btnClsoe);
            this.guna2GradientPanel1.Controls.Add(this.btnSave);
            this.guna2GradientPanel1.Controls.Add(this.txtConfirmPass);
            this.guna2GradientPanel1.Controls.Add(this.txtNewPass);
            this.guna2GradientPanel1.Controls.Add(this.txtCurrentPass);
            this.guna2GradientPanel1.Controls.Add(this.lblConfirmPass);
            this.guna2GradientPanel1.Controls.Add(this.lblNewPass);
            this.guna2GradientPanel1.Controls.Add(this.lblCurrentPass);
            this.guna2GradientPanel1.Controls.Add(this.ucPersonInfo1);
            this.guna2GradientPanel1.Controls.Add(this.ucLoginInfo1);
            this.guna2GradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2GradientPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(34)))), ((int)(((byte)(37)))));
            this.guna2GradientPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(21)))), ((int)(((byte)(23)))));
            this.guna2GradientPanel1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            this.guna2GradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.guna2GradientPanel1.Name = "guna2GradientPanel1";
            this.guna2GradientPanel1.Size = new System.Drawing.Size(920, 850);
            this.guna2GradientPanel1.TabIndex = 24;
            // 
            // btnClsoe
            // 
            this.btnClsoe.BackColor = System.Drawing.Color.Transparent;
            this.btnClsoe.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.btnClsoe.BorderRadius = 6;
            this.btnClsoe.BorderThickness = 1;
            this.btnClsoe.CustomBorderColor = System.Drawing.Color.Silver;
            this.btnClsoe.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClsoe.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClsoe.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClsoe.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClsoe.FillColor = System.Drawing.Color.Black;
            this.btnClsoe.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClsoe.ForeColor = System.Drawing.Color.White;
            this.btnClsoe.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnClsoe.Location = new System.Drawing.Point(527, 798);
            this.btnClsoe.Name = "btnClsoe";
            this.btnClsoe.Size = new System.Drawing.Size(169, 36);
            this.btnClsoe.TabIndex = 42;
            this.btnClsoe.Text = "Close";
            this.btnClsoe.Click += new System.EventHandler(this.btnClsoe_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(197)))), ((int)(((byte)(160)))), ((int)(((byte)(89)))));
            this.btnSave.BorderRadius = 6;
            this.btnSave.BorderThickness = 1;
            this.btnSave.CustomBorderColor = System.Drawing.Color.Silver;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSave.FillColor = System.Drawing.Color.Black;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(744, 798);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(169, 36);
            this.btnSave.TabIndex = 41;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtConfirmPass
            // 
            this.txtConfirmPass.BackColor = System.Drawing.Color.Transparent;
            this.txtConfirmPass.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(79)))), ((int)(((byte)(60)))));
            this.txtConfirmPass.BorderRadius = 10;
            this.txtConfirmPass.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtConfirmPass.DefaultText = "";
            this.txtConfirmPass.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtConfirmPass.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtConfirmPass.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtConfirmPass.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtConfirmPass.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.txtConfirmPass.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtConfirmPass.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtConfirmPass.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtConfirmPass.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtConfirmPass.Location = new System.Drawing.Point(195, 788);
            this.txtConfirmPass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtConfirmPass.Name = "txtConfirmPass";
            this.txtConfirmPass.PlaceholderText = "Confirm Password";
            this.txtConfirmPass.SelectedText = "";
            this.txtConfirmPass.Size = new System.Drawing.Size(175, 49);
            this.txtConfirmPass.TabIndex = 15;
            this.txtConfirmPass.Validating += new System.ComponentModel.CancelEventHandler(this.txtConfirmPass_Validating);
            // 
            // txtNewPass
            // 
            this.txtNewPass.BackColor = System.Drawing.Color.Transparent;
            this.txtNewPass.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(79)))), ((int)(((byte)(60)))));
            this.txtNewPass.BorderRadius = 10;
            this.txtNewPass.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNewPass.DefaultText = "";
            this.txtNewPass.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNewPass.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNewPass.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNewPass.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNewPass.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.txtNewPass.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNewPass.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNewPass.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtNewPass.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNewPass.Location = new System.Drawing.Point(195, 717);
            this.txtNewPass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNewPass.Name = "txtNewPass";
            this.txtNewPass.PlaceholderText = "New Password";
            this.txtNewPass.SelectedText = "";
            this.txtNewPass.Size = new System.Drawing.Size(175, 49);
            this.txtNewPass.TabIndex = 14;
            this.txtNewPass.Validating += new System.ComponentModel.CancelEventHandler(this.txtNewPass_Validating);
            // 
            // txtCurrentPass
            // 
            this.txtCurrentPass.BackColor = System.Drawing.Color.Transparent;
            this.txtCurrentPass.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(79)))), ((int)(((byte)(60)))));
            this.txtCurrentPass.BorderRadius = 10;
            this.txtCurrentPass.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCurrentPass.DefaultText = "";
            this.txtCurrentPass.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCurrentPass.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCurrentPass.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCurrentPass.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCurrentPass.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(30)))), ((int)(((byte)(34)))));
            this.txtCurrentPass.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCurrentPass.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCurrentPass.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtCurrentPass.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCurrentPass.Location = new System.Drawing.Point(195, 641);
            this.txtCurrentPass.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCurrentPass.Name = "txtCurrentPass";
            this.txtCurrentPass.PlaceholderText = "Password";
            this.txtCurrentPass.SelectedText = "";
            this.txtCurrentPass.Size = new System.Drawing.Size(175, 49);
            this.txtCurrentPass.TabIndex = 13;
            this.txtCurrentPass.Validating += new System.ComponentModel.CancelEventHandler(this.txtCurrentPass_Validating);
            // 
            // lblConfirmPass
            // 
            this.lblConfirmPass.AutoSize = true;
            this.lblConfirmPass.BackColor = System.Drawing.Color.Transparent;
            this.lblConfirmPass.Font = new System.Drawing.Font("Georgia", 10F);
            this.lblConfirmPass.ForeColor = System.Drawing.Color.White;
            this.lblConfirmPass.Location = new System.Drawing.Point(22, 805);
            this.lblConfirmPass.Name = "lblConfirmPass";
            this.lblConfirmPass.Size = new System.Drawing.Size(153, 20);
            this.lblConfirmPass.TabIndex = 12;
            this.lblConfirmPass.Text = "Confirm Password:";
            // 
            // lblNewPass
            // 
            this.lblNewPass.AutoSize = true;
            this.lblNewPass.BackColor = System.Drawing.Color.Transparent;
            this.lblNewPass.Font = new System.Drawing.Font("Georgia", 10F);
            this.lblNewPass.ForeColor = System.Drawing.Color.White;
            this.lblNewPass.Location = new System.Drawing.Point(37, 734);
            this.lblNewPass.Name = "lblNewPass";
            this.lblNewPass.Size = new System.Drawing.Size(124, 20);
            this.lblNewPass.TabIndex = 11;
            this.lblNewPass.Text = "New Password:";
            // 
            // lblCurrentPass
            // 
            this.lblCurrentPass.AutoSize = true;
            this.lblCurrentPass.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentPass.Font = new System.Drawing.Font("Georgia", 10F);
            this.lblCurrentPass.ForeColor = System.Drawing.Color.White;
            this.lblCurrentPass.Location = new System.Drawing.Point(12, 653);
            this.lblCurrentPass.Name = "lblCurrentPass";
            this.lblCurrentPass.Size = new System.Drawing.Size(149, 20);
            this.lblCurrentPass.TabIndex = 10;
            this.lblCurrentPass.Text = "Current Password:";
            // 
            // frmChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 850);
            this.Controls.Add(this.guna2GradientPanel1);
            this.Name = "frmChangePassword";
            this.Text = "frmChangePassword";
            this.Load += new System.EventHandler(this.frmChangePassword_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.guna2GradientPanel1.ResumeLayout(false);
            this.guna2GradientPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UCPersonInfo ucPersonInfo1;
        private UCLoginInfo ucLoginInfo1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private Guna.UI2.WinForms.Guna2GradientPanel guna2GradientPanel1;
        private System.Windows.Forms.Label lblConfirmPass;
        private System.Windows.Forms.Label lblNewPass;
        private System.Windows.Forms.Label lblCurrentPass;
        private Guna.UI2.WinForms.Guna2TextBox txtNewPass;
        private Guna.UI2.WinForms.Guna2TextBox txtCurrentPass;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmPass;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button btnClsoe;
    }
}