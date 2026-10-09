using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Business;
using static DVLD_Shared.Attributes.clsDocAttributes;


namespace DVLD_BusinessWorkflows
{
    /// <summary>
    /// Orchestrates the workflow for replacing a driver's license, including creating the replacement application and
    /// license, applying fees, and deactivating the old license.
    /// </summary>
    /// <remarks>Determines the application type from the issue reason and retrieves application fees. Creates
    /// and saves a new application with the appropriate fees, creates and saves a new license with IssueDate,
    /// ExpirationDate based on the license class default validity length, IssueReason, and CreatedByUserID, then
    /// deactivates the old license. Returns the newly created license or null if required records are missing or any
    /// save operation fails.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_BusinessWorkflows)]
    [DocInfo(" License Replacement Management", Version = "1.0")]
    public class ClsLicenseReplacementWorkflow
    {
        /// <summary>
        /// Creates a replacement license and associated application for an existing license, then deactivates the old
        /// license.
        /// </summary>
        /// <remarks>Creates a completed application with the appropriate application type and fees,
        /// creates a new active license whose expiration is based on the old license class's default validity length,
        /// and deactivates the old license.</remarks>
        /// <param name="OldLicenseID">Identifier of the existing license to replace.</param>
        /// <param name="IssueReason">Reason for issuing the replacement (ClsLicenseBusiness.enIssueReason).</param>
        /// <param name="CreatedByUserID">Identifier of the user performing the creation.</param>
        /// <returns>The newly created ClsLicenseBusiness instance, or null if the operation fails.</returns>
        public static ClsLicenseBusiness ReplaceLicense(int OldLicenseID, ClsLicenseBusiness.enIssueReason IssueReason, int CreatedByUserID)
        {
            // 1. Get Old License
            ClsLicenseBusiness OldLicense = ClsLicenseBusiness.FindLicenseByID(OldLicenseID);
            if (OldLicense == null) return null;

            // 2. Determine Application Type
            int ApplicationTypeID = (IssueReason == ClsLicenseBusiness.enIssueReason.ReplacemnetDamged) ? 4 : 3;

            // 3. Get Application Type Info to get Fees
            ClsApplicationTypeBusiness AppType = ClsApplicationTypeBusiness.FindApplicationTypeByID(ApplicationTypeID);
            if (AppType == null) return null;

            // 4. Create new Application
            ClsApplicationBusiness NewApplication = new ClsApplicationBusiness();
            NewApplication.ApplicationPersonID = OldLicense.Driverinfo.PersonID;
            NewApplication.ApplicationDate = DateTime.Now;
            NewApplication.ApplicationTypeID = ApplicationTypeID;
            NewApplication.AppStatus = ClsApplicationBusiness.enApplicationStatus.Completed;
            NewApplication.LastStatus = DateTime.Now;
            NewApplication.PaidFees = AppType.ApplicationFees;
            NewApplication.CreatedByUserID = CreatedByUserID;

            if (!NewApplication.Save()) return null;

            // 5. Create new License
            ClsLicenseBusiness NewLicense = new ClsLicenseBusiness();
            NewLicense.ApplicationID = NewApplication.ApplicationID;
            NewLicense.DriverID = OldLicense.DriverID;
            NewLicense.LicenseClassID = OldLicense.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(OldLicense.Classinfo.DefaultValidityLingth);
            NewLicense.Notes = OldLicense.Notes;
            NewLicense.PaidFees = 0; 
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save()) return null;

            // 6. Deactivate old license
            OldLicense.Deactivate();

            return NewLicense;
        }
    }
}
