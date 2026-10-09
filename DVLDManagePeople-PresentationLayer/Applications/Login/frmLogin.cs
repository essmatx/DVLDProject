using DVLD_Business;
using DVLDManagePeople_PresentationLayer.Global_Classes;
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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = ""; 

            if(clsGlobal.GetStoredCredential(ref UserName , ref Password))
            {
                txtUserName.Text = UserName;
                txtPassowrd.Text = Password;
                chkRememberMe.Checked = true; 
            }
            else
            {
                chkRememberMe.Checked = false; 
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            ClsUserBuiness user = ClsUserBuiness.FindUserByUserNameAndPassowrd(txtUserName.Text.Trim(), txtPassowrd.Text.Trim()); 

            if(user != null)
            {
                if(chkRememberMe.Checked)
                {
                    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassowrd.Text.Trim()); 

                }
                else
                {
                    clsGlobal.RememberUsernameAndPassword("", "");
                }

                if(!user.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = user;
                this.Hide();
               frmDashboard frm = new frmDashboard(this);
               frm.ShowDialog(); 
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtPassowrd_IconRightClick(object sender, EventArgs e)
        {
            if (txtPassowrd.UseSystemPasswordChar == true)
            {
                txtPassowrd.UseSystemPasswordChar = false;
                // Optional: Change the icon to a "hidden" eye if you have one
            }
            else
            {
                txtPassowrd.UseSystemPasswordChar = true;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();

            frm.ShowDialog(); 
        }

        private void txtPassowrd_Enter(object sender, EventArgs e)
        {
            btnEye.ForeColor = Color.Gold;
        }

        private void txtPassowrd_Leave(object sender, EventArgs e)
        {
            btnEye.ForeColor = Color.DimGray;
        }

        private void btnEye_Click(object sender, EventArgs e)
        {
            if (txtPassowrd.UseSystemPasswordChar == true)
            {
                // Show the password
                txtPassowrd.UseSystemPasswordChar = false;
            }
            else
            {
                // Hide the password (mask it with dots)
                txtPassowrd.UseSystemPasswordChar = true;
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {

        }

        private void llCreateaccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();

            frm.ShowDialog(); 
        }
    }
}
