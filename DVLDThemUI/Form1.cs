using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dvld.Theme; 



namespace DVLDThemUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DvldTheme.Apply(this);

             .Font = DvldTheme.DisplayFont(28);
            titleLabel.ForeColor = DvldTheme.Gold;

            loginButton.Tag = "primary";
            DvldTheme.StyleButton(loginButton, primary: true);
        }
    }
}
