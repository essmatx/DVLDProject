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
    /// Handles CRUD and query operations for People records.
    /// </summary>
    /// <remarks>Part of the People Management module (Version 1.0). Marked for the DVLD_DataAccess
    /// architecture layer. Contains static data-access methods that interact with stored procedures to retrieve,
    /// insert, update, delete, and query People records and related license history.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_DataAccess)]
    [DocInfo("Handles CRUD and query operations for People records.", Module = "People Management", Version = "1.0")]
    public static class clsPersonData
    {
        /// <summary>
        /// Retrieves a person's detailed record from the database using their unique <paramref name="PersonID"/>.
        /// Populates all passed reference parameters with the retrieved data.
        /// </summary>
        /// <param name="PersonID">The unique identifier of the person record to search for.</param>
        /// <param name="NationalNo">Outputs the person's national identification number.</param>
        /// <param name="FirstName">Outputs the person's first name.</param>
        /// <param name="Secondname">Outputs the person's second name (father's name).</param>
        /// <param name="ThirdName">Outputs the person's third name (grandfather's name), or an empty string if null in the database.</param>
        /// <param name="LastName">Outputs the person's last name (family name).</param>
        /// <param name="DateOfBirth">Outputs the person's birth date.</param>
        /// <param name="Gendor">Outputs the numerical gender indicator (0 for Male, 1 for Female).</param>
        /// <param name="Address">Outputs the physical residential address.</param>
        /// <param name="Phone">Outputs the primary contact telephone number.</param>
        /// <param name="Email">Outputs the email address, or an empty string if null in the database.</param>
        /// <param name="NationalityCountryID">Outputs the foreign key corresponding to the person's nationality in the <c>Countries</c> table.</param>
        /// <param name="ImagePath">Outputs the file path to the stored photo, or an empty string if no image exists.</param>
        /// <returns>
        /// <c>true</c> if a record matching the <paramref name="PersonID"/> was found and all parameters were successfully populated; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Finds a person record by unique PersonID.")]
        [StoredProcedure("SP_GetPersonInfoByID")]
        public static bool GetPersonInfoByID(ref int PersonID, ref string NationalNo, ref string FirstName, ref string Secondname, ref string ThirdName, ref string LastName,
            ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPersonInfoByID", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;
                            NationalNo = reader["NationalNo"]?.ToString() ?? "";
                            FirstName = (reader["FirstName"] != DBNull.Value) ? (string)reader["FirstName"] : "";
                            Secondname = (reader["Secondname"] != DBNull.Value) ? (string)reader["Secondname"].ToString() : "";
                            ThirdName = (reader["ThirdName"] != DBNull.Value) ? (string)reader["ThirdName"] : "";
                            LastName = (reader["LastName"] != DBNull.Value) ? (string)reader["LastName"] : "";
                            DateOfBirth = (reader["DateOfBirth"] != DBNull.Value) ? (DateTime)reader["DateOfBirth"] : DateTime.Now;
                            Gendor = (reader["Gendor"] != DBNull.Value) ? Convert.ToInt16(reader["Gendor"]) : (short)-1;
                            Address = (reader["Address"] != DBNull.Value) ? (string)reader["Address"] : "";
                            Phone = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
                            Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                            NationalityCountryID = (reader["NationalityCountryID"] != DBNull.Value) ? (int)reader["NationalityCountryID"] : (int)-1;
                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                ImagePath = (string)reader["ImagePath"];
                            }
                            else
                            {
                                ImagePath = "";
                            }
                        }
                        else
                        {
                            Isfound = false;
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
        /// Retrieves a person's detailed record from the database using their unique <paramref name="NationalNo"/>.
        /// Populates all passed reference parameters with the retrieved data.
        /// </summary>
        /// <param name="NationalNo">The unique national identification number of the person record to search for.</param>
        /// <param name="PersonID">Outputs the unique primary key identifier (<c>PersonID</c>) of the matching record.</param>
        /// <param name="FirstName">Outputs the person's first name.</param>
        /// <param name="Secondname">Outputs the person's second name (father's name).</param>
        /// <param name="ThirdName">Outputs the person's third name (grandfather's name), or an empty string if null in the database.</param>
        /// <param name="LastName">Outputs the person's last name (family name).</param>
        /// <param name="DateOfBirth">Outputs the person's birth date.</param>
        /// <param name="Gendor">Outputs the numerical gender indicator (0 for Male, 1 for Female).</param>
        /// <param name="Address">Outputs the physical residential address.</param>
        /// <param name="Phone">Outputs the primary contact telephone number.</param>
        /// <param name="Email">Outputs the email address, or an empty string if null in the database.</param>
        /// <param name="NationalityCountryID">Outputs the foreign key corresponding to the person's nationality in the <c>Countries</c> table.</param>
        /// <param name="ImagePath">Outputs the file path to the stored photo, or an empty string if no image exists.</param>
        /// <returns>
        /// <c>true</c> if a record matching the <paramref name="NationalNo"/> was found and all parameters were successfully populated; otherwise, <c>false</c>.
        /// </returns>

        [DocInfo("Finds a person record by National Number.")]
        [StoredProcedure("SP_GetPersonInfoByNationalNo")]
        public static bool GetPersonInfoByNationalNo(ref int PersonID, ref string NationalNo, ref string FirstName, ref string Secondname, ref string ThirdName, ref string LastName,
           ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool Isfound = false;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPersonInfoByNationalNo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;
                            PersonID = (reader["PersonID"] != DBNull.Value) ? (int)reader["PersonID"] : (int)-1;
                            FirstName = (reader["FirstName"] != DBNull.Value) ? (string)reader["FirstName"] : "";
                            Secondname = (reader["Secondname"] != DBNull.Value) ? (string)reader["Secondname"].ToString() : "";
                            ThirdName = (reader["ThirdName"] != DBNull.Value) ? (string)reader["ThirdName"] : "";
                            LastName = (reader["LastName"] != DBNull.Value) ? (string)reader["LastName"] : "";
                            DateOfBirth = (reader["DateOfBirth"] != DBNull.Value) ? (DateTime)reader["DateOfBirth"] : DateTime.Now;
                            Gendor = (reader["Gendor"] != DBNull.Value) ? Convert.ToInt16(reader["Gendor"]) : (short)-1;
                            Address = (reader["Address"] != DBNull.Value) ? (string)reader["Address"] : "";
                            Phone = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
                            Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                            NationalityCountryID = (reader["NationalityCountryID"] != DBNull.Value) ? (int)reader["NationalityCountryID"] : (int)-1;
                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                ImagePath = (string)reader["ImagePath"];
                            }
                            else
                            {
                                ImagePath = "";
                            }
                        }
                        else
                        {
                            Isfound = false;
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
        /// Retrieves the first person record from the database matching a given <paramref name="NationalityCountryID"/>.
        /// Populates all passed reference parameters with the retrieved record's details.
        /// </summary>
        /// <param name="PersonID">Outputs the unique primary key identifier (<c>PersonID</c>) of the matching record.</param>
        /// <param name="NationalNo">Outputs the person's national identification number, or an empty string if null.</param>
        /// <param name="FirstName">Outputs the person's first name, or an empty string if null.</param>
        /// <param name="Secondname">Outputs the person's second name (father's name), or an empty string if null.</param>
        /// <param name="ThirdName">Outputs the person's third name (grandfather's name), or an empty string if null.</param>
        /// <param name="LastName">Outputs the person's last name (family name), or an empty string if null.</param>
        /// <param name="DateOfBirth">Outputs the person's birth date, or current date/time if null.</param>
        /// <param name="Gendor">Outputs the numerical gender indicator, or -1 if null.</param>
        /// <param name="Address">Outputs the physical residential address, or an empty string if null.</param>
        /// <param name="Phone">Outputs the primary contact telephone number, or an empty string if null.</param>
        /// <param name="Email">Outputs the email address, or an empty string if null.</param>
        /// <param name="NationalityCountryID">Input filter parameter that also outputs the retrieved foreign key ID from the database record (or -1 if null).</param>
        /// <param name="ImagePath">Outputs the file path to the stored profile image, or an empty string if null.</param>
        /// <returns>
        /// <c>true</c> if at least one record matching the specified <paramref name="NationalityCountryID"/> was found and populated; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Finds the first person record matching a given Nationality Country ID.")]
        [StoredProcedure("SP_GetPersonInfoByNationality")]
        public static bool GetPersonInfoByNationality(ref int PersonID, ref string NationalNo, ref string FirstName, ref string Secondname, ref string ThirdName, ref string LastName,
            ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool Isfound = false;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPersonInfoByNationality", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;
                            PersonID = (int)reader["PersonID"];
                            NationalNo = (reader["NationalNo"] != DBNull.Value) ? (string)reader["NationalNo"] : "";
                            FirstName = (reader["FirstName"] != DBNull.Value) ? (string)reader["FirstName"] : "";
                            Secondname = (reader["Secondname"] != DBNull.Value) ? (string)reader["Secondname"] : "";
                            ThirdName = (reader["ThirdName"] != DBNull.Value) ? (string)reader["ThirdName"] : "";
                            LastName = (reader["LastName"] != DBNull.Value) ? (string)reader["LastName"] : "";
                            DateOfBirth = (reader["DateOfBirth"] != DBNull.Value) ? (DateTime)reader["DateOfBirth"] : DateTime.Now;
                            Gendor = (reader["Gendor"] != DBNull.Value) ? Convert.ToInt16(reader["Gendor"]) : (short)-1;
                            Address = (reader["Address"] != DBNull.Value) ? (string)reader["Address"] : "";
                            Phone = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
                            Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                            NationalityCountryID = (reader["NationalityCountryID"] != DBNull.Value) ? (int)reader["NationalityCountryID"] : (int)-1;
                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                ImagePath = (string)reader["ImagePath"];
                            }
                            else
                            {
                                ImagePath = "";
                            }
                        }
                        else
                        {
                            Isfound = false;
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
        /// Retrieves the first person record from the database matching the specified <paramref name="Gendor"/>.
        /// Populates all passed reference parameters with the retrieved record's details.
        /// </summary>
        /// <param name="Gendor">Input filter parameter (0 for Male, 1 for Female) that also outputs the retrieved gender value from the database record (or -1 if null).</param>
        /// <param name="PersonID">Outputs the unique primary key identifier (<c>PersonID</c>) of the matching record.</param>
        /// <param name="NationalNo">Outputs the person's national identification number, or an empty string if null.</param>
        /// <param name="FirstName">Outputs the person's first name, or an empty string if null.</param>
        /// <param name="Secondname">Outputs the person's second name (father's name), or an empty string if null.</param>
        /// <param name="ThirdName">Outputs the person's third name (grandfather's name), or an empty string if null.</param>
        /// <param name="LastName">Outputs the person's last name (family name), or an empty string if null.</param>
        /// <param name="DateOfBirth">Outputs the person's birth date, or current date/time if null.</param>
        /// <param name="Address">Outputs the physical residential address, or an empty string if null.</param>
        /// <param name="Phone">Outputs the primary contact telephone number, or an empty string if null.</param>
        /// <param name="Email">Outputs the email address, or an empty string if null.</param>
        /// <param name="NationalityCountryID">Outputs the foreign key corresponding to the person's nationality in the <c>Countries</c> table, or -1 if null.</param>
        /// <param name="ImagePath">Outputs the file path to the stored profile image, or an empty string if null.</param>
        /// <returns>
        /// <c>true</c> if at least one record matching the specified <paramref name="Gendor"/> was found and populated; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Finds the first person record matching a given gender identifier.")]
        [StoredProcedure("SP_GetPersonInfoByGender")]
        public static bool GetPersonInfoByGender(ref int PersonID, ref string NationalNo, ref string FirstName, ref string Secondname, ref string ThirdName, ref string LastName,
           ref DateTime DateOfBirth, ref short Gendor, ref string Address, ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool Isfound = false;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPersonInfoByGender", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Gendor", Gendor);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            Isfound = true;
                            PersonID = (int)reader["PersonID"];
                            NationalNo = (reader["NationalNo"] != DBNull.Value) ? (string)reader["NationalNo"] : "";
                            FirstName = (reader["FirstName"] != DBNull.Value) ? (string)reader["FirstName"] : "";
                            Secondname = (reader["Secondname"] != DBNull.Value) ? (string)reader["Secondname"] : "";
                            ThirdName = (reader["ThirdName"] != DBNull.Value) ? (string)reader["ThirdName"] : "";
                            LastName = (reader["LastName"] != DBNull.Value) ? (string)reader["LastName"] : "";
                            DateOfBirth = (reader["DateOfBirth"] != DBNull.Value) ? (DateTime)reader["DateOfBirth"] : DateTime.Now;
                            Gendor = (reader["Gendor"] != DBNull.Value) ? Convert.ToInt16(reader["Gendor"]) : (short)-1;
                            Address = (reader["Address"] != DBNull.Value) ? (string)reader["Address"] : "";
                            Phone = (reader["Phone"] != DBNull.Value) ? (string)reader["Phone"] : "";
                            Email = (reader["Email"] != DBNull.Value) ? (string)reader["Email"] : "";
                            NationalityCountryID = (reader["NationalityCountryID"] != DBNull.Value) ? (int)reader["NationalityCountryID"] : (int)-1;
                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                ImagePath = (string)reader["ImagePath"];
                            }
                            else
                            {
                                ImagePath = "";
                            }
                        }
                        else
                        {
                            Isfound = false;
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
        /// Retrieves a filtered list of people records from the database where a specified column starts with the provided search prefix.
        /// Validates the column name against an internal whitelist to prevent SQL injection.
        /// </summary>
        /// <param name="filtercolumn">The database column name to filter by. Must match one of the whitelisted column names (e.g., <c>"NationalNo"</c>, <c>"FirstName"</c>, <c>"Phone"</c>).</param>
        /// <param name="FilterValue">The string prefix used to search for matching records (<c>LIKE @FilterValue + '%'</c>).</param>
        /// <returns>
        /// A <see cref="DataTable"/> containing all matching records, or an empty <see cref="DataTable"/> if no records match or an exception occurs.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="filtercolumn"/> is not present in the allowed columns whitelist.
        /// </exception>
        [DocInfo("Retrieves a filtered DataTable of people based on a specified column and search prefix.")]
        [StoredProcedure("SP_GetFilteredPeople")]
        public static DataTable GetFilteredPeople(string filtercolumn, string FilterValue)
        {
            
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetFilteredPeople", connection))
                {
                    command.Parameters.Add("@FilterValue", SqlDbType.NVarChar).Value = FilterValue;
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

        /// <summary>
        /// Retrieves all raw records and columns directly from the <c>People</c> table.
        /// </summary>
        /// <returns>
        /// A <see cref="DataTable"/> containing all rows from the <c>People</c> table, 
        /// or an empty <see cref="DataTable"/> if no records exist or an exception is encountered.
        /// </returns>
        [DocInfo("Retrieves all raw records directly from the People table.")]
        [StoredProcedure("SP_GetPeople")]
        public static DataTable GetPeople()
        {
            DataTable dt = new DataTable();
          
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetPeople", connection))
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

        /// <summary>
        /// Inserts a new person record into the <c>People</c> table and returns the auto-generated identity ID.
        /// </summary>
        /// <param name="NationalNo">The unique national identification number of the person.</param>
        /// <param name="FirstName">The person's first name.</param>
        /// <param name="Secondname">The person's second name (father's name).</param>
        /// <param name="ThirdName">The person's third name (grandfather's name), or <c>null</c>/empty if omitted.</param>
        /// <param name="LastName">The person's last name (family name).</param>
        /// <param name="DateOfBirth">The person's birth date.</param>
        /// <param name="Gendor">The numerical gender indicator (e.g., 0 for Male, 1 for Female).</param>
        /// <param name="Address">The physical residential address.</param>
        /// <param name="Phone">The primary contact telephone number.</param>
        /// <param name="Email">The email address, or <c>null</c>/empty if omitted.</param>
        /// <param name="NationalityCountryID">The foreign key referencing the person's nationality in the <c>Countries</c> table.</param>
        /// <param name="ImagePath">The file path to the stored photo, or an empty string/<c>null</c> if no image exists.</param>
        /// <returns>
        /// The newly generated <c>PersonID</c> (greater than 0) if insertion was successful; otherwise, <c>-1</c>.
        /// </returns>
        [DocInfo("Inserts a new person record and returns the generated PersonID.")]
        [StoredProcedure("SP_InsertNewLocalDrivingLicenseApp")]
        public static int InsertNewPerson(string NationalNo, string FirstName, string Secondname, string ThirdName, string LastName,
             DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_InsertNewLocalDrivingLicenseApp", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NationalNo", (object)NationalNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FirstName", (object)FirstName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SecondName", (object)Secondname ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ThirdName", (object)ThirdName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", (object)LastName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Gendor);
                    command.Parameters.AddWithValue("@Address", (object)Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object)Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object)Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    if (ImagePath != "")
                    {
                        command.Parameters.AddWithValue("@ImagePath", ImagePath);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
                    }
                    try
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int inserted))
                        {
                            PersonID = inserted;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsEventLogger.LogException(ex, "Data Access Error");

                    }
                }
            }

            return PersonID;
        }

        /// <summary>
        /// Updates an existing person record in the <c>People</c> table matching the specified <paramref name="PersonID"/>.
        /// </summary>
        /// <param name="PersonID">The unique primary key identifier of the person record to update.</param>
        /// <param name="NationalNo">The updated national identification number.</param>
        /// <param name="FirstName">The updated first name.</param>
        /// <param name="Secondname">The updated second name (father's name).</param>
        /// <param name="ThirdName">The updated third name (grandfather's name), or <c>null</c>/empty if omitted.</param>
        /// <param name="LastName">The updated last name (family name).</param>
        /// <param name="DateOfBirth">The updated birth date.</param>
        /// <param name="Gendor">The updated numerical gender indicator (e.g., 0 for Male, 1 for Female).</param>
        /// <param name="Address">The updated physical residential address.</param>
        /// <param name="Phone">The updated primary contact telephone number.</param>
        /// <param name="Email">The updated email address, or <c>null</c>/empty if omitted.</param>
        /// <param name="NationalityCountryID">The updated foreign key referencing the person's nationality in the <c>Countries</c> table.</param>
        /// <param name="ImagePath">The updated file path to the stored photo, or an empty string/<c>null</c> if no image exists.</param>
        /// <returns>
        /// <c>true</c> if the record was successfully updated (at least 1 row affected); otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Updates an existing person record in the database.")]
        [StoredProcedure("SP_Updateperson")]
        public static bool Updateperson(int PersonID, string NationalNo, string FirstName, string Secondname, string ThirdName, string LastName,
             DateTime DateOfBirth, short Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {

            int rows = 0;
           
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_Updateperson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);
                    command.Parameters.AddWithValue("@NationalNo", (object)NationalNo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FirstName", (object)FirstName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SecondName", (object)Secondname ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ThirdName", (object)ThirdName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastName", (object)LastName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", Gendor);
                    command.Parameters.AddWithValue("@Address", (object)Address ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object)Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Email", (object)Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
                    if (ImagePath != "")
                    {
                        command.Parameters.AddWithValue("@ImagePath", ImagePath);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
                    }

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
        /// Deletes a person record from the <c>People</c> table matching the specified <paramref name="PersonID"/>.
        /// </summary>
        /// <param name="PersonID">The unique primary key identifier of the person record to delete.</param>
        /// <returns>
        /// <c>true</c> if the record was successfully deleted (at least 1 row affected); otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Deletes a person record by unique PersonID.")]
        [StoredProcedure("SP_DeletePerson")]
        public static bool DeletePerson(int PersonID)
        {
            int rows = 0;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_DeletePerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

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
        /// Checks whether a person record exists in the <c>People</c> table matching the specified <paramref name="PersonID"/>.
        /// </summary>
        /// <param name="PersonID">The unique primary key identifier of the person to verify.</param>
        /// <returns>
        /// <c>true</c> if a matching person record exists; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Checks whether a person record exists in the database by PersonID.")]
        [StoredProcedure("SP_IsPersonExist")]
        public static bool IsPersonExist(int PersonID)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_IsPersonExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        Isfound = reader.HasRows;

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
        /// Checks whether a person record exists in the <c>People</c> table matching the specified <paramref name="NationalNo"/>.
        /// </summary>
        /// <param name="NationalNo">The unique national identification number to verify.</param>
        /// <returns>
        /// <c>true</c> if a matching person record exists; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Checks whether a person record exists in the database by NationalNo.")]
        [StoredProcedure("SP_IsPersonExist")]
        public static bool IsPersonExist(string NationalNo)
        {
            bool Isfound = false;
            
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_IsPersonExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@NationalNo", NationalNo);

                    try
                    {
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        Isfound = reader.HasRows;

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
        /// Checks whether a user account exists in the <c>Users</c> table associated with the specified <paramref name="PersonID"/>.
        /// </summary>
        /// <param name="PersonID">The foreign key identifier referencing the person in the <c>People</c> table.</param>
        /// <returns>
        /// <c>true</c> if a user account is linked to the specified person; otherwise, <c>false</c>.
        /// </returns>
        [DocInfo("Checks whether a user record exists in the database for a given PersonID.")]
        [StoredProcedure("SP_IsUserExist")]
        public static bool IsUserExist(int PersonID)
        {
            bool Isfound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_IsUserExist", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        object results = command.ExecuteScalar();

                        if (results != null)
                        {
                            Isfound = true;
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
        /// Retrieves all records from the <c>People</c> table with formatted display fields for presentation layers.
        /// </summary>
        /// <returns>
        /// A <see cref="DataTable"/> containing all person records with transformed gender descriptions, 
        /// or an empty <see cref="DataTable"/> if no records exist or an exception occurs.
        /// </returns>
        [DocInfo("Retrieves a DataTable of all people records with formatted display fields.")]
        [StoredProcedure("SP_GetAllPeople")]
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
         
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllPeople", connection))
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
        /// Retrieves the complete local driver's license history for a specific person using their unique <paramref name="PersonID"/>.
        /// </summary>
        /// <param name="PersonID">The primary key identifier of the person whose license history is being requested.</param>
        /// <returns>
        /// A <see cref="DataTable"/> containing all local licenses associated with the person, including details such as 
        /// <c>LicenseID</c>, <c>ApplicationID</c>, <c>ClassName</c>, <c>IssueDate</c>, <c>ExpirationDate</c>, and <c>IsActive</c> status; 
        /// or an empty <see cref="DataTable"/> if no records exist or an exception occurs.
        /// </returns>
        [DocInfo("Retrieves the local license history for a person by joining Licenses, LicenseClasses, and Drivers tables.")]
        [StoredProcedure("SP_GetPersonLocalLicensesHistory")]
        public static DataTable GetPersonLocalLicensesHistory(int PersonID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
               
                using (SqlCommand command = new SqlCommand("SP_GetPersonLocalLicensesHistory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
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

        /// <summary>
        /// Retrieves the complete international driver's license history for a specific person using their unique <paramref name="PersonID"/>.
        /// </summary>
        /// <param name="PersonID">The primary key identifier of the person whose international license history is being requested.</param>
        /// <returns>
        /// A <see cref="DataTable"/> containing all international licenses associated with the person, including details such as 
        /// <c>Int.LicenseID</c>, <c>ApplicationID</c>, <c>Local.LicenseID</c>, <c>IssueDate</c>, <c>ExpirationDate</c>, and <c>IsActive</c> status; 
        /// or an empty <see cref="DataTable"/> if no records exist or an exception occurs.
        /// </returns>
        [DocInfo("Retrieves international license history for a person by joining InternationalLicenses and Drivers tables.")]
        [StoredProcedure("SP_GetPersonInternationalLicensesHistory")]
        public static DataTable GetPersonInternationalLicensesHistory(int PersonID)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.Connection))
            {
               
                using (SqlCommand command = new SqlCommand("SP_GetPersonInternationalLicensesHistory", connection))
                {
                    command.CommandType = CommandType.StoredProcedure; 

                    command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
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
