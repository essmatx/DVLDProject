using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_Business
{
    /// <summary>
    /// Represents and manages international driver license issuance, lifecycle, composition objects, and persistence logic.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_Business)]
    [DocInfo("Manages international license records, validity rules, driver links, and persistence.", Module = "Licenses Management", Version = "1.0")]
    public class ClsInternationalLicenseBusiness
    {
        #region Enums & Modes

        /// <summary>
        /// Defines the state mode of the international license business object instance.
        /// </summary>
        public enum enMode { AddNew = 0, Update = 1 };

        /// <summary>
        /// Gets or sets the current object mode to determine whether to insert or update during persistence.
        /// </summary>
        public enMode Mode { get; set; }
        #endregion

        #region Composition Properties

        /// <summary>
        /// Gets or sets the base application domain object instance associated with this international license.
        /// </summary>
        public ClsApplicationBusiness Appinfo { get; set; }

        /// <summary>
        /// Gets or sets the user domain object instance who created the international license record.
        /// </summary>
        public ClsUserBuiness Userinfo { get; set; }

        /// <summary>
        /// Gets or sets the driver domain object instance associated with this international license.
        /// </summary>
        public ClsDriverBusiness Driverinfo { get; set; }

        /// <summary>
        /// Gets or sets the local driving license application domain object instance associated with issuance.
        /// </summary>
        public ClsLocalDrivingLicenseAppBusiness LocalLicenseinfo { get; set; }
        #endregion

        #region Scalar Properties

        /// <summary>
        /// Gets or sets the unique identifier for the international license.
        /// </summary>
        public int InternationalLicenseID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the underlying application.
        /// </summary>
        public int ApplicationID { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the driver.
        /// </summary>
        public int DriverID { get; set; }

        /// <summary>
        /// Gets or sets the issuance date and time of the international license.
        /// </summary>
        public int IssuedUsingLocalLicenseID { get; set; }

        /// <summary>
        /// Gets or sets the issuance date and time of the international license.
        /// </summary>
        public DateTime IssueDate { get; set; }


        /// <summary>
        /// Gets or sets the expiration date and time of the international license.
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the international license is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the User ID of the system user who created this international license record.
        /// </summary>
        public int CreatedByUserID { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of <see cref="ClsInternationalLicenseBusiness"/> in AddNew mode with default initial values.
        /// </summary>
        public ClsInternationalLicenseBusiness()
        {
            this.InternationalLicenseID = -1;

            this.ApplicationID = -1;

            this.DriverID = -1;

            this.IssuedUsingLocalLicenseID = -1;

            this.IssueDate = DateTime.Now;

            this.ExpirationDate = DateTime.Now;

            this.IsActive = false;

            this.CreatedByUserID = -1;

            this.Mode = enMode.AddNew;
        }

        /// <summary>
        /// Private parameterized constructor used to hydrate an existing international license domain object instance from DAL data (Update Mode).
        /// </summary>
        /// <param name="InternationalLicenseID">The unique international license identifier.</param>
        /// <param name="ApplicationID">The associated application identifier.</param>
        /// <param name="DriverID">The associated driver identifier.</param>
        /// <param name="IssuedUsingLocalLicenseID">The local license identifier used for issuance.</param>
        /// <param name="IssueDate">The date and time of issuance.</param>
        /// <param name="ExpirationDate">The date and time of expiration.</param>
        /// <param name="IsActive">Indicates whether the license is currently active.</param>
        /// <param name="CreatedByUserID">The User ID of the creator.</param>
        private ClsInternationalLicenseBusiness(int InternationalLicenseID, int ApplicationID,
            int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            this.InternationalLicenseID = InternationalLicenseID;

            this.ApplicationID = ApplicationID;

            this.DriverID = DriverID;

            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;

            this.IssueDate = IssueDate;

            this.ExpirationDate = ExpirationDate;

            this.IsActive = IsActive;

            this.CreatedByUserID = CreatedByUserID;

            this.Mode = enMode.Update;

            this.Appinfo = ClsApplicationBusiness.FindApplicationByID(ApplicationID);

            this.Userinfo = ClsUserBuiness.FindUserByID(CreatedByUserID);

            this.Driverinfo = ClsDriverBusiness.FindDriverByID(DriverID);

            this.LocalLicenseinfo = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(IssuedUsingLocalLicenseID);
        }
        #endregion

        #region Private CRUD Helpers

        /// <summary>
        /// Calls DAL insertion logic to add a new international license record and updates the internal InternationalLicenseID state.
        /// </summary>
        /// <returns></returns>
        private bool AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.InsertNewInternationalLicense(this.ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);

            return (this.InternationalLicenseID > 0);
        }

        /// <summary>
        /// Calls DAL update logic to persist modifications on an existing international license record.
        /// </summary>
        /// <returns></returns>
        private bool UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
        }
        #endregion

        #region Factory & Search Methods

        /// <summary>
        /// Finds and hydrates an international license business instance by its unique International License ID.
        /// </summary>
        /// <param name="InternationalLicenseID">The unique international license identifier.</param>
        /// <returns>Populated <see cref="ClsInternationalLicenseBusiness"/> instance if found; otherwise, null.</returns>
        [DocInfo("Finds and hydrates an international license record by InternationalLicenseID.")]
        public static ClsInternationalLicenseBusiness FindInternationalLicenseByID(int InternationalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1;
            int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now;
            DateTime ExpirationDate = DateTime.Now;
            bool IsActive = false;
            int CreatedByUserID = -1;

            if (clsInternationalLicenseData.FindInternationalLicenseByID(ref InternationalLicenseID, ref ApplicationID, ref DriverID, ref IssuedUsingLocalLicenseID, ref IssueDate
                , ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                ClsInternationalLicenseBusiness InternationalLicenseinfo = new ClsInternationalLicenseBusiness(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);



                return InternationalLicenseinfo;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Finds and hydrates the active international license business instance associated with a specific Driver ID.
        /// </summary>
        /// <param name="DriverID">The unique driver identifier.</param>
        /// <returns>Populated <see cref="ClsInternationalLicenseBusiness"/> instance if an active license exists; otherwise, null.</returns>
        [DocInfo("Finds active international license record for a specific DriverID.")]
        public static ClsInternationalLicenseBusiness FindActiveInternationalLicenseByDriverID(int DriverID)
        {
            int InternationalLicense = clsInternationalLicenseData.GetActiveInternationalLicenseByDriverID(DriverID);

            if (InternationalLicense > 0)
            {
                return ClsInternationalLicenseBusiness.FindInternationalLicenseByID(InternationalLicense);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Retrieves all international license records from the system for administration grid display.
        /// </summary>
        /// <returns>A <see cref="DataTable"/> containing all international licenses.</returns>
        [DocInfo("Retrieves all international license records for display grid.")]
        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicense();
        }

        /// <summary>
        /// Deletes an international license record by its unique identifier.
        /// </summary>
        /// <param name="InternationalLicenseID">The unique international license identifier to delete.</param>
        /// <returns>True if deletion succeeded; otherwise, false.</returns>
        [DocInfo("Deletes an international license record by InternationalLicenseID.")]
        public static bool DeleteInternationalLicense(int InternationalLicenseID)
        {
            return clsInternationalLicenseData.DeleteInternationalLicense(InternationalLicenseID);
        }
        #endregion

        #region Business Rules & Workflows

        /// <summary>
        /// Checks whether the international license has passed its expiration date.
        /// </summary>
        /// <returns>True if current date is greater than ExpirationDate; otherwise, false.</returns>
        [DocInfo("Checks whether the international license is expired based on current date.")]
        public bool IsExpired()
        {
            return DateTime.Now > this.ExpirationDate;
        }

        /// <summary>
        /// Persists changes to the international license record according to the object's current state (<see cref="Mode"/>).
        /// </summary>
        /// <returns>True if save or update succeeded; otherwise, false.</returns>
        [DocInfo("Saves new international license or updates existing record based on current mode.")]
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (AddNewInternationalLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return UpdateInternationalLicense();
            }

            return false;

        }
        #endregion

    }
}
