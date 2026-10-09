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
    public partial class frmLicenseInfo : Form
    {
        private int _LicenseID = -1; 
        public frmLicenseInfo(int LicenseID)
        {
            _LicenseID = LicenseID; 
            InitializeComponent();
        }

        private void frmLicenseInfo_Load(object sender, EventArgs e)
        {
            if (_LicenseID == -1)
            {
                MessageBox.Show("Error: No valid License ID was passed to this form.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return; 
            }
            ucLicenseInfo1.LoadLicenseInfo(_LicenseID); 
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
