
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using DVLD_Business;
using static DVLD_Shared.Attributes.clsDocAttributes;


namespace DVLD_BusinessWorkflows
{
    /// <summary>
    /// Handles the end-to-end workflow for first-time driving license issuance, including application validation, test
    /// verification, driver creation, license creation, persistence, and activation.
    /// </summary>
    /// <remarks>Performs precondition checks (application existence and status, test results, absence of an
    /// active license). Creates or reuses a driver record, computes issue and expiration dates, saves the license,
    /// ensures the application is completed, and returns the new license ID or negative integers indicating specific
    /// failure conditions (-1..-8).</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_BusinessWorkflows)]
    [DocInfo(" License Issuance Management", Version = "1.0")]
    public class ClsLicenseIssuanceWorkflow
    {
        /// <summary>
        /// Issue a first-time driving license for the specified local driving license application.
        /// </summary>
        /// <remarks>Validates application status and test results, ensures no existing active license for
        /// the person and class, creates or retrieves a driver record, creates and saves the license with computed
        /// issue and expiration dates, and marks the application completed.</remarks>
        /// <param name="localDrivingLicenseApplicationID">Identifier of the local driving license application to process.</param>
        /// <param name="notes">Optional notes to record on the issued license; null is treated as empty.</param>
        /// <param name="createdByUserID">Identifier of the user creating the license record.</param>
        /// <returns>On success, the newly created license ID (>0). On failure, a negative error code: -1 = application not
        /// found; -2 = application not in New status; -3 = required tests not passed; -4 = active license already
        /// exists for the person and class; -5 = failed to create or retrieve driver; -6 = license class not found; -7
        /// = failed to save license; -8 = failed to mark application as completed.</returns>
        public static int IssueLicenseForTheFirstTime( int localDrivingLicenseApplicationID, string notes ,int createdByUserID)
        {
            ClsLocalDrivingLicenseAppBusiness localApp = ClsLocalDrivingLicenseAppBusiness.FindLocalDrivingLicenseApplicationByID(localDrivingLicenseApplicationID); 

            if(localApp == null)
            {
                return -1; 
            }

            if(localApp.AppInfo.AppStatus != ClsApplicationBusiness.enApplicationStatus.New)
            {
                return -2;
            }

            if(!AreAllTestsPassed(localApp))
            {
                return -3; 
            }

            ClsLicenseBusiness existingLicense =  ClsLicenseBusiness.FindActiveLicensByPersonandClass(localApp.AppInfo.ApplicationPersonID, localApp.LicenseClassID); 

            if(existingLicense != null)
            {
                return -4; 
            }

            int DriverID = GetOrCreateDriver(localApp.AppInfo.ApplicationPersonID, localApp.AppInfo.CreatedByUserID);

            if(DriverID == -1)
            {
                return -5; 
            }

            ClsLicensClassBusiness licenseClass = ClsLicensClassBusiness.FindLicenseClassByID(localApp.LicenseClassID);

            if (licenseClass == null)
            {
                return -6; 
            }

            ClsLicenseBusiness license = new ClsLicenseBusiness();
            license.ApplicationID = localApp.ApplicationID; 
            license.DriverID = DriverID;
            license.LicenseClassID = localApp.LicenseClassID;
            license.IssueDate = DateTime.Now;
            license.ExpirationDate = DateTime.Now.AddYears(licenseClass.DefaultValidityLingth);
            license.Notes = notes ?? "";
            license.PaidFees = licenseClass.ClassFees;
            license.IsActive = true;
            license.IssueReason = ClsLicenseBusiness.enIssueReason.FirstTime;
            license.CreatedByUserID = createdByUserID;

            if (!license.Save())
            {
                return -7; 
            }

            if(!localApp.AppInfo.Completed())
            {
                return -8; 
            }

            return license.LicenseID; 
        }

        /// <summary>
        /// Determines whether the vision, written, and practical tests for the specified local driving license
        /// application have all passed.
        /// </summary>
        /// <remarks>Evaluates test results by using the application's LocalDrivingLicenseApplicationID
        /// and checking each required test type.</remarks>
        /// <param name="localApp">Local driving license application whose test results are evaluated.</param>
        /// <returns>true if all required tests (vision, written, practical) have passed; otherwise, false.</returns>
        private static bool AreAllTestsPassed(ClsLocalDrivingLicenseAppBusiness localApp)
        {
            int appID = localApp.LocalDrivingLicenseApplicationID;

            return IsTestTypePassed(appID, (int)ClsTestTypeBusiness.enTestType.Vision)
                && IsTestTypePassed(appID, (int)ClsTestTypeBusiness.enTestType.Written)
                && IsTestTypePassed(appID, (int)ClsTestTypeBusiness.enTestType.Practical);
        }

        /// <summary>
        /// Determines whether the specified test type has been passed for the given local driving license application.
        /// </summary>
        /// <remarks>Delegates to ClsTestAppointmentBusiness.DoesPassTestType.</remarks>
        /// <param name="LocalDrivingLicenseApplicationID">Local driving license application identifier.</param>
        /// <param name="TestTypeID">Test type identifier.</param>
        /// <returns>True if the specified test type has been passed for the application; otherwise, false.</returns>
        private static bool IsTestTypePassed(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return ClsTestAppointmentBusiness.DoesPassTestType(LocalDrivingLicenseApplicationID, TestTypeID); 
        }

        /// <summary>
        /// Returns the driver ID for the specified person, creating a new driver record if none exists.
        /// </summary>
        /// <remarks>When creating a new driver, the method sets CreatedDate to the current time and
        /// persists the record via ClsDriverBusiness.Save().</remarks>
        /// <param name="PersonID">Identifier of the person to find or create a driver for.</param>
        /// <param name="CreatedByUserID">Identifier of the user who will be recorded as the creator when a new driver record is created.</param>
        /// <returns>The driver ID if found or created; -1 if creation or save fails.</returns>
        private static int GetOrCreateDriver(int PersonID , int CreatedByUserID)
        {
            ClsDriverBusiness existingDriver = ClsDriverBusiness.FindDriverByPerson(PersonID); 


            if(existingDriver != null)
            {
                return existingDriver.DriverID; 
            }

            ClsDriverBusiness NewDriver = new ClsDriverBusiness();

            NewDriver.PersonID = PersonID;

            NewDriver.CreatedUserID = CreatedByUserID;

            NewDriver.CreatedDate = DateTime.Now; 

            if(NewDriver.Save())
            {
                return NewDriver.DriverID; 
            }

            return -1; 
        }

        
    }
}
