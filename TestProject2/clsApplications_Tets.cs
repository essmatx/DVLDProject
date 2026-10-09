using DVLD_Business;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Shared.Attributes.clsDocAttributes;

namespace TestProject2
{
    /// <summary>
    /// Integration test fixture for ClsApplicationBusiness that verifies CRUD operations, database persistence, and
    /// person-application query behavior.
    /// </summary>
    /// <remarks>Uses NUnit TestFixture with SetUp/TearDown to create and remove test person and application
    /// records. Requires a configured test database and may modify persistent data; run against an isolated test
    /// instance.</remarks>
    [ArchitectureLayer(enArchLayer.DVLD_UintTest)]
    [DocInfo("Integration test suite for ClsApplicationBusiness CRUD operations, database persistence, and person application query validation.", Module = "Applications Management", Version = "1.0")]
    [TestFixture]
    public class clsApplications_Tets
    {
        #region Private Test Fixture State

        /// <summary>
        /// Identifier for the test person used by the test fixture.
        /// </summary>
        /// <remarks>Set during test setup to the database identifier of the created test person and
        /// reused across tests.</remarks>
        private int _testPersonID;

        /// <summary>
        /// Identifier for the test application.
        /// </summary>
        private int _testAppID;
        #endregion

        #region Setup & Teardown

        /// <summary>
        /// Generates and saves a unique dummy person record to serve as the parent foreign key for application tests.
        /// </summary>
        /// <remarks>Creates a ClsPerson, assigns a unique NationalNo using APP_ plus a GUID suffix to
        /// avoid duplicates, populates name and demographic fields (FirstName, SecondName, ThirdName, LastName,
        /// DateOfBirth, Gendor, NationalityCountryID), saves the record, and stores the resulting PersonID in
        /// _testPersonID.</remarks>
        [SetUp]
        [DocInfo("Generates and saves a unique dummy person record to serve as the parent foreign key for application tests.")]
        public void Setup()
        {
            // Generate a random unique suffix so tests never lock up on duplicate NationalNo
            string uniqueNationalNo = "APP_" + Guid.NewGuid().ToString().Substring(0, 5);

            var p = new ClsPerson();
            p.NationalNo = uniqueNationalNo;
            p.FirstName = "App";
            p.SecondName = "Unit";
            p.ThirdName = "Framework";
            p.LastName = "Tester";
            p.DateOfBirth = DateTime.Now;
            p.Gendor = 0;
            p.NationalityCountryID = 1;
            p.Save();
            _testPersonID = p.PersonID;
        }

        /// <summary>
        /// Purge created application and person entities from the database to prevent test environment pollution.
        /// </summary>
        /// <remarks>Executed as a test teardown; deletes entities only when their test IDs are positive
        /// and relies on application-specific deletion helpers.</remarks>
        [DocInfo("Purges created application and person entities from the database to prevent test environment pollution.")]
        [TearDown]
        public void Cleanup()
        {
            if (_testAppID > 0)
                ClsApplicationBusiness.DeleteAppLication(_testAppID);

            if (_testPersonID > 0)
                ClsPerson.DeletePerson(_testPersonID);

        }
        #endregion

        #region Test Cases

        /// <summary>
        /// Verifies saving a valid application: Save() returns true, a positive ApplicationID is assigned, and the
        /// application is retrievable via FindApplicationByID.
        /// </summary>
        /// <remarks>Sets _testAppID to the saved ApplicationID. Populates required fields
        /// (ApplicationPersonID, ApplicationDate, ApplicationTypeID, AppStatus, LastStatus, PaidFees, CreatedByUserID)
        /// before saving.</remarks>
        [Test]
        [DocInfo("Verifies successful saving of a new application entity and confirms positive ID assignment and lookup payload.")]
        public void AddApplication_WhenValid_ShouldSave()
        {
            // Arrange
            var app = new ClsApplicationBusiness();
            app.ApplicationPersonID = _testPersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = 1;
            app.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
            app.LastStatus = DateTime.Now;
            app.PaidFees = 15;
            app.CreatedByUserID = 1;

            // Act
            bool result = app.Save();

            // Assert
            Assert.That(result, Is.True,"The application failed to save.");
            Assert.That(app.ApplicationID,Is.GreaterThan(0));
            _testAppID = app.ApplicationID;

            var found = ClsApplicationBusiness.FindApplicationByID(app.ApplicationID);
            Assert.That(found, Is.Not.Null);
        }

