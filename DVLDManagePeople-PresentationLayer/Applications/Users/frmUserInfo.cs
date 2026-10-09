
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business; 

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmUserInfo : Form
    {
        private int _UserID;

        private ClsUserBuiness _User; 
        public frmUserInfo(int UserID)
        {
            InitializeComponent();

            _UserID = UserID; 
        }

        private void frmUserInfo_Load(object sender, EventArgs e)
        {
            _User = ClsUserBuiness.FindUserByID(_UserID); 

            if(_User == null)
            {
                MessageBox.Show("Could not find this user in the system.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
                return; 
            }

            ucLoginInfo1.LoadByUserID(_User.UserID); 

            ucPersonInfo1.LoadPersonInfo(_User.PersonID); 
        }
    }
}
