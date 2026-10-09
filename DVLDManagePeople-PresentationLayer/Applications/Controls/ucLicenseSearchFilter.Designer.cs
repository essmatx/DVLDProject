namespace DVLDManagePeople_PresentationLayer
{
    partial class ucLicenseSearchFilter
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSearch = new Guna.UI2.WinForms.Guna2Button();
            this.txtLicenseIDFilter = new Guna.UI2.WinForms.Guna2TextBox();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BorderRadius = 20;
            this.btnSearch.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSearch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Image = global::DVLDManagePeople_PresentationLayer.Properties.Resources.driving_license1;
            this.btnSearch.Location = new System.Drawing.Point(274, 13);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(122, 38);
            this.btnSearch.TabIndex = 44;
            this.btnSearch.Text = "Search ";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtLicenseIDFilter
            // 
            this.txtLicenseIDFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLicenseIDFilter.BackColor = System.Drawing.Color.Transparent;
            this.txtLicenseIDFilter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(206)))), ((int)(((byte)(134)))));
            this.txtLicenseIDFilter.BorderRadius = 18;
            this.txtLicenseIDFilter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLicenseIDFilter.DefaultText = "";
            this.txtLicenseIDFilter.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtLicenseIDFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtLicenseIDFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtLicenseIDFilter.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtLicenseIDFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLicenseIDFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtLicenseIDFilter.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtLicenseIDFilter.IconLeft = global::DVLDManagePeople_PresentationLayer.Properties.Resources.search;
            this.txtLicenseIDFilter.Location = new System.Drawing.Point(9, 14);
            this.txtLicenseIDFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtLicenseIDFilter.Name = "txtLicenseIDFilter";
            this.txtLicenseIDFilter.PlaceholderForeColor = System.Drawing.Color.Black;
            this.txtLicenseIDFilter.PlaceholderText = "           License ID 123...";
            this.txtLicenseIDFilter.SelectedText = "";
            this.txtLicenseIDFilter.Size = new System.Drawing.Size(245, 37);
            this.txtLicenseIDFilter.TabIndex = 45;
            // 
            // ucLicenseSearchFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.Controls.Add(this.txtLicenseIDFilter);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucLicenseSearchFilter";
            this.Size = new System.Drawing.Size(439, 64);
            this.Load += new System.EventHandler(this.ucLicenseSearchFilter_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Button btnSearch;
        private Guna.UI2.WinForms.Guna2TextBox txtLicenseIDFilter;
    }
}
