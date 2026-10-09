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
    /// Represents and manages local driver license records, issue reasons, validity checks, composition objects, and persistence logic.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Manages local driver licenses, issue reasons, renewal status, expiration tracking, and persistence.", Module = "Licenses Management", Version = "1.0")]
    public class ClsLicenseBusiness
    {
        #region Enums & Modes

        /// <summary>
        /// Defines the state mode of the license business object instance.
        /// </summary>

        public enum enMode { AddNew = 0, Update = 1 };

        /// <summary>
        /// Defines the reason for driver license issuance.
        /// </summary>
        public enum enIssueReason : short
        {
            /// <summary>
            /// First time license issuance upon passing all requirements.
            /// </summary>
            FirstTime = 1,


            /// <summary>
            /// Standard license renewal after expiration.
            /// </summary>
            Renew = 2,

            /// <summary>
            /// Replacement issuance due to lost license card.
            /// </summary>
            ReplacmentLost = 3,

            /// <summary>
            /// Replacement issuance due to damaged license card.
            /// </summary>
            ReplacemnetDamged = 4

        }

        /// <summary>
        /// Gets or sets the current object mode to determine whether to insert or update during persistence.
        /// </summary>
        public enMode Mode { get; set; }


        /// <summary>
        /// Gets or sets the reason for issuing this license.
        /// </summary>
        public enIssueReason IssueReason { get; set; }
        #endregion

        #region Composition Properties

        /// <summary>
        /// Gets or sets the user domain object instance who issued/created this license record.
        /// </summary>
        public ClsUserBuiness Userinfo { get; set; }

        /// <summary>
        /// Gets or sets the underlying base application domain object instance.
        /// </summary>
        public ClsApplicationBusiness Appinfo { get; set; }

        /// <summary>
        /// Gets or sets the driver domain object instance who owns this license.
        /// </summary>
        public ClsDriverBusiness Driverinfo { get; set; }

        /// <summary>
        /// Gets or sets the license class domain object instance defining the terms of this license.
        /// </summary>
        public ClsLicensClassBusiness Classinfo { get; set; }
        #endregion

        #region Scalar Properties

        /// <summary>
        /// Gets or sets the unique identifier for the license.
        /// </summary>
        public int LicenseID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the associated application.
        /// </summary>
        public int ApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the driver.
        /// </summary>
        public int DriverID { get; set; }

        /// <summary>
        /// Gets or sets the license class identifier specifying driver authorization scope.
        /// </summary>
        public int LicenseClassID { get; set; }

        /// <summary>
        /// Gets or sets the issuance date and time of the license.
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date and time of the license.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets additional notes or comments recorded during license issuance.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the total paid fees for license issuance or processing.
        /// </summary>
        public decimal PaidFees { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the license is currently active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the User ID of the system user who created this license record.
        /// </summary>
        public int CreatedByUserID { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of <see cref="ClsLicenseBusiness"/> in AddNew mode with default initial values.
        /// </summary>
        public ClsLicenseBusiness()
        {
            this.LicenseID = -1;

            this.ApplicationID = -1;

            this.DriverID = -1;

            this.LicenseClassID = -1;

            this.IssueDate = DateTime.Now;

            this.ExpirationDate = DateTime.Now;

            this.Notes = "";

            this.PaidFees = -1;

            this.IsActive = false;

            this.IssueReason = enIssueReason.FirstTime;

            this.CreatedByUserID = -1;

            this.Mode = enMode.AddNew;
        }

        /// <summary>
        /// Private parameterized constructor used to hydrate an existing license domain object instance from DAL data (Update Mode).
        /// </summary>
        /// <param name="LicenseID">The unique license identifier.</param>
        /// <param name="ApplicationID">The associated application identifier.</param>
        /// <param name="DriverID">The associated driver identifier.</param>
        /// <param name="LicenseClass">The license class identifier.</param>
        /// <param name="IssueDate">The date and time of issuance.</param>
        /// <param name="ExpirationDate">The date and time of expiration.</param>
        /// <param name="Notes">Notes or comments regarding issuance.</param>
        /// <param name="PaidFees">Paid fee amount.</param>
        /// <param name="IsActive">Indicates whether the license is currently active.</param>
        /// <param name="IssueReason">Numerical representation of the issuance reason.</param>
        /// <param name="CreatedByUserID">The User ID of the creator.</param>
        private ClsLicenseBusiness(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees,
           bool IsActive, short IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID;

            this.ApplicationID = ApplicationID;

            this.DriverID = DriverID;

            this.LicenseClassID = LicenseClass;

            this.IssueDate = IssueDate;

            this.ExpirationDate = ExpirationDate;

            this.Notes = Notes;

            this.PaidFees = PaidFees;

            this.IsActive = IsActive;

            this.IssueReason = (enIssueReason)IssueReason;

            this.CreatedByUserID = CreatedByUserID;

            this.Appinfo = ClsApplicationBusiness.FindApplicationByID(ApplicationID);

            this.Driverinfo = ClsDriverBusiness.FindDriverByID(DriverID);

            this.Userinfo = ClsUserBuiness.FindUserByID(CreatedByUserID);

            this.Classinfo = ClsLicensClassBusiness.FindLicenseClassByID(LicenseClass);

            this.Mode = enMode.Update;
        }
        #endregion

        #region Private CRUD Helpers
        /// <summary>
        /// Calls DAL insertion logic to add a new license record and updates internal LicenseID state.
        /// </summary>
        /// <returns></returns>
        private bool AddNewLicense()
        {
            this.LicenseID = clsLicenseData.InsertNewLicense(this.ApplicationID, this.DriverID, this.LicenseClassID,
                this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (short)this.IssueReason, this.CreatedByUserID);
            return (this.LicenseID > 0);
        }

        /// <summary>
        /// Calls DAL update logic to persist modifications on an existing license record.
        /// </summary>
        /// <returns></returns>
        private bool UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClassID,
                this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (short)this.IssueReason, this.CreatedByUserID);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates a license business instance by its unique License ID.
        /// </summary>
        /// <param name="LicenseID">The unique license identifier.</param>
        /// <returns>Populated <see cref="ClsLicenseBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates a license record by LicenseID.")]
        public static ClsLicenseBusiness FindLicenseByID(int LicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int LicenseClass = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            decimal PaidFees = -1;
            bool IsActive = false;
            short IssueReason = -1;
            int CreatedByUserID = -1;

            if (clsLicenseData.FindLicenseByID(ref LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate,
                ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                ClsLicenseBusiness LicenseInfo = new ClsLicenseBusiness(LicenseID, ApplicationID, DriverID, LicenseClass
                    , IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);

                return LicenseInfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves all license records assigned to a specific driver.
        /// </summary>
        /// <param name="DriverID">The unique driver identifier.</param>
        /// <returns>A <see cref="DataTable"/> containing all licenses belonging to the specified driver.</returns>
        [DocInfo("Retrieves license history records for a specific DriverID.")]
        public static DataTable FindLicenseByDriverID(int DriverID)
        {
            return clsLicenseData.FindLicenseByDriverID(DriverID);
        }

        /// <summary>
        /// Finds and hydrates the active license business instance associated with a person for a specific license class.
        /// </summary>
        /// <param name="PersonID">The unique person identifier.</param>
        /// <param name="LicenseClassID">The unique license class identifier.</param>
        /// <returns>Populated <see cref="ClsLicenseBusiness"/> instance if an active license exists; otherwise, null.</returns>
        [DocInfo("Finds active license record for a specific person and license class combination.")]
        public static ClsLicenseBusiness FindActiveLicensByPersonandClass(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            if (clsLicenseData.FindActiveLicensByPersonandClass(ref LicenseID, PersonID, LicenseClassID))
            {
                return ClsLicenseBusiness.FindLicenseByID(LicenseID);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Retrieves the active License ID associated with a specific Application ID.
        /// </summary>
        /// <param name="ApplicationID">The unique application identifier.</param>
        /// <returns>The License ID if an active license exists for the application; otherwise, -1.</returns>
        [DocInfo("Gets active LicenseID generated by a specific ApplicationID.")]
        public static int GetActiveLicenseIDByApplicationID(int ApplicationID)
        {

            return clsLicenseData.GetActiveLicenseIDByApplicationID(ApplicationID);
        }

        /// <summary>
        /// Retrieves all license records in the system for administration grid display.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing all licenses.</returns>
        [DocInfo("Retrieves all license records for display grid.")]
        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }

        /// <summary>
        /// Deletes a license record by its unique License ID.
        /// </summary>
        /// <param name="LicenseID">The unique license identifier to delete.</param>
        /// <returns>True if deletion succeeded; otherwise, false.</returns>
        [DocInfo("Deletes a license record by LicenseID.")]
        public static bool DeleteLicense(int LicenseID)
        {
            return clsLicenseData.DeleteLicense(LicenseID);
        }

        /// <summary>
        /// Checks whether a license record exists for a specific Application ID.
        /// </summary>
        /// <param name="ApplicationID">The unique application identifier.</param>
        /// <returns>True if a license exists for the application; otherwise, false.</returns>
        [DocInfo("Checks if a license record already exists for an application.")]
        public static bool DoesLicenseExistForApplication(int ApplicationID)
        {
            return clsLicenseData.DoesLicenseExistForApplication(ApplicationID);
        }
        #endregion

        #region Aggregation & Metrics Methods

        /// <summary>
        /// Retrieves the count of currently active licenses across the system.
        /// </summary>
        /// <returns>Total active license count.</returns>
        [DocInfo("Retrieves count of active licenses in system.")]
        public static int GetActualActiveLicensesCount()
        {
            return clsLicenseData.GetActualActiveLicensesCount(); 
        }

        /// <summary>
        /// Retrieves the total count of license records created in the system.
        /// </summary>
        /// <returns>Total license count.</returns>
        [DocInfo("Retrieves total count of all licenses in system.")]
        public static int GetTotalLicensesCount()
        {
            return clsLicenseData.GetTotalLicensesCount();
        }

        /// <summary>
        /// Retrieves the count of licenses expiring within a given threshold of days.
        /// </summary>
        /// <param name="days">The threshold window in days (default: 30 days).</param>
        /// <returns>Count of licenses expiring within the specified timeframe.</returns>
        [DocInfo("Retrieves count of licenses expiring within specified days threshold.")]
        public static int GetExpiringSoonLicensesCount(int days = 30)
        {
            return clsLicenseData.GetExpiringSoonLicensesCount(days);
        }
        #endregion

        #region Business Rules & Workflows

        /// <summary>
        /// Persists changes to the license record according to the object's current state (<see cref="Mode"/>).
        /// </summary>
        /// <returns>True if save or update succeeded; otherwise, false.</returns>
        [DocInfo("Saves new license or updates existing record based on current mode.")]
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewLicense())
                    {
                        Mode = enMode.Update;

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return UpdateLicense();
            }

            return false;
        }

        /// <summary>
        /// Deactivates the current license and persists the updated status.
        /// </summary>
        /// <returns>True if deactivation save succeeded; otherwise, false.</returns>
        [DocInfo("Deactivates license status and saves change.")]
        public bool Deactivate()
        {
            this.IsActive = false;

            return this.Save();
        }

        /// <summary>
        /// Reactivates the current license and persists the updated status.
        /// </summary>
        /// <returns>True if reactivation save succeeded; otherwise, false.</returns>
        [DocInfo("Reactivates license status and saves change.")]
        public bool Reactivate()
        {
            this.IsActive = true;
            return this.Save();
        }


        /// <summary>
        /// Checks whether the license has passed its expiration date.
        /// </summary>
        /// <returns>True if current date is greater than ExpirationDate; otherwise, false.</returns>
        [DocInfo("Checks whether the license is expired based on current date.")]
        public bool IsExpired()
        {
            return DateTime.Now > this.ExpirationDate;
        }

        /// <summary>
        /// Calculates the remaining days before the license expires.
        /// </summary>
        /// <returns>Number of remaining valid days.</returns>
        [DocInfo("Calculates remaining days until license expiration.")]
        public int GetRemainingDays()
        {
            return (this.ExpirationDate - DateTime.Now).Days;
        }

        #endregion


    }
}
