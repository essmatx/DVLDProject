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
    /// Coordinates the license renewal workflow by validating and deactivating the existing license, creating a renewed
    /// license record with class defaults, persisting the new license, and returning the new license identifier or a
    /// negative error code.
    /// </summary>
    /// <remarks>Performs these steps: verifies the existing license is present and active; looks up the
    /// license class; deactivates the old license; creates a new license with IssueDate = DateTime.Now, ExpirationDate
    /// = DateTime.Now.AddYears(licenseClass.DefaultValidityLingth), PaidFees from the license class, IssueReason =
    /// Renew, and the provided application and creator information; saves the new license. Returns negative codes for
    /// failures: -1 = old license not found or inactive, -2 = license class not found, -3 = failed to deactivate old
    /// license, -4 = failed to save new license (attempts to reactivate the old license on failure).</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_BusinessWorkflows)]
    [DocInfo(" License Renewal Management", Version = "1.0")]
    public partial class ClsLicenseRenewalWorkflow
    {
        /// <summary>
        /// Renews an active license by deactivating the existing license and creating a new license record with updated
        /// issue and expiration dates.
        /// </summary>
        /// <remarks>Deactivates the old license, creates a new license populated from the old license and
        /// license class (IssueDate = now, ExpirationDate = now plus the class default validity, PaidFees from class,
        /// IssueReason = Renew), and attempts to reactivate the old license if saving the new license fails.</remarks>
        /// <param name="oldLicenseID">Identifier of the existing license to renew.</param>
        /// <param name="applicationID">Identifier of the application associated with the new license.</param>
        /// <param name="notes">Optional notes to store on the new license.</param>
        /// <param name="createdByUserID">Identifier of the user creating the renewed license.</param>
        /// <returns>New license ID on success; negative values indicate errors: -1 = old license not found or inactive; -2 =
        /// license class not found; -3 = failed to deactivate old license; -4 = failed to save new license (old license
        /// reactivated).</returns>
        public static int RenewLicense(int oldLicenseID,int applicationID,string notes,int createdByUserID)
        {
            ClsLicenseBusiness oldLicense = ClsLicenseBusiness.FindLicenseByID(oldLicenseID); 

            if (oldLicense == null || !oldLicense.IsActive)
            {
                return -1; 
            }

            ClsLicensClassBusiness licenseClass = ClsLicensClassBusiness.FindLicenseClassByID(oldLicense.LicenseClassID); 


            if(licenseClass == null)
            {
                return -2; 
            }

            if(!oldLicense.Deactivate())
            {
                return -3; 
            }


            ClsLicenseBusiness newLicense = new ClsLicenseBusiness();
            newLicense.ApplicationID = applicationID;
            newLicense.DriverID = oldLicense.DriverID;
            newLicense.LicenseClassID = oldLicense.LicenseClassID;
            newLicense.IssueDate = DateTime.Now;
            newLicense.ExpirationDate = DateTime.Now.AddYears(licenseClass.DefaultValidityLingth);
            newLicense.Notes = notes ?? "";
            newLicense.PaidFees = licenseClass.ClassFees;
            newLicense.IsActive = true;
            newLicense.IssueReason = ClsLicenseBusiness.enIssueReason.Renew;
            newLicense.CreatedByUserID = createdByUserID;


            if(!newLicense.Save())
            {
               oldLicense.Reactivate();

                return -4; 
            }

            return newLicense.LicenseID; 
        }
    }
}
