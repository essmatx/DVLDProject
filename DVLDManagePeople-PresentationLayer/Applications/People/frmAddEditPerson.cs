using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDManagePeople_PresentationLayer
{
    public partial class frmAddUpdatePerson : Form
    {

        public delegate void DataBackEventHandler(object sender, int PersonID);

        public event DataBackEventHandler DataBack; 
        public enum enMode {AddNew = 0, Update = 1 };
        enMode Mode = enMode.AddNew;
        private int _personID = -1 ;

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _personID = PersonID;

            this.Mode = enMode.Update;
        }
        public frmAddUpdatePerson()
        {
            InitializeComponent();

            this.Mode = enMode.AddNew; 
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            if(Mode == enMode.Update)
            {
                

                lblPersonID.Text = _personID.ToString();

                //userControl11.LoadData(_personID); 
                ucAddPerson1.LoadData(_personID);

            }
            else
            {
                
                lblPersonID.Text = "N/A";
               
            }
        }

        private void userControl11_Load(object sender, EventArgs e)
        {
            //userControl11.OnSaveSuccess += userControl11_OnSaveSuccess; 
            ucAddPerson1.OnSaveSuccess += userControl11_OnSaveSuccess;
        }

        private void userControl11_OnSaveSuccess(object sender,int PersonID)
        {
            DataBack?.Invoke(this, PersonID);

            this.Close();
        }
        private void userControl11_Load_1(object sender, EventArgs e)
        {

        }
    }
}
