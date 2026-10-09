using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Business;
using System.Data;
using System.Data.SqlClient;
using static DVLD_Shared.Attributes.clsDocAttributes;


namespace TestProject2;

/// <summary>
/// Integration test suite for ClsDriverBusiness covering CRUD operations, person linkage, and driver record queries.
/// </summary>
/// <remarks>Uses SetUp to create a Person and a linked Driver before each test and TearDown to remove created
/// records. Contains tests for adding, finding by driver ID and person ID, updating, deleting, and retrieving all
/// drivers. Tests interact with the database and require a valid test environment; they rely on a persistent
/// CreatedUserID (1) and existing lookup data where applicable.</remarks>
[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Integration test suite for ClsDriverBusiness CRUD operations, person linkage, and driver record queries.", Module = "Drivers Management", Version = "1.0")]
public class clsDrivers_Tests
{
    #region Private Test Fixture State

    /// <summary>
    /// Identifier for the test person used by the test fixture.
    /// </summary>
    /// <remarks>Assigned during test setup and referenced by tests to identify the persistent test
    /// record.</remarks>
    private int _testPersonID;

    /// <summary>
    /// Backing field for the test driver's identifier.
    /// </summary>
    private int _testDriverID;
    #endregion

    #region Setup & Teardown

    /// <summary>
    /// Generates a unique ClsPerson and a linked ClsDriverBusiness record to serve as fixture data for driver business
    /// tests.
    /// </summary>
    /// <remarks>Executed before each test ([SetUp]). Creates a ClsPerson with all name fields, contact
    /// information, nationality, and other required fields, saves it and assigns its PersonID to _testPersonID. Then
    /// creates a ClsDriverBusiness linked to that person, sets CreatedUserID and CreatedDate, saves it and assigns its
    /// DriverID to _testDriverID.</remarks>
    [SetUp]
    [DocInfo("Generates a unique person and linked driver record to serve as fixture data for driver business tests.")]
    public void Setup()
    {
        // --- Step A: Create a Person (with ALL 4 name fields) ---
        var p = new ClsPerson();
        p.NationalNo = "DRIVER_TEST_001";
        p.FirstName = "Driver";
        p.SecondName = "Unit";
        p.ThirdName = "Test";
        p.LastName = "Framework";
        p.DateOfBirth = new DateTime(1990, 1, 1);
        p.Gendor = 0;
        p.Address = "456 Driver Ave";
        p.Phone = "01111111111";
        p.Email = "driver.test@example.com";
        p.NationalityCountryID = 1;
        p.ImagePath = "";
        p.Save();
        _testPersonID = p.PersonID;

        // --- Step B: Create a Driver linked to this Person ---
        var driver = new ClsDriverBusiness();
        driver.PersonID = _testPersonID;
        driver.CreatedUserID = 1;
        driver.CreatedDate = DateTime.Now;
        driver.Save();
        _testDriverID = driver.DriverID;
    }

    /// <summary>
    /// Purge created driver and parent person entities from the database to ensure environment isolation.
    /// </summary>
    /// <remarks>Deletes entities in reverse order of creation: deletes the driver if its ID is greater than
    /// zero, then deletes the person.</remarks>
    [TearDown]
    [DocInfo("Purges created driver and parent person entities from the database to ensure environment isolation.")]
    public void Cleanup()
    {
        // Delete in REVERSE order of creation
        if (_testDriverID > 0)
        {
            ClsDriverBusiness.DeleteDiver(_testDriverID); // NOW THIS EXISTS!
        }

        // Delete the Person
        ClsPerson.DeletePerson(_testPersonID);
    }
    #endregion

    #region Test Cases

    /// <summary>
    /// Saves a new driver linked to a newly created person and verifies the save succeeded and a positive DriverID was
    /// assigned.
    /// </summary>
    /// <remarks>Creates and saves a ClsPerson with test data, constructs a ClsDriverBusiness with the new
    /// PersonID and metadata, calls Save(), asserts the result is true and DriverID is greater than zero, then deletes
    /// the created person.</remarks>
    [Test]
    [DocInfo("Verifies successful creation of a new driver entity and confirms positive ID assignment.")]
    public void AddDriver_WhenValid_ShouldSaveToDatabase()
    {
        // ========== ARRANGE ==========
        // Create a NEW Person specifically for this test
        var p = new ClsPerson();
        p.NationalNo = "ADD_DRIVER_002";
        p.FirstName = "Add";
        p.SecondName = "Driver";
        p.ThirdName = "Test";
        p.LastName = "User";
        p.DateOfBirth = new DateTime(1990, 1, 1);
        p.Gendor = 0;
        p.Address = "789 New St";
        p.Phone = "01234567890";
        p.Email = "add.driver@test.com";
        p.NationalityCountryID = 1;
        p.ImagePath = "";
        p.Save();
        int newPersonID = p.PersonID;

        var driver = new ClsDriverBusiness();
        driver.PersonID = newPersonID;
        driver.CreatedUserID = 1;
        driver.CreatedDate = DateTime.Now;

        // ========== ACT (READ) ==========
        bool result = driver.Save();

        // ========== ASSERT (COMPARE) ==========
        Assert.That(result,Is.True, "Save operation failed!");
        Assert.That(driver.DriverID, Is.GreaterThan(0), "Driver ID was not generated!");

        // Cleanup the Person we created for this test
        ClsPerson.DeletePerson(newPersonID);
    }

    /// <summary>
    /// Verifies that FindDriverByID returns the expected driver business entity for a valid driver ID.
    /// </summary>
    /// <remarks>Depends on a driver created in SetUp with ID = _testDriverID. Calls
    /// ClsDriverBusiness.FindDriverByID(_testDriverID) and asserts the result is not null and that DriverID and
    /// PersonID match expected values.</remarks>
    [Test]
    [DocInfo("Ensures FindDriverByID returns the target driver business entity when provided with a valid primary key.")]
    public void FindDriverByID_WhenValid_ShouldReturnDriver()
    {
        // ========== ARRANGE ==========
        // Driver was created in [SetUp] with ID = _testDriverID

        // ========== ACT (READ) ==========
        var found = ClsDriverBusiness.FindDriverByID(_testDriverID);

        // ========== ASSERT (COMPARE) ==========
        Assert.That(found, Is.Not.Null,"Driver was not found by ID!");
        Assert.That(found.DriverID,Is.EqualTo( _testDriverID) , "Driver ID does not match!");
        Assert.That(found.PersonID, Is.EqualTo(_testPersonID), "Person ID does not match!");
    }

    /// <summary>
  /// Verifies that FindDriverByPerson returns the driver associated with the specified person ID.
  /// </summary>
  /// <remarks>Uses a driver record created in SetUp with PersonID = _testPersonID. Asserts the result is
  /// non-null and that PersonID and DriverID match the expected values.</remarks>
    [Test]
    [DocInfo("Ensures FindDriverByPerson returns the corresponding driver record linked to the target person ID.")]
    public void FindDriverByPersonID_WhenValid_ShouldReturnDriver()
    {
        // ========== ARRANGE ==========
        // Driver was created in [SetUp] with PersonID = _testPersonID

        // ========== ACT (READ) ==========
        var found = ClsDriverBusiness.FindDriverByPerson(_testPersonID);

        // ========== ASSERT (COMPARE) ==========
        Assert.That(found, Is.Not.Null,"Driver was not found by Person ID!");
        Assert.That(found.PersonID,Is.EqualTo( _testPersonID) , "Person ID does not match!");
        Assert.That(found.DriverID,Is.EqualTo( _testDriverID), "Driver ID does not match!");
    }

    /// <summary>
   /// Update and verify persistence of an existing driver's CreatedDate and CreatedUserID in the database.
   /// </summary>
   /// <remarks>Performs a read-modify-write cycle against the persistent store and re-fetches the record to
   /// assert database-level changes. Uses an exact historical date to avoid millisecond alignment issues and assumes a
   /// driver with _testDriverID and a valid CreatedUserID (1) exist. Intended as an integration test; failures
   /// typically indicate a data access layer or SQL issue.</remarks>
    [Test]
    [DocInfo("Ensures updating driver creation date and creator user ID mutates database record state correctly.")]
    public void UpdateDriver_ShouldModifyExistingRecord()
    {
        // ========== ARRANGE ==========
        // Use an exact historical date to avoid any weird milliseconds alignment issues
        DateTime oldDate = new DateTime(2020, 1, 1);

        // ========== ACT (READ & WRITE) ==========
        var toUpdate = ClsDriverBusiness.FindDriverByID(_testDriverID);
        Assert.That(toUpdate, Is.Not.Null,"Could not find the test driver to update!");

        toUpdate.CreatedDate = oldDate;

        // FIX: Kept as 1 (or ensure User ID 2 actually exists in your database)
        toUpdate.CreatedUserID = 1;

        bool result = toUpdate.Save();

        // ========== ASSERT (COMPARE) ==========
        // If this fails, look at ClsDriverData.UpdateDriver() for a SQL syntax or column name error!
        Assert.That(result, Is.True,"Update failed inside the Data Access Layer!");

        var verify = ClsDriverBusiness.FindDriverByID(_testDriverID);
        Assert.That(verify, Is.Not.Null,"Failed to re-fetch the updated driver record.");

        Assert.That(verify.CreatedDate.Date,Is.EqualTo( oldDate.Date), "CreatedDate was not updated in the database!");
        Assert.That(verify.CreatedUserID, Is.EqualTo(1), "CreatedUserID mismatch!"); 
    }

    /// <summary>
   /// Verifies that deleting a driver removes the corresponding driver entity from the database and that subsequent
   /// lookups return null.
   /// </summary>
   /// <remarks>Creates a Person and Driver for the test, calls ClsDriverBusiness.DeleteDiver(driverID),
   /// asserts the delete operation succeeded, verifies ClsDriverBusiness.FindDriverByID(driverID) returns null, and
   /// cleans up the created Person record.</remarks>
    [Test]
    [DocInfo("Ensures DeleteDiver removes driver entity from database, causing subsequent lookup to return null.")]
    public void DeleteDriver_WhenValid_ShouldRemoveFromDatabase()
    {
        // ========== ARRANGE ==========
        // Create a NEW Person and Driver specifically for this test
        var p = new ClsPerson();
        p.NationalNo = "DEL_DRIVER_003";
        p.FirstName = "Delete";
        p.SecondName = "Driver";
        p.ThirdName = "Test";
        p.LastName = "User";
        p.DateOfBirth = new DateTime(1990, 1, 1);
        p.Gendor = 0;
        p.Address = "999 Delete St";
        p.Phone = "01999999999";
        p.Email = "delete.driver@test.com";
        p.NationalityCountryID = 1;
        p.ImagePath = "";
        p.Save();
        int newPersonID = p.PersonID;

        var driver = new ClsDriverBusiness();
        driver.PersonID = newPersonID;
        driver.CreatedUserID = 1;
        driver.CreatedDate = DateTime.Now;
        driver.Save();
        int driverID = driver.DriverID;

        // ========== ACT (READ) ==========
        // ✅ FIXED: Uses the method we just added
        bool result = ClsDriverBusiness.DeleteDiver(driverID);

        // ========== ASSERT (COMPARE) ==========
        Assert.That(result, Is.True,"DeleteDriver returned false!");

        var verify = ClsDriverBusiness.FindDriverByID(driverID);
        Assert.That(verify, Is.Null,"Driver still exists after deletion!");

        // Cleanup the Person (safe now because the Driver is gone)
        ClsPerson.DeletePerson(newPersonID);
    }

    /// <summary>
    /// Verifies that ClsDriverBusiness.GetAllDriveres returns a non-empty DataTable of driver records.
    /// </summary>
    /// <remarks>Requires a driver to exist from the test fixture SetUp. Calls
    /// ClsDriverBusiness.GetAllDriveres and asserts the returned DataTable is not null and contains at least one
    /// DataRow.</remarks>
    [Test]
    [DocInfo("Verifies GetAllDriveres yields a populated DataTable containing driver records.")]
    public void GetAllDrivers_ShouldReturnDataTableWithRows()
    {
        // ========== ARRANGE ==========
        // Driver already exists from [SetUp]

        // ========== ACT (READ) ==========
        DataTable dt = ClsDriverBusiness.GetAllDriveres();

        // ========== ASSERT (COMPARE) ==========
        Assert.That(dt, Is.Not.Null,"GetAllDrivers returned null!");
        Assert.That(dt.Rows.Count, Is.GreaterThan(0), "Drivers table is empty!"); 
    }

    #endregion
}
