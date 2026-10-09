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
    /// Data Access class responsible for all database operations related to Application Types.
    /// </summary>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles operations for Application Types.", Module = "Application Management", Version = "1.0")]
    public class clsApplicationTypeData
    {
        /// <summary>
        /// Retrieves Application Type details using its unique database ID.
        /// </summary>
        /// <param name="ApplicationTypeID">The unique identifier of the application type.</param>
        /// <param name="ApplicationTypeTitle">Output parameter for the application type title.</param>
        /// <param name="ApplicationFees">Output parameter for the application fees.</param>
        /// <returns>Returns true if the application type was found successfully; otherwise, false.</returns>
        [DocInfo("Finds an application type record by its ID.")]
        [StoredProcedure("SP_FindApplicationTypeByID")]
        public static bool FindApplicationTypeByID(ref int ApplicationTypeID, ref string ApplicationTypeTitle, ref decimal ApplicationFees)
        {
            bool Isfound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindApplicationTypeByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            ApplicationTypeID = (reader["ApplicationTypeID"] != DBNull.Value) ? (int)reader["ApplicationTypeID"] : -1;

                            ApplicationTypeTitle = (reader["ApplicationTypeTitle"] != DBNull.Value) ? (string)reader["ApplicationTypeTitle"] : "";

                            ApplicationFees = (reader["ApplicationFees"] != DBNull.Value) ? Convert.ToDecimal(reader["ApplicationFees"]) : -1;
                        }
                        else
                        {
                            return false;
                        }
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
        /// Retrieves Application Type details using its unique title.
        /// </summary>
        /// <param name="ApplicationTypeID">Output parameter for the unique identifier of the application type.</param>
        /// <param name="ApplicationTypeTitle">The title of the application type to search for.</param>
        /// <param name="ApplicationFees">Output parameter for the application fees.</param>
        /// <returns>Returns true if the application type was found successfully; otherwise, false.</returns>
        [DocInfo("Finds an application type record by its title.")]
        [StoredProcedure("SP_FindApplicationTypeByTitle")]
        public static bool FindApplicationTypeByTitle(ref int ApplicationTypeID, ref string ApplicationTypeTitle, ref decimal ApplicationFees)
        {
            bool Isfound = false;
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindApplicationTypeByTitle", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;

                            ApplicationTypeID = (reader["ApplicationTypeID"] != DBNull.Value) ? (int)reader["ApplicationTypeID"] : -1;

                            ApplicationTypeTitle = (reader["ApplicationTypeTitle"] != DBNull.Value) ? (string)reader["ApplicationTypeTitle"] : "";

                            ApplicationFees = (reader["ApplicationFees"] != DBNull.Value) ? Convert.ToDecimal(reader["ApplicationFees"]) : -1;
                        }
                        else
                        {
                            return false;
                        }
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
        /// Updates an existing application type's details in the database.
        /// </summary>
        /// <param name="ApplicationTypeID">The unique identifier of the application type to update.</param>
        /// <param name="ApplicationTypeTitle">The updated title of the application type.</param>
        /// <param name="ApplicationFees">The updated fees for the application type.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [DocInfo("Updates an existing application type record.")]
        [StoredProcedure("SP_UpdateApplicationType")]
        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        {
            int rows = -1;  
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateApplicationType", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    command.Parameters.AddWithValue("@ApplicationTypeTitle", (object)ApplicationTypeTitle ?? DBNull.Value);

                    command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

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
        /// Retrieves all application types currently stored in the system.
        /// </summary>
        /// <returns>A DataTable containing all application type records.</returns>
        [DocInfo("Retrieves a complete list of all application types.")]
        [StoredProcedure("SP_GetAllApplicationTypes")]
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllApplicationTypes", connection))
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

    }
}
