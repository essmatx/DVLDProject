
using DVLD_Business; 
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
    
    public partial class frmManageDrivers : Form
    {
        private DataTable _dtAllDivers;
        public frmManageDrivers()
        {
            InitializeComponent();
        }

        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            cbFilter.Items.Clear();
            cbFilter.Items.Add("None");
            cbFilter.Items.Add("Driver ID");
            cbFilter.Items.Add("Person ID");
            cbFilter.Items.Add("National No");
            cbFilter.Items.Add("Full Name");

            cbFilter.SelectedIndex = 0;

            _RefreshDriversList();
        }


        private void _RefreshDriversList()
        {
            _dtAllDivers = ClsDriverBusiness.GetAllDriveres();
            dgvDrivers.DataSource = _dtAllDivers; 
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dtAllDivers == null)
            {
                return;
            }

            _dtAllDivers.DefaultView.RowFilter = "";

            string SelectedFilter = cbFilter.Text;

            switch (SelectedFilter)
            {
                case "None":
                    break;

                case "Driver ID":
                    _dtAllDivers.DefaultView.RowFilter = "[Driver ID] IS NOT NULL";
                    break;

                case "Person ID":
                    _dtAllDivers.DefaultView.RowFilter = "[Person ID] IS NOT NULL";
                    break;

                case "National No.":
                    _dtAllDivers.DefaultView.RowFilter = "[National No.] <> ''";
                    break;

                case "Full Name":
                    _dtAllDivers.DefaultView.RowFilter = "[Full Name] <> ''";
                    break; 
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = "";

            switch (cbFilter.Text)
            {
                case "Person ID": filterColumn = "PersonID"; break;
                case "National No.": filterColumn = "NationalNo"; break;
                case "First Name": filterColumn = "FirstName"; break;
                case "Second Name": filterColumn = "SecondName"; break;
                case "Third Name": filterColumn = "ThirdName"; break;
                case "Last Name": filterColumn = "LastName"; break;
                case "Nationality": filterColumn = "CountryName"; break;
                case "Gendor": filterColumn = "Gendor"; break;
                case "Phone": filterColumn = "Phone"; break;
                case "Email": filterColumn = "Email"; break;
                default: filterColumn = "None"; break;
            }

            // If "None" is selected or the search box is empty, show all records
            if (txtFilterValue.Text.Trim() == "" || filterColumn == "None")
            {
                ((DataTable)dgvDrivers.DataSource).DefaultView.RowFilter = "";
                return;
            }

            if (filterColumn == "PersonID")
            {
                // PersonID is a number: use '=' without single quotes
                if (int.TryParse(txtFilterValue.Text.Trim(), out int parsedID))
                {
                    ((DataTable)dgvDrivers.DataSource).DefaultView.RowFilter =
                   string.Format("[{0}] = {1}", filterColumn, parsedID);
                }
                else
                {
                    ((DataTable)dgvDrivers.DataSource).DefaultView.RowFilter = "";
                }

            }
            else
            {
                // Text columns: use 'LIKE' with single quotes
                ((DataTable)dgvDrivers.DataSource).DefaultView.RowFilter =
                    string.Format("[{0}] LIKE '{1}%'", filterColumn, txtFilterValue.Text.Trim());
            }
        }
    }
}
