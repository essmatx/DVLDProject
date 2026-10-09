
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
using DVLDManagePeople_PresentationLayer.Global_Classes;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmManageUsers : Form
    {

        DataTable _dataTable;
        public frmManageUsers()
        {
            InitializeComponent();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.Add("None");
            cbFilterBy.Items.Add("User ID");
            cbFilterBy.Items.Add("User Name");
            cbFilterBy.Items.Add("Person ID");
            cbFilterBy.Items.Add("Full Name");
            cbFilterBy.Items.Add("Is Active");
            cbIsActive.Items.Add("Yes");
            cbIsActive.Items.Add("No");
            cbIsActive.Items.Add("All");

            _RefreshUserList();

            cbIsActive.SelectedIndex = 0; 
            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                txtFilterValue.Enabled = false;

                cbIsActive.Enabled = true;
                cbIsActive.Visible = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            }

            else
            {
                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsActive.Visible = false;



                if (cbFilterBy.Text == "None")
                {
                    txtFilterValue.Enabled = false;
                }
                else
                {
                    txtFilterValue.Enabled = true;
                }

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = "";

            switch (cbFilterBy.Text)
            {
                case "User ID": filterColumn = "UserID"; break;
                case "User Name": filterColumn = "UserName"; break;
                case "Person ID": filterColumn = "PersonID"; break;
                case "Full Name": filterColumn = "FullName"; break;
                default: filterColumn = "None"; break;
            }

            // If "None" is selected or the search box is empty, show all records
            if (txtFilterValue.Text.Trim() == "" || filterColumn == "None")
            {
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = "";
                return;
            }

            if (filterColumn == "PersonID")
            {
                // PersonID is a number: use '=' without single quotes
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter =
                    string.Format("[{0}] = {1}", filterColumn, txtFilterValue.Text.Trim());
            }
            if (filterColumn == "UserID")
            {
                // PersonID is a number: use '=' without single quotes
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter =
                    string.Format("[{0}] = {1}", filterColumn, txtFilterValue.Text.Trim());
            }

            else
            {
                // Text columns: use 'LIKE' with single quotes
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter =
                    string.Format("[{0}] LIKE '{1}%'", filterColumn, txtFilterValue.Text.Trim());
            }  }

        private void _RefreshUserList()
        {
            _dataTable = ClsUserBuiness.GetAllUsers();

          dataGridView1.DataSource = _dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.ShowDialog();

            _RefreshUserList();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            // 1. Get the object reference
            object cellValue = dataGridView1.CurrentRow.Cells["PersonID"].Value;

            // 2. Check for null or DBNull
            if (cellValue == null || cellValue == DBNull.Value)
            {
                // Optionally show a message box here, or just return
                return;
            }

            // 3. Convert safely
            int SelectedPersonID = Convert.ToInt32(cellValue);

            frmUserInfo frm = new frmUserInfo(SelectedPersonID);
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.ShowDialog();

            _RefreshUserList(); 
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a user from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning); 
            }

            int SelectedUserID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            frmAddEditUser frm = new frmAddEditUser(SelectedUserID);

            frm.ShowDialog();

            _RefreshUserList(); 
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a user from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            int SelectedUserID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value); 

            if(SelectedUserID == clsGlobal.CurrentUser.UserID)
            {
                MessageBox.Show("You cannot delete your own account while you are logged into a system.", "Access Denied",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                return; 
            }

            if (MessageBox.Show("Are you sure you want permanently Delete this user account ?", "Confirm Deletion", 
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                if (ClsUserBuiness.DeleteUser(SelectedUserID))
                {
                    MessageBox.Show("User account was successfully deleted from the system.", "Deleted",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    _RefreshUserList(); 
                }
                else
                {
                    MessageBox.Show("This user account cannot be deleted because it is linked to active system records/data.", "Deletion Blocked",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a user from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            int SelectedUserID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            frmChangePassword frm = new frmChangePassword(SelectedUserID);

            frm.ShowDialog(); 
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text; 

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }

            if(FilterValue == "All")
            {
                _dataTable.DefaultView.RowFilter = ""; 
            }
            else
            {
                _dataTable.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            }
        }
    }
}
