using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_Business
{
    /// <summary>
    /// Represents a test result record with associated appointment and user information, and provides lookup,
    /// persistence, and deletion operations.
    /// </summary>
    /// <remarks>Use the Mode enum to control Save behavior: AddNew creates a new record (checks for an
    /// existing test for the appointment and locks the appointment on success) and Update applies changes to an
    /// existing record. Factory methods load records by test ID or appointment ID; static helpers provide deletion and
    /// retrieval of all tests and a check for whether an appointment already has a test. The default constructor
    /// initializes sensible defaults; loading constructors and factory methods populate related UserInfo and
    /// TestAppointmentinfo.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Core business entity managing driving test results, scoring, examiner notes, and appointment lifecycle locks.", Module = "Test Management", Version = "1.0")]
    public class ClsTestBuisness
    {
        #region Enums & Modes

        /// <summary>
        /// Defines whether an operation adds a new item or updates an existing item.
        /// </summary>
        /// <remarks>Used to control behavior in create/update workflows: AddNew indicates creation,
        /// Update indicates modification.</remarks>
        public enum enMode { AddNew = 0, Update = 1 };

        /// <summary>
        /// Gets or sets the current mode.
        /// </summary>
        /// <remarks>Specifies an enMode enumeration value that determines the component's operating
        /// mode.</remarks>
        public enMode Mode { get; set; }
        #endregion

        #region Scalar & Navigation Properties
        /// <summary>
        /// Gets or sets the test identifier.
        /// </summary>
        /// <remarks>Typically used as the primary key for the entity in persistence scenarios.</remarks>
        public int TestID { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the test appointment.
        /// </summary>
        public int TestAppointmentID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the test succeeded.
        /// </summary>
        public bool TestResult { get; set; }

        /// <summary>
        /// Gets or sets free-form notes or remarks associated with the object.
        /// </summary>
        /// <remarks>Intended for human-readable comments; may contain plain text or simple markup. May be
        /// null or empty.</remarks>
        public string Notes { get; set; }

        /// <summary>
       /// Gets or sets the identifier of the user who created the entity.
       /// </summary>
       /// <remarks>Corresponds to the primary key of the user account that created the entity.</remarks>
        public int CreatedByUserID { get; set; }
        

        /// <summary>
        /// Gets or sets the test appointment business information.
        /// </summary>
        public ClsTestAppointmentBusiness TestAppointmentinfo { get; set; }

        /// <summary>
        /// Gets or sets the user's business information.
        /// </summary>
        public ClsUserBuiness UserInfo { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ClsTestBuisness class and sets default property values.
        /// </summary>
        /// <remarks>Sets TestID and TestAppointmentID to -1, TestResult to false, Notes to an empty
        /// string, CreatedByUserID to -1, initializes UserInfo and TestAppointmentinfo, and sets Mode to
        /// enMode.AddNew.</remarks>
        public ClsTestBuisness()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;

            this.UserInfo = new ClsUserBuiness();
            this.TestAppointmentinfo = new ClsTestAppointmentBusiness();

            this.Mode = enMode.AddNew;
        }

        /// <summary>
        /// Creates an instance representing an existing test and loads related user and appointment information.
        /// </summary>
        /// <remarks>Sets Mode to Update and populates UserInfo and TestAppointmentinfo using the provided
        /// identifiers.</remarks>
        /// <param name="TestID">Identifier of the test record.</param>
        /// <param name="TestAppointmentID">Identifier of the associated test appointment.</param>
        /// <param name="TestResult">Result of the test.</param>
        /// <param name="Notes">Notes or comments associated with the test.</param>
        /// <param name="CreatedByUserID">Identifier of the user who created the test record.</param>
        private ClsTestBuisness(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;

            this.Mode = enMode.Update;

            this.UserInfo = ClsUserBuiness.FindUserByID(CreatedByUserID);

            this.TestAppointmentinfo = ClsTestAppointmentBusiness.FindTestAppointByID(TestAppointmentID);
        }
        #endregion

        #region Private CRUD Helpers
        /// <summary>
        /// Insert a new test record and assign the resulting identifier to TestID.
        /// </summary>
        /// <remarks>Calls clsTestData.InsertNewTest with TestAppointmentID, TestResult, Notes, and
        /// CreatedByUserID; TestID is set to the returned identifier.</remarks>
        /// <returns>True if a new test record was created and TestID is greater than zero; otherwise false.</returns>
        private bool AddNewTest()
        {
            this.TestID = clsTestData.InsertNewTest(this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);

            return (this.TestID > 0);
        }

        /// <summary>
        /// Updates the test record in the data store using TestID, TestAppointmentID, TestResult, Notes, and
        /// CreatedByUserID.
        /// </summary>
        /// <remarks>Delegates the update operation to clsTestData.UpdateTest.</remarks>
        /// <returns>true if the update succeeded; otherwise, false.</returns>
        private bool UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates a test record by primary key TestID.
        /// </summary>
        /// <param name="TestID">Primary key of the test record to retrieve.</param>
        /// <returns>A ClsTestBuisness representing the test if found; otherwise null.</returns>
        [DocInfo("Finds and hydrates a test record by primary key TestID.")]
        public static ClsTestBuisness FindTestByID(int TestID)
        {
            int TestAppointmentID = -1;

            bool TestResult = false;

            string Notes = "";

            int CreatedByUserID = -1;

            if (clsTestData.FindTestByID(ref TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                ClsTestBuisness TestInfo = new ClsTestBuisness(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);

                return TestInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds and hydrates a test record for the specified appointment identifier.
        /// </summary>
        /// <remarks>Calls clsTestData.FindTestByAppointmentID to retrieve stored values and constructs a
        /// ClsTestBuisness instance when a matching record exists.</remarks>
        /// <param name="TestAppointmentID">The appointment identifier used to locate the test record.</param>
        /// <returns>A hydrated ClsTestBuisness for the specified appointment if a matching record is found; otherwise, null.</returns>

        [DocInfo("Finds and hydrates a test record linked to a specific appointment ID.")]
        public static ClsTestBuisness FindByAppointmentID(int TestAppointmentID)
        {
            int TestID = -1;

            bool TestResult = false;

            string Notes = "";

            int CreatedByUserID = -1;

            if (clsTestData.FindTestByAppointmentID(ref TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))
            {
                ClsTestBuisness TestInfo = new ClsTestBuisness(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);

                return TestInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a DataTable containing all test records for use in data grids and reporting.
        /// </summary>
        /// <remarks>Delegates to clsTestData.GetAllTest and is intended for UI binding and reporting
        /// scenarios.</remarks>
        /// <returns>A DataTable containing all test records; the table is empty if no records are available.</returns>
        [DocInfo("Retrieves complete list of test records for data grids and reporting.")]
        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTest();
        }

        #endregion

        #region Validation, Deletion & Persistence Rules

        /// <summary>
        /// Determines whether the specified appointment has an associated test result.
        /// </summary>
        /// <remarks>Delegates the existence check to the underlying data-layer implementation.</remarks>
        /// <param name="TestAppointmentID">The appointment identifier to check.</param>
        /// <returns>True if the appointment has an associated test result; otherwise, false.</returns>
        [DocInfo("Validates whether a specific appointment already has an associated test result.")]
        public static bool DoesAppointmentHaveTest(int TestAppointmentID)
        {
            return clsTestData.DoesAppointmentHaveTest(TestAppointmentID);
        }

        /// <summary>
        /// Deletes the test record with the specified identifier.
        /// </summary>
        /// <remarks>Delegates to clsTestData.DeleteTest for data-layer deletion.</remarks>
        /// <param name="TestID">Unique identifier of the test record to delete.</param>
        /// <returns>true if the test record was deleted; otherwise, false.</returns>
        [DocInfo("Deletes a test record by TestID.")]
        public static bool DeleteTest(int TestID)
        {
            return clsTestData.DeleteTest(TestID);
        }

        /// <summary>
        /// Persists the test entity to storage and enforces the test appointment lock state. In AddNew mode, validates
        /// that the appointment has no existing test, adds the test, locks the appointment, and sets Mode to Update. In
        /// Update mode, updates the existing test.
        /// </summary>
        /// <remarks>Writes validation and status messages to the console. Duplicate checking is performed
        /// only when creating a new test. After a successful add, the method attempts to lock the appointment via
        /// ClsTestAppointmentBusiness.LockAppointment and will fail if the lock cannot be obtained. The method mutates
        /// Mode and calls external business logic and persistence helpers.</remarks>
        /// <returns>True when the test was successfully persisted and any required appointment lock was acquired; otherwise
        /// false.</returns>
        [DocInfo("Persists test entity to storage and enforces test appointment lock state.")]
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    // FIX: Move the validation here. Only check for duplicates when creating a NEW test!
                    if (DoesAppointmentHaveTest(this.TestAppointmentID))
                    {
                        Console.WriteLine("Validation Error: This appointment already has a recorded result!");
                        return false;
                    }

                    if (AddNewTest())
                    {
                        Console.WriteLine("Appointment ID = " + this.TestAppointmentinfo.TestAppointmentID);

                        if (!ClsTestAppointmentBusiness.LockAppointment(this.TestAppointmentID))
                        {
                            return false;
                        }

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    // This will now successfully execute during your update test!
                    return UpdateTest();
            }

            return false;

        }

        #endregion
    }
}