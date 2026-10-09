using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Business;
using static DVLD_Shared.Attributes.clsDocAttributes;


namespace DVLD_BusinessWorkflows
{
    [ArchitectureLayer(enArchLayer.DVLD_BusinessWorkflows)]
    [DocInfo("License Management", Version = "1.0")]
    internal class ClsDetainedLicenseWorkflow
    {
        /// <summary>
        /// Detains a license by creating and persisting a detained-license record and returns the created detain
        /// identifier.
        /// </summary>
        /// <remarks>Creates a ClsDetainedLicenseBusiness record with DetainDate set to the current time,
        /// FineFees and CreatedByUserID assigned from parameters, IsReleased set to false, and attempts to save the
        /// record.</remarks>
        /// <param name="licenseID">Identifier of the license to detain.</param>
        /// <param name="fineFees">Fine amount to apply to the detained license.</param>
        /// <param name="createdByUserID">Identifier of the user performing the detainment.</param>
        /// <returns>Detain record identifier on success; -1 if the license does not exist; -2 if the license is already
        /// detained; -3 if saving the detain record fails.</returns>
        public static int DetainLicense(int licenseID,decimal fineFees,int createdByUserID)
        {
            ClsLicenseBusiness license = ClsLicenseBusiness.FindLicenseByID(licenseID); 

            if(license == null)
            {
                return -1; 
            }

            if(ClsDetainedLicenseBusiness.IsLicenseDetaind(licenseID))
            {
                return -2; 
            }

            ClsDetainedLicenseBusiness detain = new ClsDetainedLicenseBusiness();
            detain.LicenseID = licenseID;
            detain.DetainDate = DateTime.Now;
            detain.FineFees = fineFees;
            detain.CreatedByUserID = createdByUserID;
            detain.IsReleased = false;

            if(!detain.Save())
            {
                return -3; 
            }

            return detain.DetainID; 
        }

        /// <summary>
        /// Releases a detained license identified by detainID if it exists and is not already released.
        /// </summary>
        /// <remarks>Returns false if the detained license is not found or already released; otherwise
        /// invokes ReleaseDetain on the detained license.</remarks>
        /// <param name="detainID">Identifier of the detained license to release.</param>
        /// <param name="releasedByUserID">Identifier of the user performing the release.</param>
        /// <param name="releaseApplicationID">Identifier of the application initiating the release.</param>
        /// <returns>True if the license was released; otherwise, false.</returns>
        public static bool ReleaseLicense(int detainID,int releasedByUserID,int releaseApplicationID)
        {
            ClsDetainedLicenseBusiness detainedLicens = ClsDetainedLicenseBusiness.FindDetainedLicenseByDetainID(detainID); 

            if(detainedLicens == null || detainedLicens.IsReleased)
            {
                return false ; 
            }

            return detainedLicens.ReleaseDetain(releasedByUserID, releaseApplicationID);
        }
    }
}
