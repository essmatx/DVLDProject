using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmInternationalDrivingLicenseInfo : Form
    {
        public frmInternationalDrivingLicenseInfo()
        {
            InitializeComponent();
        }

        public frmInternationalDrivingLicenseInfo(int InternationalLicenseID)
        {
            InitializeComponent();
            ucInternationalLicenseInfo1.LoadInternationalLicenseInfo(InternationalLicenseID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
