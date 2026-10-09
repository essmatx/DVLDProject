using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessWorkflows;
using DVLD_Business; 
namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmManagePeople : Form
    {
        private DataTable _dataTable;

        private void _RefreshPeopleList()
        {
            try
            {
                _dataTable = ClsPerson.GetAllPeople();

                if(_dataTable == null)
                {
                    MessageBox.Show("The Database returned a NULL table! Check your Connection String.",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dataGridView1.DataSource = _dataTable; 

                if(_dataTable.Rows.Count == 0)
                {
                    MessageBox.Show("Connection successful, but your 'People' table contains 0 records!",
                            "Database Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Critical Database Exception: {ex.Message}\n\nTarget Method: {ex.TargetSite}",
                        "Data Layer Crash Hooked", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        public frmManagePeople()
        {
            InitializeComponent();

            showDetailsToolStripMenuItem.Click += showDetailsToolStripMenuItem_Click;
            editToolStripMenuItem.Click += editToolStripMenuItem_Click;
            deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            
           

        }


        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {

            string filterColumn = "";

            switch (cbFilterBy.Text)
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
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = "";
                return;
            }

            if (filterColumn == "PersonID")
            {
                // PersonID is a number: use '=' without single quotes
                if (int.TryParse(txtFilterValue.Text.Trim(), out int parsedID))
                {
                    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter =
                   string.Format("[{0}] = {1}", filterColumn, parsedID);
                }
                else
                {
                    ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter = "";
                }

            }
            else
            {
                // Text columns: use 'LIKE' with single quotes
                ((DataTable)dataGridView1.DataSource).DefaultView.RowFilter =
                    string.Format("[{0}] LIKE '{1}%'", filterColumn, txtFilterValue.Text.Trim());
            }
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

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                var hit = dataGridView1.HitTest(e.X, e.Y); 

                if(hit.RowIndex >= 0)
                {
                    dataGridView1.ClearSelection();

                    dataGridView1.Rows[hit.RowIndex].Selected = true;

                    dataGridView1.CurrentCell = dataGridView1.Rows[hit.RowIndex].Cells[0]; 
                }
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0) return;

            int PersonID = (int)dataGridView1.CurrentRow.Cells["PersonID"].Value;

            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);

            frm.ShowDialog(); 


        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0) return;
            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson();

           frmAddUpdatePerson.ShowDialog();

            _RefreshPeopleList(); 
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null || dataGridView1.CurrentRow.Index < 0) return;

            int PersonID = (int)dataGridView1.CurrentRow.Cells["PersonID"].Value;

            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson(PersonID);

            frmAddUpdatePerson.ShowDialog();

            _RefreshPeopleList();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();

            frm.ShowDialog(); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            cbFilterBy.Items.Clear();

            cbFilterBy.Items.Add("None");
            cbFilterBy.Items.Add("Person ID");
            cbFilterBy.Items.Add("National No.");
            cbFilterBy.Items.Add("First Name");
            cbFilterBy.Items.Add("Second Name");
            cbFilterBy.Items.Add("Third Name");
            cbFilterBy.Items.Add("Last Name");
            cbFilterBy.Items.Add("Nationality");
            cbFilterBy.Items.Add("Gendor");
            cbFilterBy.Items.Add("Phone");
            cbFilterBy.Items.Add("Email");

            cbFilterBy.SelectedIndex = 0;

            _RefreshPeopleList();



        }

        private void btnAddNew_Click_1(object sender, EventArgs e)
        {
            frmAddUpdatePerson frmAddUpdatePerson = new frmAddUpdatePerson();

            frmAddUpdatePerson.ShowDialog();

            _RefreshPeopleList();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void txtFilterValue_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
