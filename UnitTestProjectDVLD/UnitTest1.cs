using DVLD_Business;
using DVLD_DataAccess;
using NUnit.Framework;
using System;
using System.Data.SqlClient;


namespace UnitTestProjectDVLD
{
    // [TestClass]

    [TestFixture]
    public class UnitTest1:clsBaseDalTests
    {
        private int _testPersonID;

        [SetUp]
        public void AppSetup()
        {
            _testPersonID = CreateTestPerson("DAL_APP_BASE");
        }

        public void TestMethod1()
        {

        }

        [Test]
        public void Insert_ShouldReturnIdentity()
        {
            string query = @"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                             VALUES (@PID, GETDATE(), 1, 1, GETDATE(), 15, 1); SELECT SCOPE_IDENTITY();";
            int newID = ExecuteScalarInt(query, cmd => cmd.Parameters.AddWithValue("@PID", _testPersonID));
            Assert.Greater(newID, 0);
        }



        [Test]
        public void Update_ShouldAffectOneRow()
        {
            int id = ExecuteScalarInt(@"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                             VALUES (@PID, GETDATE(), 1, 1, GETDATE(), 15, 1); SELECT SCOPE_IDENTITY();",
                cmd => cmd.Parameters.AddWithValue("@PID", _testPersonID));

            int rows = ExecuteNonQuery("UPDATE Applications SET PaidFees = 100 WHERE ApplicationID = @ID",
                cmd => cmd.Parameters.AddWithValue("@ID", id));
            Assert.AreEqual(1, rows);
        }


        [Test]
        public void Delete_ShouldAffectOneRow()
        {
            int id = ExecuteScalarInt(@"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                             VALUES (@PID, GETDATE(), 1, 1, GETDATE(), 15, 1); SELECT SCOPE_IDENTITY();",
                cmd => cmd.Parameters.AddWithValue("@PID", _testPersonID));

            int rows = ExecuteNonQuery("DELETE FROM Applications WHERE ApplicationID = @ID",
                cmd => cmd.Parameters.AddWithValue("@ID", id));
            Assert.AreEqual(1, rows);
        }


        [Test]
        public void GetAll_ShouldReturnRows()
        {
            // Arrange: Ensure a test application exists
            int personID = CreateTestPerson("DAL_APP_GETALL");
            ExecuteNonQuery(@"INSERT INTO Applications (ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID)
                     VALUES (@PID, GETDATE(), 1, 1, GETDATE(), 15, 1)",
                cmd => cmd.Parameters.AddWithValue("@PID", personID));

            // Act
            string query = "SELECT COUNT(*) FROM Applications";
            using (SqlCommand cmd = new SqlCommand(query, _connection, _transaction))
            {
                int count = Convert.ToInt32(cmd.ExecuteScalar());

                // Assert
                Assert.Greater(count, 0, "GetAll returned no rows.");
            }
        }
    }

}
