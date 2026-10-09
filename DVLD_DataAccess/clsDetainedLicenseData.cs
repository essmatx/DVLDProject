using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace DVLD_DataAccess
{
    /// <summary>
    /// Provides CRUD and query operations for detained license records.
    /// </summary>
    /// <remarks>Uses stored procedures for database access and clsDataAccessSettings.Connection for the
    /// connection string. Methods return booleans, scalar values, or DataTable results as appropriate.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD and query operations for Detained Licenses.", Module = "Detained Licenses Management", Version = "1.0")]
    public class clsDetainedLicenseData
    {
        /// <summary>
        /// Retrieves Detained License details using its unique detain ID.
        /// </summary>
        /// <param name="DetainID">The unique identifier of the detained license record.</param>
        /// <param name="LicenseID">Output parameter for the associated license ID.</param>
        /// <param name="DetainDate">Output parameter for the date the license was detained.</param>
        /// <param name="FineFees">Output parameter for the fine fees.</param>
        /// <param name="CreatedByUserID">Output parameter for the user ID who created the record.</param>
        /// <param name="IsReleased">Output parameter indicating whether the license has been released.</param>
        /// <param name="ReleaseDate">Output parameter for the date the license was released.</param>
        /// <param name="ReleasedByUserID">Output parameter for the user ID who released the license.</param>
        /// <param name="ReleaseApplicationID">Output parameter for the release application ID.</param>
        /// <returns>Returns true if the detained license record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a detained license record by its detain ID.")]
        [StoredProcedure("SP_FindDetainedLicenseByDetainID")]
        public static bool FindDetainedLicenseByDetainID(ref int DetainID, ref int LicenseID, ref DateTime DetainDate,
             ref decimal FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool Isfound = false;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindDetainedLicenseByDetainID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@DetainID", DetainID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            DetainID = (reader["DetainID"] != DBNull.Value) ? (int)reader["DetainID"] : -1;
                            LicenseID = (reader["LicenseID"] != DBNull.Value) ? (int)reader["LicenseID"] : -1;
                            DetainDate = (reader["DetainDate"] != DBNull.Value) ? (DateTime)reader["DetainDate"] : DateTime.Now;
                            FineFees = (reader["FineFees"] != DBNull.Value) ? (decimal)reader["FineFees"] : -1;
                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? (int)reader["CreatedByUserID"] : -1;
                            IsReleased = (reader["IsReleased"] != DBNull.Value) ? (bool)reader["IsReleased"] : false;
                            ReleaseDate = (reader["ReleaseDate"] != DBNull.Value) ? (DateTime)reader["ReleaseDate"] : DateTime.Now;
                            ReleasedByUserID = (reader["ReleasedByUserID"] != DBNull.Value) ? (int)reader["ReleasedByUserID"] : -1;
                            ReleaseApplicationID = (reader["ReleaseApplicationID"] != DBNull.Value) ? (int)reader["ReleaseApplicationID"] : -1;
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        Isfound = false;
                    }
                }

            }

            return Isfound;

        }

        /// <summary>
        /// Retrieves Detained License details using its associated license ID.
        /// </summary>
        /// <param name="DetainID">Output parameter for the unique identifier of the detained license record.</param>
        /// <param name="LicenseID">The license ID to search for.</param>
        /// <param name="DetainDate">Output parameter for the date the license was detained.</param>
        /// <param name="FineFees">Output parameter for the fine fees.</param>
        /// <param name="CreatedByUserID">Output parameter for the user ID who created the record.</param>
        /// <param name="IsReleased">Output parameter indicating whether the license has been released.</param>
        /// <param name="ReleaseDate">Output parameter for the date the license was released.</param>
        /// <param name="ReleasedByUserID">Output parameter for the user ID who released the license.</param>
        /// <param name="ReleaseApplicationID">Output parameter for the release application ID.</param>
        /// <returns>Returns true if the detained license record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a detained license record by its license ID.")]
        [StoredProcedure("SP_FindDetainedLicenseByLicenseID")]
        public static bool FindDetainedLicenseByLicenseID(ref int DetainID, ref int LicenseID, ref DateTime DetainDate,
           ref decimal FineFees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {

                using (SqlCommand command = new SqlCommand("SP_FindDetainedLicenseByLicenseID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            DetainID = (reader["DetainID"] != DBNull.Value) ? (int)reader["DetainID"] : -1;
                            LicenseID = (reader["LicenseID"] != DBNull.Value) ? (int)reader["LicenseID"] : -1;
                            DetainDate = (reader["DetainDate"] != DBNull.Value) ? (DateTime)reader["DetainDate"] : DateTime.Now;
                            FineFees = (reader["FineFees"] != DBNull.Value) ? (decimal)reader["FineFees"] : -1;
                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? (int)reader["CreatedByUserID"] : -1;
                            IsReleased = (reader["IsReleased"] != DBNull.Value) ? (bool)reader["IsReleased"] : false;
                            ReleaseDate = (reader["ReleaseDate"] != DBNull.Value) ? (DateTime)reader["ReleaseDate"] : DateTime.Now;
                            ReleasedByUserID = (reader["ReleasedByUserID"] != DBNull.Value) ? (int)reader["ReleasedByUserID"] : -1;
                            ReleaseApplicationID = (reader["ReleaseApplicationID"] != DBNull.Value) ? (int)reader["ReleaseApplicationID"] : -1;
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        Isfound = false;
                    }
                }
            }

            return Isfound;

        }

        /// <summary>
        /// Retrieves the active detain ID for a specified license ID if it is currently detained.
        /// </summary>
        /// <param name="DetainID">Output parameter for the active detain ID.</param>
        /// <param name="LicenseID">The license ID to check.</param>
        /// <returns>Returns true if an active detain record is found; otherwise, false.</returns>
        [DocInfo("Gets an active detained license ID for a given license ID.")]
        [StoredProcedure("SP_GetActiveDetaindLicense")]
        public static bool GetActiveDetaindLicense(ref int DetainID, int LicenseID)
        {
            bool IsFound = false;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetActiveDetaindLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            DetainID = ID;

                            return IsFound = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        IsFound = false;
                    }

                }

            }

            return IsFound;
        }

        /// <summary>
        /// Inserts a new detained license record into the database.
        /// </summary>
        /// <param name="LicenseID">The ID of the license being detained.</param>
        /// <param name="DetainDate">The date and time of the detainment.</param>
        /// <param name="FineFees">The fine fees associated with the detainment.</param>
        /// <param name="CreatedByUserID">The user ID who created the detain record.</param>
        /// <param name="IsReleased">Indicates whether the license is released.</param>
        /// <param name="ReleaseDate">The date and time the license was released, if applicable.</param>
        /// <param name="ReleasedByUserID">The user ID who released the license, if applicable.</param>
        /// <param name="ReleaseApplicationID">The release application ID, if applicable.</param>
        /// <returns>Returns the newly generated DetainID if successful; otherwise, returns -1.</returns>
        [DocInfo("Inserts a new detained license record and returns its ID.")]
        [StoredProcedure("SP_InsertDetainedLicense")]
        public static int InsertDetainedLicense(int LicenseID, DateTime DetainDate,
            decimal FineFees, int CreatedByUserID, bool IsReleased, DateTime? ReleaseDate, int? ReleasedByUserID, int? ReleaseApplicationID)
        {
            int DetainID = -1;
           

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertDetainedLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    command.Parameters.AddWithValue("@DetainDate", DetainDate);
                    command.Parameters.AddWithValue("@FineFees", FineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@IsReleased", IsReleased);
                    command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate.HasValue ? (object)ReleaseDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID.HasValue ? (object)ReleasedByUserID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID.HasValue ? (object)ReleaseApplicationID.Value : DBNull.Value);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            return DetainID = ID;
                        }

                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }

            }

            return DetainID;
        }

        /// <summary>
        /// Updates an existing detained license record in the database.
        /// </summary>
        /// <param name="DetainID">The unique identifier of the detain record to update.</param>
        /// <param name="LicenseID">The updated license ID.</param>
        /// <param name="DetainDate">The updated detain date.</param>
        /// <param name="FineFees">The updated fine fees.</param>
        /// <param name="CreatedByUserID">The user ID who created the record.</param>
        /// <param name="IsReleased">The updated release status.</param>
        /// <param name="ReleaseDate">The updated release date.</param>
        /// <param name="ReleasedByUserID">The updated user ID who released it.</param>
        /// <param name="ReleaseApplicationID">The updated release application ID.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [DocInfo("Updates an existing detained license record.")]
        [StoredProcedure("SP_UpdateDetainedLicense")]
        public static bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate,
           decimal FineFees, int CreatedByUserID, bool IsReleased, DateTime? ReleaseDate, int? ReleasedByUserID, int? ReleaseApplicationID)
        {
            int RowsAffected = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateDetainedLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@DetainID", DetainID);
                    command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    command.Parameters.AddWithValue("@DetainDate", DetainDate);
                    command.Parameters.AddWithValue("@FineFees", FineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@IsReleased", IsReleased);
                    command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate.HasValue ? (object)ReleaseDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID.HasValue ? (object)ReleasedByUserID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID.HasValue ? (object)ReleaseApplicationID.Value : DBNull.Value);


                    try
                    {
                        connection.Open();

                        RowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                        return false;
                    }
                  
                }


            }

            return (RowsAffected > 0);
        }

        /// <summary>
        /// Checks whether a specific license is currently detained.
        /// </summary>
        /// <param name="LicenseID">The license ID to check.</param>
        /// <returns>Returns true if the license is currently detained; otherwise, false.</returns>
        [DocInfo("Checks if a license is currently detained.")]
        [StoredProcedure("SP_IsLicenseDetained")]
        public static bool IsLicenseDetaind(int LicenseID)
        {
            int DetainID = -1;
            return GetActiveDetaindLicense(ref DetainID, LicenseID);
        }

        /// <summary>
        /// Updates a detained license record to mark it as released.
        /// </summary>
        /// <param name="DetainID">The detain ID to release.</param>
        /// <param name="ReleaseDate">The date and time of release.</param>
        /// <param name="ReleasedByUserID">The user ID who performed the release.</param>
        /// <param name="ReleaseApplicationID">The application ID associated with the release.</param>
        /// <returns>Returns true if the release operation was successful; otherwise, false.</returns>
        [DocInfo("Releases a detained license.")]
        [StoredProcedure("SP_ReleaseDetain")]
        public static bool ReleaseDetain(int DetainID, DateTime? ReleaseDate, int? ReleasedByUserID, int? ReleaseApplicationID)
        {
            int rowsAffected = -1;
           

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {

                using (SqlCommand command = new SqlCommand("SP_ReleaseDetain", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@DetainID", DetainID);
                    command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate.HasValue ? (object)ReleaseDate.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID.HasValue ? (object)ReleasedByUserID.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID.HasValue ? (object)ReleaseApplicationID.Value : DBNull.Value);

                    try
                    {
                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");

                        return false;
                    }
                }
            }

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Retrieves all detained license records for a specific license ID.
        /// </summary>
        /// <param name="LicenseID">The license ID to filter by.</param>
        /// <returns>A DataTable containing the detained license records.</returns>
        [DocInfo("Retrieves all detained license records for a specific license ID.")]
        [StoredProcedure("SP_GetDetainedLicensesByLicenseID")]
        public static DataTable GetDetainedLicensesByLicenseID(int LicenseID)
        {
            DataTable dt = new DataTable();

            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetDetainedLicensesByLicenseID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();


                        if (reader.HasRows)
                        {

                            dt.Load(reader);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                   
                }

            }

            return dt;

        }

        /// <summary>
        /// Retrieves all detained licenses currently stored in the database.
        /// </summary>
        /// <returns>A DataTable containing all detained license records.</returns>
        [DocInfo("Retrieves all detained licenses in the system.")]
        [StoredProcedure("SP_GetAllDetainedLicenses")]
        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllDetainedLicenses", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }

                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }

            }

            return dt;
        }

        /// <summary>
        /// Retrieves active detained license details by license ID from the view.
        /// </summary>
        /// <param name="LicenseID">The license ID to check.</param>
        /// <returns>Returns the active DetainID if found; otherwise, -1.</returns>
        [DocInfo("Retrieves details of an active detained license by license ID.")]
        [StoredProcedure("SP_GetActiveDetainedLicenseByLicenseID")]
        public static int GetActiveDetainedLicenseByLicenseID(int LicenseID)
        {
            int DetainID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
               
                using (SqlCommand command = new SqlCommand("SP_GetActiveDetainedLicenseByLicenseID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Result != null && int.TryParse(Result.ToString(), out int FoundID))
                        {
                            DetainID = FoundID;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                       
                    }
                }
            }

            return DetainID;
        }

        /// <summary>
        /// Deletes a detained license record from the database by its detain ID.
        /// </summary>
        /// <param name="DetainID">The unique identifier of the detain record to delete.</param>
        /// <returns>Returns true if the deletion was successful; otherwise, false.</returns>
        [DocInfo("Deletes a detained license record by its detain ID.")]
        [StoredProcedure("SP_DeleteDetainedLicense")]
        public static bool DeleteDetainedLicense(int DetainID)
        {
            bool IsDeleted = false; 

            using(SqlConnection connection =  new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteDetainedLicense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@DetainID", DetainID);

                    try
                    {
                        connection.Open();

                        int RowsAffected = command.ExecuteNonQuery();

                        if(RowsAffected > 0) 
                        {
                            IsDeleted = true; 
                        }
                    }
                    catch(Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");

                        IsDeleted = false; 
                    }
                }
            }

            return IsDeleted; 
        }

        /// <summary>
        /// Gets the total count of currently active (unreleased) detained licenses.
        /// </summary>
        /// <returns>The total number of active detained licenses.</returns>
        [DocInfo("Gets the total count of currently active detained licenses.")]
        [StoredProcedure("SP_GetActiveDetainedLicensesCount")]
        public static int GetActiveDetainedLicensesCount()
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetActiveDetainedLicensesCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int parsedCount))
                        {
                            count = parsedCount;
                        }
                    }
                    catch (Exception ex)
                    {
                        if(count == 0)
                        {
                            clsEventLogger.LogException(ex, "Data Access Error");
                        }    
                    }
                }
            }

            return count;
        }

   
    }
}