        /// <summary>
        /// Verifies that FindApplicationByPersonID returns a non-null DataTable containing at least one row for the
        /// specified person ID.
        /// </summary>
        /// <remarks>Arranges and saves a test application for the given person ID, invokes
        /// FindApplicationByPersonID, and asserts the returned DataTable is not null and contains at least one row. The
        /// test fails early if saving the application does not succeed.</remarks>
        [Test]
        [DocInfo("Ensures FindApplicationByPersonID returns a non-null DataTable containing records for the designated person ID.")]
        public void FindApplicationByPersonID_ShouldReturnDataTable()
        {
            // 1.Arrange
            var app = new ClsApplicationBusiness();

            // Mapping using your exact Business Layer property names
            app.ApplicationPersonID = _testPersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = 1;
            app.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
            app.LastStatus = DateTime.Now;
            app.PaidFees = 15;
            app.CreatedByUserID = 1;

            // Act: Attempt to save the record
            bool result = app.Save();

            // FIREWALL ASSERT: If your DAL prints "Error", the test stops right here with a clear message.
            Assert.That(result, Is.True,"The application failed to save to the database! Check your DAL's AddNewApplication method.");
            _testAppID = app.ApplicationID;

            // 2. Act: If it saves successfully, retrieve the data
            DataTable dt = ClsApplicationBusiness.FindApplicationByPersonID(_testPersonID);

            // 3. Assert: Verify the retrieval worked
            Assert.That(dt,Is.Not.Null ,"The returned DataTable should not be null.");
            Assert.That(dt.Rows.Count, Is.GreaterThan(0), "The DataTable should return at least one row for the test person.");
        }

        /// <summary>
        /// Verifies that updating an existing application's attributes persists changes to the database.
        /// </summary>
        /// <remarks>Creates a test application, modifies its PaidFees, saves the application, and asserts
        /// the persisted PaidFees matches the updated value.</remarks>
        [Test]
        [DocInfo("Ensures that modifying attributes on an existing application updates the database record correctly.")]
        public void UpdateApplication_ShouldModifyRecord()
        {
            // Arrange
            var app = new ClsApplicationBusiness();
            app.ApplicationPersonID = _testPersonID;
            app.ApplicationDate = DateTime.Now;
            app.ApplicationTypeID = 1;
            app.AppStatus = ClsApplicationBusiness.enApplicationStatus.New;
            app.LastStatus = DateTime.Now;
            app.PaidFees = 15;
            app.CreatedByUserID = 1;
            app.Save();
            _testAppID = app.ApplicationID;

            // Act
            var toUpdate = ClsApplicationBusiness.FindApplicationByID(_testAppID);
            Assert.That(toUpdate,Is.Not.Null,"Could not find the application to update.");
            toUpdate.PaidFees = 100;
            bool result = toUpdate.Save();

            // Assert
            Assert.That(result, Is.True,"Failed to save the updated application properties.");
            var verify = ClsApplicationBusiness.FindApplicationByID(_testAppID);
            Assert.That(verify.PaidFees,Is.EqualTo(100));
        }

        /// <summary>
        /// Verifies that GetAllApplications returns a non-null DataTable containing at least one row.
        /// </summary>
        /// <remarks>Asserts that the returned DataTable is not null and that Rows.Count is greater than
        /// zero.</remarks>
        [Test]
        [DocInfo("Verifies that GetAllApplications returns a populated DataTable object.")]
        public void GetAllApplications_ShouldReturnDataTable()
        {
            // Act
            DataTable dt = ClsApplicationBusiness.GetAllApplications();

            // Assert
            Assert.That(dt, Is.Not.Null,"GetAllApplications returned a null object.");
            Assert.That(dt.Rows.Count, Is.GreaterThan(0), "GetAllApplications returned no records.");
        }

        #endregion
    }
}
