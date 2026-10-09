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
    /// Provides Create, Read, Update, and Delete operations for country records using database stored procedures.
    /// </summary>
    /// <remarks>Methods use ADO.NET with the connection string from clsDataAccessSettings.Connection and log
    /// exceptions via clsEventLogger. Most members are static and return primitives or DataTable results.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD operations for Countries.", Module = "Country Management", Version = "1.0")]
    public class clsCountryData
    {
        /// <summary>
        /// Retrieves Country details using its unique name.
        /// </summary>
        /// <param name="CountryID">Output parameter for the unique identifier of the country.</param>
        /// <param name="CountryName">The name of the country to search for.</param>
        /// <returns>Returns true if the country was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a country record by its name.")]
        [StoredProcedure("SP_FindCountry")]
        static public bool FindCountry(ref int CountryID, ref string CountryName)
        {
            bool Isfound = false;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindCountry", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@CountryName", CountryName);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;
                            CountryID = (reader["CountryID"] != DBNull.Value) ? (int)reader["CountryID"] : -1;
                            CountryName = (reader["CountryName"] != DBNull.Value) ? (string)reader["CountryName"] : "";
                        }
                        else
                        {
                            return false;
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                   
                }

            }

            return Isfound;
        }

        /// <summary>
        /// Retrieves Country details using its unique database ID.
        /// </summary>
        /// <param name="CountryID">The unique identifier of the country to find.</param>
        /// <param name="CountryName">Output parameter for the name of the country.</param>
        /// <returns>Returns true if the country was found successfully; otherwise, false.</returns>
        [DocInfo("Finds a country record by its ID.")]
        [StoredProcedure("SP_FindCountryByID")]
        static public bool FindCountryByID(ref int CountryID, ref string CountryName)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_FindCountryByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@CountryID", CountryID);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;
                            CountryID = (reader["CountryID"] != DBNull.Value) ? (int)reader["CountryID"] : -1;
                            CountryName = (reader["CountryName"] != DBNull.Value) ? (string)reader["CountryName"] : "";
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
                 
                }

            }



            
            return Isfound;
        }

        /// <summary>
        /// Updates an existing country's details in the database.
        /// </summary>
        /// <param name="CountryID">The unique identifier of the country to update.</param>
        /// <param name="CountryName">The updated name of the country.</param>
        /// <returns>Returns true if the update was successful; otherwise, false.</returns>
        [DocInfo("Updates an existing country record.")]
        [StoredProcedure("SP_UpdateCountry")]
        static public bool UpdateCountry(int CountryID, string CountryName)
        {
            bool Iseffected = false;

            int rows = 0;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateCountry", connection))
                {

                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@CountryID", CountryID);
                    command.Parameters.AddWithValue("@CountryName", (object)CountryName ?? DBNull.Value);

                    try
                    {
                        connection.Open();

                        rows = command.ExecuteNonQuery();

                        Iseffected = (rows > 0);
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                   
                }

            }

            return Iseffected;
        }

        /// <summary>
        /// Inserts a new country record into the database.
        /// </summary>
        /// <param name="CountryName">The name of the country to insert.</param>
        /// <returns>Returns the newly generated CountryID if successful; otherwise, returns -1.</returns>
        [DocInfo("Inserts a new country record and returns the generated ID.")]
        [StoredProcedure("SP_InsertNewCountry")]
        static public int InsertNewCountry(string CountryName)
        {
            int CountryID = -1;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertNewCountry", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@CountryName", (object)CountryName ?? DBNull.Value);

                    try
                    {
                        connection.Open();
                        object row = command.ExecuteScalar();

                        if (row != null && int.TryParse(row.ToString(), out int inserted))
                        {
                            CountryID = inserted;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }
                   
                }

            }

            return CountryID;
        }

        /// <summary>
        /// Deletes a country record from the database.
        /// </summary>
        /// <param name="CountryID">The unique identifier of the country to delete.</param>
        /// <returns>Returns true if the deletion was successful; otherwise, false.</returns>
        [DocInfo("Deletes a country by its ID.")]
        [StoredProcedure("SP_DeleteCountry")]
        public static bool DeleteCountry(int CountryID)
        {

            bool Isdeleted = false;
            int rows = 0;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteCountry", connection))
                {

                    command.CommandType = CommandType.StoredProcedure; 
                    command.Parameters.AddWithValue("@CountryID", CountryID);

                    try
                    {
                        connection.Open();

                        rows = command.ExecuteNonQuery();

                        Isdeleted = (rows > 0);
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");
                    }

                }
            }

            return Isdeleted;


        }

        /// <summary>
        /// Retrieves all countries currently stored in the system.
        /// </summary>
        /// <returns>A DataTable containing all country records.</returns>
        [DocInfo("Retrieves a complete list of all countries.")]
        [StoredProcedure("SP_GetAllCountries")]
        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllCountries", connection))
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
