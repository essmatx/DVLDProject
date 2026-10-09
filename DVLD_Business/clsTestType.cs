using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_Business
{
    public class ClsTestTypeBusiness
    {
        #region Enums & Modes

        /// <summary>
        /// Specifies whether an operation adds a new item or updates an existing one.
        /// </summary>
        /// <remarks>Use AddNew for create operations and Update for modify operations.</remarks>
        public enum enMode { AddNew = 0, Update = 1 };
       
        /// <summary>
        /// Specifies the type of test.
        /// </summary>
        /// <remarks>Used to categorize assessment formats such as Vision, Written, and
        /// Practical.</remarks>
        public enum enTestType { Vision = 1, Written = 2, Practical = 3 };

        /// <summary>
        /// Specifies the current mode of operation.
        /// </summary>
        /// <remarks>Initialized to enMode.AddNew by default.</remarks>
        public enMode Mode = enMode.AddNew;
        #endregion

        #region Scalar Properties

        /// <summary>
        /// Gets or sets the test type identifier.
        /// </summary>
        /// <remarks>Represents the test type using the ClsTestTypeBusiness.enTestType
        /// enumeration.</remarks>
        public ClsTestTypeBusiness.enTestType TestTypeID { set; get; }

        /// <summary>
        /// Gets or sets the title of the test type.
        /// </summary>
        /// <remarks>Used for display and identification. May be null or empty.</remarks>
        public string TestTypeTitle { set; get; }

        /// <summary>
        /// Gets or sets the description of the test type.
        /// </summary>
        public string TestTypeDescription { set; get; }

        /// <summary>
        /// Gets or sets the fee amount for the test type.
        /// </summary>
        /// <remarks>Represents a monetary value; perform currency labeling, rounding, and validation
        /// outside this property.</remarks>
        public decimal TestTypeFees { set; get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClsTestTypeBusiness class with default property values.
        /// </summary>
        /// <remarks>Sets TestTypeID to enTestType.Vision, TestTypeTitle and TestTypeDescription to empty
        /// strings, TestTypeFees to 0, and Mode to enMode.AddNew.</remarks>
        public ClsTestTypeBusiness()
        {
            this.TestTypeID = ClsTestTypeBusiness.enTestType.Vision;
            this.TestTypeTitle = "";
            this.TestTypeDescription = "";
            this.TestTypeFees = 0;
            Mode = enMode.AddNew;

        }

       /// <summary>
       /// Initializes a new instance of ClsTestTypeBusiness with the specified test type identifier, title,
       /// description, and fees, and sets Mode to enMode.Update.
       /// </summary>
       /// <remarks>Private constructor used to populate an existing instance for update scenarios; Mode is
       /// set to enMode.Update.</remarks>
       /// <param name="ID">The test type identifier.</param>
       /// <param name="TestTypeTitle">The test type title.</param>
       /// <param name="TestTypeDescription">The test type description.</param>
       /// <param name="TestTypeFees">The test type fees.</param>
        private ClsTestTypeBusiness(ClsTestTypeBusiness.enTestType ID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {
            this.TestTypeID = ID;

            this.TestTypeTitle = TestTypeTitle;

            this.TestTypeDescription = TestTypeDescription;

            this.TestTypeFees = TestTypeFees;
            this.Mode = enMode.Update; 
        }
        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Creates a new test type via the data access layer and assigns the returned enum value to the TestTypeID
        /// property.
        /// </summary>
        /// <remarks>Calls clsTestTypeData.AddNewTestType and casts the result to
        /// ClsTestTypeBusiness.enTestType before assigning to TestTypeID. No additional validation is
        /// performed.</remarks>
        /// <returns>True if TestTypeTitle is not empty; otherwise, false.</returns>
        private bool _AddNewTestType()
        {
            //call DataAccess Layer 

            this.TestTypeID = (ClsTestTypeBusiness.enTestType)clsTestTypeData.AddNewTestType(this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);

            return (this.TestTypeTitle != "");
        }

        /// <summary>
        /// Updates the test type identified by TestTypeID with the current title, description, and fees.
        /// </summary>
        /// <remarks>Delegates to clsTestTypeData.UpdateTestType.</remarks>
        /// <returns>true if the update succeeded; otherwise, false.</returns>
        private bool _UpdateTestType()
        {
            return clsTestTypeData.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        }
        #endregion

        #region Factory & Search Methods
        /// <summary>
        /// Finds and hydrates a ClsTestTypeBusiness for the specified test type enum identifier.
        /// </summary>
        /// <remarks>Retrieves Title, Description, and Fees from the data layer and constructs the
        /// business object; returns null when the record cannot be found.</remarks>
        /// <param name="TestTypeID">The strongly-typed enum value that identifies the test type.</param>
        /// <returns>A ClsTestTypeBusiness populated with Title, Description, and Fees for the specified ID, or null if no record
        /// exists.</returns>
        [DocInfo("Finds and hydrates a test type record by strongly-typed enum ID.")]
        public static ClsTestTypeBusiness FindTestTypeByID(ClsTestTypeBusiness.enTestType TestTypeID)
        {
            string Title = "", Description = ""; decimal Fees = 0;

            if (clsTestTypeData.FindTestTypeByID((int)TestTypeID, ref Title, ref Description,ref Fees))

                return new ClsTestTypeBusiness(TestTypeID, Title, Description, Fees);
            else
                return null;

        }

        /// <summary>
        /// Retrieves a DataTable containing all configured test types for use in management grids and dropdown
        /// controls.
        /// </summary>
        /// <remarks>Intended for populating management grids and dropdown controls.</remarks>
        /// <returns>A DataTable containing the configured test types.</returns>

        [DocInfo("Retrieves complete list of configured test types for management grids and dropdown controls.")]
        public static DataTable GetAllTestTypess()
        {
            return clsTestTypeData.GetAllTestTypes();

        }
        #endregion

        #region Persistence Operations
        /// <summary>
        /// Persists the test type catalog record to storage based on the current Mode.
        /// </summary>
        /// <remarks>When Mode is enMode.AddNew and the add succeeds, Mode is set to enMode.Update. Only
        /// enMode.AddNew and enMode.Update are handled; other modes result in no action and return false.</remarks>
        /// <returns>True if the record was added or updated successfully; otherwise, false.</returns>
        [DocInfo("Persists test type catalog record to storage based on entity Mode state.")]
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;
                case enMode.Update:
                    return _UpdateTestType();
            }
            return false;
        }
        #endregion

    }
}
