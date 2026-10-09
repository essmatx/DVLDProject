using DVLD_Shared.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;
using DVLD_Shared; 


namespace DVLD_DataAccess
{
    /// <summary>
    /// Data Access class responsible for all database operations related to the Application table.
    /// </summary>
    [ArchitectureLayer(clsDocAttributes.enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD operations for Applications.", Module = "Application Management", Version = "1.0")]
    public static class clsApplicationData
    {
        /// <summary>
        /// Retrieves an Application's details using their unique database ID.
        /// </summary>
        /// <param name="ApplicationID">The unique identifier of the Application to find.</param>
        /// <returns>Returns true if the Application was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a single Application record by ID.")]
        [StoredProcedure("SP_FindApplicationByID")]
        public static bool FindApplicationByID(ref int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref short ApplicationStatus, ref DateTime LastStatus, ref decimal PaidFees, ref int CreatedByUserID)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindApplicationByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            ApplicationID = (reader["ApplicationID"] != DBNull.Value) ? (int)reader["ApplicationID"] : -1;

                            ApplicantPersonID = (reader["ApplicantPersonID"] != DBNull.Value) ? (int)reader["ApplicantPersonID"] : -1;

                            ApplicationDate = (reader["ApplicationDate"] != DBNull.Value) ? (DateTime)reader["ApplicationDate"] : DateTime.Now;

                            ApplicationTypeID = (reader["ApplicationTypeID"] != DBNull.Value) ? (int)reader["ApplicationTypeID"] : -1;

                            ApplicationStatus = (reader["ApplicationStatus"] != DBNull.Value) ? Convert.ToInt16(reader["ApplicationStatus"]) : (short)-1;

                            LastStatus = (reader["LastStatusDate"] != DBNull.Value) ? (DateTime)reader["LastStatusDate"] : DateTime.Now;

                            PaidFees = (reader["PaidFees"] != DBNull.Value) ? Convert.ToDecimal(reader["PaidFees"]) : (decimal)-1;

                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? (int)reader["CreatedByUserID"] : (int)-1;
                        }
                        else
                        {
                            return false;
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error " + ex);
                        Isfound = false;

                        clsEventLogger.LogException(ex, "Data Access Error"); 
                    }
                }

            }

