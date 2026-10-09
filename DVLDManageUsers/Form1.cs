using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDUser_BusinessTier;
using ClsPersonBusiness; 


namespace DVLDManageUsers
{
    public partial class Form1 : Form
    {
        private DataTable _dataTable; 
        public Form1()
        {
            InitializeComponent();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
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
                case "Is Active": filterColumn = "IsActive"; break;
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
            }
        }

        private void _RefreshUserList()
        {
            _dataTable = ClsUserBuiness.GetAllUsers();

            dataGridView1.DataSource = _dataTable; 
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.Add("None");
            cbFilterBy.Items.Add("User ID");
            cbFilterBy.Items.Add("User Name.");
            cbFilterBy.Items.Add("Person ID");
            cbFilterBy.Items.Add("Full Name");
            cbFilterBy.Items.Add(" Is Activee");
            cbFilterBy.SelectedIndex = 0;

            _RefreshUserList();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {

        }
    }
    
}
