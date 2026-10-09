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
    /// Represents and manages local driving license application records, coordinating underlying application metadata, license class requirements, test tracking, and workflow persistence.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Manages local driving license applications, base application coordination, test status checks, and workflow persistence.", Module = "Applications Management", Version = "1.0")]
    public class ClsLocalDrivingLicenseAppBusiness
    {
        #region Enums & Modes

        /// <summary>
        /// Defines the state mode of the local driving license application object instance.
        /// </summary>
        public enum enMode { _AddNew = 0, _Update = 1 };

        /// <summary>
        /// Gets or sets the current object mode to determine whether to insert or update during persistence.
        /// </summary>
        public enMode Mode { get; set; }
        #endregion

        #region Composition Properties

        /// <summary>
        /// Gets or sets the underlying base application domain object instance.
        /// </summary>
        public ClsApplicationBusiness AppInfo { get; set; }

        /// <summary>
        /// Gets or sets the license class domain object instance associated with this application.
        /// </summary>
        public ClsLicensClassBusiness LicenseClassInfo { get; set; }
        #endregion

        #region Scalar Properties

        /// <summary>
        /// Gets or sets the unique identifier for the local driving license application.
        /// </summary>
        public int LocalDrivingLicenseApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the underlying base application.
        /// </summary>
        public int ApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the requested license class.
        /// </summary>
        public int LicenseClassID { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ClsLocalDrivingLicenseAppBusiness"/> in AddNew mode with default initial values.
        /// </summary>
        public ClsLocalDrivingLicenseAppBusiness()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.ApplicationID = -1;
            this.LicenseClassID = -1;

            this.AppInfo = new ClsApplicationBusiness();

            this.Mode = enMode._AddNew;
        }

        /// <summary>
        /// Private parameterized constructor used to hydrate an existing local driving license application domain object instance from DAL data (Update Mode).
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The unique local driving license application identifier.</param>
        /// <param name="ApplicationID">The associated base application identifier.</param>
        /// <param name="LicenseClassID">The requested license class identifier.</param>
        private ClsLocalDrivingLicenseAppBusiness(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            this.ApplicationID = ApplicationID;

            this.LicenseClassID = LicenseClassID;

            this.LicenseClassInfo = ClsLicensClassBusiness.FindLicenseClassByID(LicenseClassID);

            this.AppInfo = ClsApplicationBusiness.FindApplicationByID(ApplicationID);

            this.Mode = enMode._Update;
        }
        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Calls DAL insertion logic to add a new local driving license application record and updates internal ID state.
        /// </summary>
        /// <returns>True if insertion succeeded; otherwise, false.</returns>
        private bool AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.InsertNewLocalDrivingLicenseApp(this.ApplicationID, this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID > 0);
        }

        /// <summary>
        /// Calls DAL update logic to persist modifications on an existing local driving license application record.
        /// </summary>
        /// <returns>True if update succeeded; otherwise, false.</returns>
        private bool UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApp(this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates a local driving license application business instance by its unique Local Driving License Application ID.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The unique local driving license application identifier.</param>
        /// <returns>Populated <see cref="ClsLocalDrivingLicenseAppBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a local driving license application record by LocalDrivingLicenseApplicationID.")]
        public static ClsLocalDrivingLicenseAppBusiness FindLocalDrivingLicenseApplicationByID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1;

            int LicenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationData.FindLocalDrivingLicenseAppByID(ref LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID))
            {
                ClsLocalDrivingLicenseAppBusiness App = new ClsLocalDrivingLicenseAppBusiness(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID);


                return App;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds and hydrates a local driving license application business instance associated with a specific base Application ID.
        /// </summary>
        /// <param name="ApplicationID">The unique base application identifier.</param>
        /// <returns>Populated <see cref="ClsLocalDrivingLicenseAppBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a local driving license application record by base ApplicationID.")]
        public static ClsLocalDrivingLicenseAppBusiness FindLocalDrivingLicenseApplicationByAppID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            int LicenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationData.FindLocalDrivingLicenseAppByAppID(ref LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID))
            {
                ClsLocalDrivingLicenseAppBusiness App = new ClsLocalDrivingLicenseAppBusiness(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID);

                return App;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all local driving license application records from the system for administration grid display.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing all local driving license applications.</returns>
        [DocInfo("Retrieves all local driving license application records for display grid.")]
        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApps();
        }

        /// <summary>
        /// Deletes a local driving license application record by its unique identifier.
        /// </summary>
        /// <param name="LocalDrivingLicenseApplicationID">The unique application identifier to delete.</param>
        /// <returns>True if deletion succeeded; otherwise, false.</returns>
        [DocInfo("Deletes a local driving license application record by LocalDrivingLicenseApplicationID.")]
        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApp(LocalDrivingLicenseApplicationID);
        }

        /// <summary>
        /// Checks whether a person already has an active application for a given license class.
        /// </summary>
        /// <param name="PersonID">The unique person identifier.</param>
        /// <param name="LicenseClassID">The unique license class identifier.</param>
        /// <returns>True if an active application exists; otherwise, false.</returns>
        [DocInfo("Checks if a person has an existing active application for the specified license class.")]
        public static bool DoesPersonHaveActiveApplication(int PersonID, int LicenseClassID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveApplication(PersonID, LicenseClassID);
        }
        #endregion

        #region Business Rules & Workflows

        /// <summary>
        /// Persists changes to the base application and local driving license application records sequentially.
        /// </summary>
        /// <returns>True if saving base application and local application succeeded; otherwise, false.</returns>
        [DocInfo("Cascades save operation to underlying base application and persists local license application state.")]
        public bool Save()
        {
            if (LicenseClassID <= 0) return false;

            switch (Mode)
            {
                case enMode._AddNew:

                    // if (this.Mode == enMode._AddNew && DoesPersonHaveActiveApplication(this.AppInfo.ApplicationPersonID, this.LicenseClassID))
                    // {
                    //     // You could log an error here or show a message box in the UI later
                    //     return false;
                    // }

                    if (!this.AppInfo.Save())
                    {
                        return false;
                    }

                    this.ApplicationID = this.AppInfo.ApplicationID;

                    if (AddNewLocalDrivingLicenseApplication())
                    {
                        Mode = enMode._Update;

                        return true;
                    }
                    else
                    {
                        return false;
                    }




                // if (this.AppInfo.ApplicationPersonID != -1)
                // {
                //     if (AddNewLocalDrivingLicenseApplication())
                //     {
                //         Mode = enMode._Update;
                //         return true;
                //     }
                //
                //     return false;
                // }
                //






                case enMode._Update:

                    if (!this.AppInfo.Save())
                        return false;

                    return UpdateLocalDrivingLicenseApplication();
            }

            return false;

        }

        /// <summary>
        /// Retrieves the total count of passed examination tests (Vision, Written, Street) for this application.
        /// </summary>
        /// <returns>The total number of passed tests (0 to 3).</returns>
        [DocInfo("Gets total count of passed tests associated with this application.")]
        public int GetPassedTestsCount()
        {
            return clsLocalDrivingLicenseApplicationData.GetPassedTestsCount(this.LocalDrivingLicenseApplicationID);
        }

        /// <summary>
        /// Cancels the underlying base application associated with this local driving license application.
        /// </summary>
        /// <returns>True if cancellation succeeded; otherwise, false.</returns>
        [DocInfo("Cancels the associated base application workflow.")]
        public void CancelApplication(int LocalDrivingLicenseApplicationID)
        {
            ClsLocalDrivingLicenseAppBusiness clsLocalDriving = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByAppID(LocalDrivingLicenseApplicationID);

            if (clsLocalDriving != null)
            {
                this.AppInfo.Cancel();
            }
            else
            {
                Console.WriteLine("Application was not found");
            }
        }

        /// <summary>
        /// Determines whether all required examination tests (3 out of 3) have been successfully passed.
        /// </summary>
        /// <returns>True if all 3 tests are passed; otherwise, false.</returns>
        [DocInfo("Validates whether applicant has passed all 3 required examinations.")]
        public bool PassedAllTests()
        {
            // If the count is exactly 3, it returns true. Otherwise, false. 👍
            return (clsLocalDrivingLicenseApplicationData.GetPassedTestsCount(this.LocalDrivingLicenseApplicationID) == 3);
        }

        #endregion

    }
}