            return Isfound;
        }

        /// <summary>
        /// Retrieves all application associated with a speciic person ID.
        /// </summary>
        /// <param name="ApplicationPersonID">The unique identiier of the applicant.</param>
        /// <returns>A DataTable containing all mathcing application records.</returns>
        [DocInfo("Finds all applications belonging to a specific person.")]
        [StoredProcedure("SP_FindApplicationByPersonID")]
        public static DataTable FindApplicationByPersonID(int ApplicationPersonID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {

                using (SqlCommand command = new SqlCommand("SP_FindApplicationByPersonID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicationPersonID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dt.Load(reader);
                            }
                        }
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
        /// Checks if a person has a specific active application type.
        /// </summary>
        /// <param name="ApplicantPersonID">The unique ID of the applicant.</param>
        /// <param name="ApplicationTypeID">The ID of the application type to check.</param>
        /// <returns>Returns true if an active application exists; otherwise,false.</returns>
        [DocInfo("Checks if a person already has a specific active application.")]
        [StoredProcedure("SP_FindApplicationByPersonIDAndApplicationTypeID")]
        public static bool FindApplicationByPersonIDAndApplicationTypeID(int @ApplicantPersonID, int ApplicationTypeID)
        {
            bool Isfound = false;
 
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindApplicationByPersonIDAndApplicationTypeID", connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                    finally
                    {
                        connection.Close();
                    }

                }

            }
            return Isfound;
        }

        /// <summary>
        /// Inserts a new application record into the database.
        /// </summary>
        /// <param name="ApplicantPersonID">The unique ID of the applicant.</param>
        /// <param name="ApplicationDate">The date the application was submitted.</param>
        /// <param name="ApplicationTypeID">The ID representing the type of application.</param>
        /// <param name="ApplicationStatus">The current status of the application (1=New, 2=Cancelled, 3=Completed).</param>
        /// <param name="LastStatusDate">The date the status was last updated.</param>
        /// <param name="PaidFees">The fees paid for the application.</param>
        /// <param name="CreatedByUserID">The ID of the system user who created the application.</param>
        /// <returns>Returns the newly generated ApplicationID if successful; otherwise, returns -1.</returns>
        [DocInfo("Inserts a new application and returns the generated ID.")]
        [StoredProcedure("SP_InsertNewApplication")]
        public static int InsertNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, short ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            int ApplicationID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {

                using (SqlCommand command = new SqlCommand("SP_InsertNewApplication", connection))
                {

                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();

                        object results = command.ExecuteScalar();

                        if (results != null && int.TryParse(results.ToString(), out int rows))
                        {
                            ApplicationID = rows;
                        }

                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");


                    }

                }

            }
            return ApplicationID;
        }

        /// <summary>
        /// Updates an existing application's details in the database.
        /// </summary>
        /// <param name="ApplicationID">The ID of the application to update.</param>
        /// <param name="ApplicantPersonID">The updated applicant ID.</param>
        /// <param name="ApplicationDate">The updated application date.</param>
        /// <param name="ApplicationTypeID">The updated application type.</param>
        /// <param name="ApplicationStatus">The updated application status.</param>
        /// <param name="LastStatusDate">The updated last status date.</param>
        /// <param name="PaidFees">The updated paid fees.</param>
        /// <param name="CreatedByUserID">The updated user ID who modified it.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [DocInfo("Updates an existing application record.")]
        [StoredProcedure("SP_UpdateApplication")]
        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, short ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            int rows = -1;
            
            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateApplication", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                    command.Parameters.AddWithValue("@PaidFees", PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    try
                    {
                        connection.Open();

                        rows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }

                }

            }
            return (rows > 0);
        }

        /// <summary>
        /// Deletes an application record from the database.
        /// </summary>
        /// <param name="ApplicationID">The unique identifier of the application to delete.</param>
        /// <returns>Returns true if the deletion was successful; otherwise, false.</returns>
        [DocInfo("Deletes an application by its ID.")]
        [StoredProcedure("SP_DeleteApplication")]
        public static bool DeleteApplication(int ApplicationID)
        {
            int rows = -1;

            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteApplication", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    try
                    {
                        connection.Open();

                        rows = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }

            }
            return (rows > 0);
        }

        /// <summary>
        /// Retrieves all applications currently stored in the system.
        /// </summary>
        /// <returns>A DataTable containing all application records.</returns>
        [DocInfo("Retrieves a complete list of all applications.")]
        [StoredProcedure("SP_GetAllApplications")]
        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllApplications", connection))
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
        /// Retrieves the total number of applications processed today, alongside the total number of applications created today.
        /// </summary>
        /// <param name="processedCount">Output parameter for today's completed applications.</param>
        /// <param name="totalCount">Output parameter for today's total applications.</param>
        /// <returns>Returns true if the query executed successfully; otherwise, false.</returns>
        [DocInfo("Gets daily application statistics (Processed vs Total).")]
        [StoredProcedure("SP_GetDailyApplicationShortStats")]
        public static bool GetDailyApplicationShortStats(ref int processedCount, ref int totalCount)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetDailyApplicationShortStats", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                processedCount = reader["ProcessedToday"] != DBNull.Value ? Convert.ToInt32(reader["ProcessedToday"]) : 0;
                                totalCount = reader["TotalToday"] != DBNull.Value ? Convert.ToInt32(reader["TotalToday"]) : 0;
                                isFound = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }
            }
            return isFound;
        }

        /// <summary>
        /// Gets the total count of applications that are currently pending for a specific application type.
        /// </summary>
        /// <param name="applicationTypeID">The ID of the application type to check.</param>
        /// <returns>The number of pending applications.</returns>
        [DocInfo("Counts pending applications filtered by Application Type.")]
        [StoredProcedure("SP_GetPendingCountByApplicationType")]
        public static int GetPendingCountByApplicationType(int applicationTypeID)
        {
            int pendingCount = 0; 

            using(SqlConnection connection =  new SqlConnection(clsDataAccessSettings.Connection))
            {
                using(SqlCommand command = new SqlCommand("SP_GetPendingCountByApplicationType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar(); 

                        if(result != null && int.TryParse(result.ToString(),out int count))
                        {
                            pendingCount = count; 
                        }
                    }
                    catch(Exception ex)
                    {
                        if (pendingCount == 0)
                        {
                            clsEventLogger.LogException(ex, "Data Access Error");
                        }
                    }
                }
            }

            return pendingCount; 
        }

        /// <summary>
        /// Gets the total number of all applications in the system.
        /// </summary>
        /// <returns>The total count of applications.</returns>
        [DocInfo("Gets the aggregate count of all applications.")]
        [StoredProcedure("SP_GetTotalApplicationsCount")]
        public static int GetTotalApplicationsCount()
        {
            int totalCount = 0; 

            using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
              
                using(SqlCommand command =  new SqlCommand("SP_GetTotalApplicationsCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar(); 

                        if(result != null && int.TryParse(result.ToString(),out int count))
                        {
                            totalCount = count; 
                        }
                    }
                    catch(Exception ex)
                    {
                       if(totalCount == 0)
                        {
                            clsEventLogger.LogException(ex, "Data Access Error");
                        }
                    }
                }
            }

            return totalCount; 
        }

        /// <summary>
        /// Gets the total number of completed applications in the system.
        /// </summary>
        /// <returns>The total count of completed applications.</returns>
        [DocInfo("Gets the aggregate count of all completed applications.")]
        [StoredProcedure("SP_GetCompletedApplicationsCount")]
        public static int GetCompletedApplicationsCount()
        {
            int completedCount = 0; 

            using(SqlConnection connection =  new SqlConnection(clsDataAccessSettings.Connection))
            {
               
                using(SqlCommand command = new SqlCommand("SP_GetCompletedApplicationsCount", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(),out int count))
                        {
                            completedCount = count; 
                        }
                    }
                    catch(Exception ex)
                    {
                        if(completedCount == 0)
                        {
                            clsEventLogger.LogException(ex, "Data Access Error");
                        }
                    }
                }
            }

            return completedCount; 
        }
    }
}
