using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class FormSplash : Form
    {
        private Guna2PictureBox pbLogo;
        private Guna2HtmlLabel lblTitle;
        private Guna2HtmlLabel lblSubtitle1;
        private Guna2HtmlLabel lblSubtitle2;

        // Status and Progress Controls
        //private Guna2HtmlLabel lblStatus;
        private Guna2ProgressBar progressBar;

        public FormSplash()
        {
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(500, 360);
            this.BackColor = Color.FromArgb(24, 24, 24);

            // --- Existing Logo & Titles ---
            pbLogo = new Guna2PictureBox
            {
                Size = new Size(120, 80),
                Location = new Point((this.ClientSize.Width - 120) / 2, 30),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = Properties.Resources.Screenshot_2026_05_17_140541 // Replace with your image
            };

            lblTitle = new Guna2HtmlLabel
            {
                Text = "DVLD",
                ForeColor = Color.FromArgb(228, 185, 119),
                Font = new Font("Georgia", 28, FontStyle.Bold),
                TextAlignment = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(300, 45),
                Location = new Point((this.ClientSize.Width - 300) / 2, 115)
            };

            lblSubtitle1 = new Guna2HtmlLabel
            {
                Text = "DRIVING & VEHICLE",
                ForeColor = Color.FromArgb(120, 110, 95),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                TextAlignment = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(300, 20),
                Location = new Point((this.ClientSize.Width - 300) / 2, 165)
            };

            lblSubtitle2 = new Guna2HtmlLabel
            {
                Text = "LICENSING",
                ForeColor = Color.FromArgb(120, 110, 95),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                TextAlignment = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(300, 20),
                Location = new Point((this.ClientSize.Width - 300) / 2, 185)
            };

            // --- Status Label ---
            lblStatus = new Guna2HtmlLabel
            {
                Text = "Loading application...",
                ForeColor = Color.FromArgb(150, 150, 150),
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                TextAlignment = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Size = new Size(420, 20),
                Location = new Point(40, 255)
            };

            // --- Progress Bar ---
            progressBar = new Guna2ProgressBar
            {
                Size = new Size(420, 8),
                Location = new Point(40, 280),
                ProgressColor = Color.FromArgb(228, 185, 119),
                ProgressColor2 = Color.FromArgb(228, 185, 119),
                FillColor = Color.FromArgb(40, 40, 40),
                BorderRadius = 4,
                Minimum = 0,
                Maximum = 100,
                Value = 0
            };

            // Add controls
            this.Controls.Add(pbLogo);
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle1);
            this.Controls.Add(lblSubtitle2);
            this.Controls.Add(lblStatus);
            this.Controls.Add(progressBar);
        }

        // Thread-safe update method for progress and message
        public void UpdateProgress(int percent, string message)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => UpdateProgress(percent, message)));
                return;
            }

            lblStatus.Text = message;
            progressBar.Value = Math.Min(100, Math.Max(0, percent));
        }

        public void UpdateStatus(string message)
        {
            UpdateProgress(progressBar?.Value ?? 0, message);
        }
    }
}