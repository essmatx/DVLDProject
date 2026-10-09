
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
    public partial class frmManageTestTypes : Form
    {

        private DataTable _dataTable; 
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void _RefreshList()
        {
            _dataTable = ClsTestTypeBusiness.GetAllTestTypess();

            dataGridView1.DataSource = _dataTable; 
        }
        private void edditeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select one from the Item list");

                return; 
            }

            int SelectedItem = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmUpdateTestType frmUpdateTest = new frmUpdateTestType(SelectedItem);

            frmUpdateTest.ShowDialog();

            _RefreshList(); 
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            _RefreshList(); 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
