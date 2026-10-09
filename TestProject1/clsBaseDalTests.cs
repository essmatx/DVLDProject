using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace TestProject1
{
    public abstract class clsBaseDalTests
    {
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;

        [SetUp]
        public void Setup()
        {
            string connectionString = "Server=ESSMAT;Database=DVLD;Trusted_Connection=True;";

            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        [TearDown]
        public void Teardown()
        {
            _transaction?.Rollback();
            _connection?.Close();
            _connection?.Dispose();
        }

        protected int ExecuteScalarInt(string query, Action<SqlCommand> parameterAction = null)
        {
            using (SqlCommand cmd = new SqlCommand(query, _connection, _transaction))
            {
                bool Isfound = false;
                parameterAction?.Invoke(cmd);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    Isfound = true;
                }

                return result != null ? Convert.ToInt32(result) : -1;

            }
        }


        protected int ExecuteNonQuery(string query, Action<SqlCommand> parameterAction = null)
        {
            using (SqlCommand cmd = new SqlCommand(query, _connection, _transaction))
            {
                parameterAction?.Invoke(cmd);

                return cmd.ExecuteNonQuery();
            }
        }

        protected int CreateTestPerson(string nationalNo = "DAL_BASE_001")
        {
            string query = @"INSERT INTO People (NationalNo, FirstName, SecondName, ThirdName, LastName, 
                       DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
                     VALUES (@NationalNo, 'Base', 'Dal', 'Test', 'User', 
                       '1990-01-01', 0, 'Street', '010', 'base@x.com', 1, ''); SELECT SCOPE_IDENTITY();";

            return ExecuteScalarInt(query, cmd => cmd.Parameters.AddWithValue("@NationalNo", nationalNo));
        }


    }
}
