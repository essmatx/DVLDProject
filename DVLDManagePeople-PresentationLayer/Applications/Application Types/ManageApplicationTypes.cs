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
    public partial class frmManageApplicationTypes : Form
    {
        private DataTable _dataTable; 

        private void _RefreshList()
        {
            dataGridView1.AutoGenerateColumns = true; 
            _dataTable = ClsApplicationTypeBusiness.GetAllApplicationTypes();

            dataGridView1.DataSource = _dataTable; 
        }
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void ManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _RefreshList(); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void editieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select an item from the list first.", "No Selection",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }


            int SelectedItem = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmUpdateApplicationType frm = new frmUpdateApplicationType(SelectedItem);

            frm.ShowDialog();

            _RefreshList(); 
        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
