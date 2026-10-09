using DVLD_Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;


namespace DVLD_DataAccess
{
    /// <summary>
    /// Provides CRUD and query operations for driver records using stored procedures.
    /// </summary>
    /// <remarks>All members are static and use ADO.NET to execute stored procedures. Methods return
    /// primitives or a DataTable and log errors to the console while returning default values on failure. Callers must
    /// validate inputs and configure the connection string via clsDataAccessSettings.Connection.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD and query operations for Drivers.", Module = "Drivers Management", Version = "1.0")]
    public class clsDriverData
    {
        /// <summary>
        /// Retrieves Driver details using its unique driver ID.
        /// </summary>
        /// <param name="DriverID">The unique identifier of the driver record.</param>
        /// <param name="PersonID">Output parameter for the associated person ID.</param>
        /// <param name="CreatedByUserID">Output parameter for the user ID who created the record.</param>
        /// <param name="CreatedDate">Output parameter for the date the driver record was created.</param>
        /// <returns>Returns true if the driver record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a driver record by its driver ID.")]
        [StoredProcedure("SP_FindDriverByID")]
        public static bool FindDriverByID(ref int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindDriverByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            IsFound = true;
                            PersonID = (reader["PersonID"] != DBNull.Value) ? (int)reader["PersonID"] : -1;
                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? (int)reader["CreatedByUserID"] : -1;
                            CreatedDate = (reader["CreatedDate"] != DBNull.Value) ? (DateTime)reader["CreatedDate"] : DateTime.Now;

                        }
                        reader.Close();
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
        /// Retrieves Driver details using its associated person ID.
        /// </summary>
        /// <param name="DriverID">Output parameter for the unique identifier of the driver record.</param>
        /// <param name="PersonID">The person ID to search for.</param>
        /// <param name="CreatedByUserID">Output parameter for the user ID who created the record.</param>
        /// <param name="CreatedDate">Output parameter for the date the driver record was created.</param>
        /// <returns>Returns true if the driver record was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a driver record by its person ID.")]
        [StoredProcedure("SP_FindDriverByPersonID")]
        public static bool FindDriverByPersonID(ref int DriverID, ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindDriverByPersonID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            IsFound = true;
                            DriverID = (reader["DriverID"] != DBNull.Value) ? (int)reader["DriverID"] : -1;
                            CreatedByUserID = (reader["CreatedByUserID"] != DBNull.Value) ? (int)reader["CreatedByUserID"] : -1;
                            CreatedDate = (reader["CreatedDate"] != DBNull.Value) ? (DateTime)reader["CreatedDate"] : DateTime.Now;

                        }
                        reader.Close();
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
        /// Inserts a new driver record into the database.
        /// </summary>
        /// <param name="PersonID">The person ID associated with the driver.</param>
        /// <param name="CreatedByUserID">The user ID who created the driver record.</param>
        /// <param name="CreatedDate">The date and time the driver record was created.</param>
        /// <returns>Returns the newly generated DriverID if successful; otherwise, returns -1.</returns>
        [DocInfo("Inserts a new driver record and returns its ID.")]
        [StoredProcedure("SP_InsertNewDriver")]
        public static int InsertNewDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            int DriverID = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertNewDriver", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

                    try
                    {
                        connection.Open();

                        object Result = command.ExecuteScalar();

                        if (Result != null && int.TryParse(Result.ToString(), out int Inserted))
                        {
                            DriverID = Inserted;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                    

                }
            }            

            return DriverID;
        }

        /// <summary>
        /// Updates an existing driver record in the database.
        /// </summary>
        /// <param name="DriverID">The unique identifier of the driver record to update.</param>
        /// <param name="PersonID">The updated person ID.</param>
        /// <param name="CreatedByUserID">The user ID who created the record.</param>
        /// <param name="CreatedDate">The updated creation date.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [DocInfo("Updates an existing driver record.")]
        [StoredProcedure("SP_UpdateDriver")]
        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            int IsAffected = -1;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateDriver", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@DriverID", DriverID);
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    command.Parameters.AddWithValue("@CreatedDate", CreatedDate);

                    try
                    {
                        connection.Open();

                        IsAffected = command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                    

                }
            }

            return (IsAffected > 0);
        }

        /// <summary>
        /// Deletes a driver record from the database by its driver ID.
        /// </summary>
        /// <param name="DriverID">The unique identifier of the driver record to delete.</param>
        /// <returns>Returns true if the deletion was successful; otherwise, false.</returns>
        [DocInfo("Deletes a driver record by its driver ID.")]
        [StoredProcedure("SP_DeleteDriver")]
        public static bool DeleteDriver(int DriverID)
        {
            int rows = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteDriver", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@DriverID", DriverID);

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
        /// Retrieves all driver records from the Drivers view.
        /// </summary>
        /// <returns>A DataTable containing all driver records.</returns>
        [DocInfo("Retrieves all drivers from the view.")]
        [StoredProcedure("SP_GetAllDrivers")]
        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllDrivers", connection))
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
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                }
            }

            return dt;
        }
    }
}
