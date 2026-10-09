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
/// Integration test suite for ClsUserBuiness lifecycle management, authentication logic, and user record queries.
/// </summary>
/// <remarks>Uses NUnit [SetUp], [TearDown], and [Test] attributes. Creates a test person for each test and
/// removes created users and the person during cleanup. Exercises add, find by ID and username, update, authenticate
/// (valid and invalid credentials), delete, and retrieval of all users. Tests interact with the database and may
/// perform hard deletions; run in an isolated test environment.</remarks>
[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Integration test suite for ClsUserBuiness lifecycle management, authentication logic, and user record queries.", Module = "User Management", Version = "1.0")]
public class clsUsers_Tests
{
    #region Private Constants & Fields

    /// <summary>
    /// The identifier of the test person.
    /// </summary>
    private int _testPersonID;

    /// <summary>
    /// User name constant used by unit tests.
    /// </summary>
    /// <remarks>Intended for use within unit tests and test fixtures.</remarks>
    private const string TestUserName = "UNIT_TEST_USER";
    #endregion

    #region Setup & Teardown

    /// <summary>
    /// Creates and saves a transient person entity and stores its identifier for use in fixture tests.
    /// </summary>
    /// <remarks>Executed as test setup; populates a ClsPerson with predefined test data, persists it via
    /// Save(), and assigns the resulting PersonID to _testPersonID.</remarks>
    [SetUp]
    [DocInfo("Creates a transient person entity to associate with user accounts in fixture tests.")]
    public void Setup()
    {
        var p = new ClsPerson();

        p.NationalNo = "USER_TEST_PER_001";
        p.FirstName = "User";
        p.SecondName = "Unit";
        p.ThirdName = "Test";
        p.LastName = "Framework";
        p.DateOfBirth = new System.DateTime(1995, 1, 1);
        p.Gendor = 0;
        p.Address = "123 Test Street";
        p.Phone = "01000000000";
        p.Email = "unit.test@example.com";
        p.NationalityCountryID = 1;

        p.ImagePath = "";

        p.Save();

        _testPersonID = p.PersonID;

    }

    /// <summary>
    /// Cleans up created test users and associated person records to maintain test isolation.
    /// </summary>
    /// <remarks>Finds and deletes the user with TestUserName if present, then deletes the person record
    /// identified by _testPersonID. Executed as a teardown operation after each test and safe to run multiple
    /// times.</remarks>
    [TearDown]
    [DocInfo("Cleans up created test users and associated person records to maintain test isolation.")]
    public void Cleanup()
    {
        var user = ClsUserBuiness.FindUserByUserName(TestUserName);
        if (user != null)
        {
            ClsUserBuiness.DeleteUser(user.UserID);
        }

        ClsPerson.DeletePerson(_testPersonID);

    }
    #endregion

    #region Test Cases

    /// <summary>
    /// Verifies that Save creates and persists a new user account associated with the specified person ID and that the
    /// user can be retrieved by username.
    /// </summary>
    /// <remarks>Relies on an existing test person identified by _testPersonID and a unique TestUserName;
    /// asserts Save returns true and that the persisted user has the expected PersonID. Does not perform cleanup of
    /// created test data.</remarks>
    [Test]
    [DocInfo("Ensures Save successfully creates a new user account linked to a valid person ID.")]
    public void AddUser_WhenValid_ShouldSave()
    {
        var user = new ClsUserBuiness();
        user.PersonID = _testPersonID;
        user.UserName = TestUserName;
        user.Password = "1234";
        user.IsActive = true;

        bool result = user.Save();

        Assert.That(result, Is.True,"Save operation failed.");

        var found = ClsUserBuiness.FindUserByUserName(TestUserName);
        Assert.That(found, Is.Not.Null,"User was not found after save.");

        Assert.That(found.PersonID, Is.EqualTo(_testPersonID));
    }

    /// <summary>
    /// Retrieves the user entity with the specified identifier.
    /// </summary>
    /// <remarks>Queries the underlying data store using the primary key and returns the matching user or null
    /// if none is found.</remarks>
    [Test]
    [DocInfo("Ensures FindUserByID retrieves the expected user entity when provided a valid primary key.")]
    public void FindUserByID_WhenValid_ShouldReturnUser()
    {
        var user = new ClsUserBuiness();
        user.PersonID = _testPersonID;
        user.UserName = TestUserName;
        user.Password = "1234";
        user.IsActive = true;
        user.Save();
        int id = user.UserID;

        var found = ClsUserBuiness.FindUserByID(id);

        Assert.That(found,Is.Not.Null);
        Assert.That(found.UserName,Is.EqualTo(TestUserName));
    }

    /// <summary>
    /// Verifies that modifying a user's username and password persists the changes to the database record.
    /// </summary>
    /// <remarks>Creates a test user, updates its username and password, saves the changes and asserts Save
    /// returns true; then retrieves the user by the updated username and verifies the stored password matches the new
    /// value.</remarks>
    [Test]
    [DocInfo("Ensures modifying user credentials and profile information updates the database record.")]
    public void UpdateUser_ShouldModifyRecord()
    {
        // ========== ARRANGE ==========
        var user = new ClsUserBuiness();
        user.PersonID = _testPersonID;
        user.UserName = TestUserName;
        user.Password = "1234";
        user.IsActive = true;
        user.Save();
        int id = user.UserID;

        // ========== ACT ==========
        var toUpdate = ClsUserBuiness.FindUserByID(id);
        toUpdate.UserName = "UPDATED_USER";
        toUpdate.Password = "NewPassword";
        bool result = toUpdate.Save();

        // ========== ASSERT ==========
        Assert.That(result, Is.True,"The Save() method returned false during update!");

        // FIX: Added the missing 'D' to match "UPDATED_USER"
        var verify = ClsUserBuiness.FindUserByUserName("UPDATED_USER");

        Assert.That(verify, Is.Not.Null,"Failed to find the user by their updated username!");
        Assert.That(verify.Password, Is.EqualTo("NewPassword"), "The password was not updated in the database!"); 

    }

    /// <summary>
    /// Authenticates a user by username and password and returns the matching user entity when credentials are valid.
    /// </summary>
    /// <remarks>Verifies exact credential matching (including password casing) and that the user is active;
    /// asserts the result is not null and that the returned username equals the supplied username.</remarks>
    [Test]
    [DocInfo("Ensures Loging returns the matching user entity when provided valid credentials.")]
    public void Login_WithValidCredentials_ShouldReturnUser()
    {
        // ========== ARRANGE ==========
        var user = new ClsUserBuiness();
        user.PersonID = _testPersonID;
        user.UserName = TestUserName;
        user.Password = "CorrectPass"; // Saved with a Capital 'C'
        user.IsActive = true;
        user.Save();

        // ========== ACT ==========
        // FIX: Changed "correctPass" to "CorrectPass" to match casing exactly
        var result = ClsUserBuiness.Loging(TestUserName, "CorrectPass");

        // ========== ASSERT ==========
        Assert.That(result, Is.Not.Null,"Login failed! Returned null for valid credentials.");
        Assert.That(result.UserName, Is.EqualTo(TestUserName), "The authenticated user's username does not match."); 

    }

    /// <summary>
    /// Verifies that Loging returns null when an incorrect password is supplied for an existing active user.
    /// </summary>
    /// <remarks>Creates and persists a user with a known password, then attempts authentication using an
    /// incorrect password and asserts that the result is null.</remarks>
    [Test]
    [DocInfo("Ensures Loging returns null when an incorrect password is provided for authentication.")]
    public void Login_WithInvalidPassword_ShouldReturnNull()
    {
        var user = new ClsUserBuiness();
        user.PersonID = _testPersonID;
        user.UserName = TestUserName;
        user.Password = "CorrectPass";
        user.IsActive = true;
        user.Save();

        var result = ClsUserBuiness.Loging(TestUserName, "WrongPass");

        Assert.That(result, Is.Null,"SECURITY BREACH: The system allowed a wrong password!");
    }

    /// <summary>
    /// Verifies that DeleteUser permanently removes an existing user record so subsequent FindUserByID calls return
    /// null.
    /// </summary>
    /// <remarks>Creates and saves a test user, calls DeleteUser(id), asserts a successful delete result, and
    /// verifies FindUserByID(id) returns null. If the application uses soft deletes (IsActive flag), change the
    /// verification to assert IsActive is false.</remarks>
    [Test]
    [DocInfo("Ensures DeleteUser removes the user record, causing subsequent FindUserByID lookups to return null.")]
    public void DeleteUser_WhenValid_ShouldRemoveFromDatabase()
    {
        // ========== ARRANGE ==========
        var user = new ClsUserBuiness();
        user.PersonID = _testPersonID;
        user.UserName = TestUserName;
        user.Password = "1234";
        user.IsActive = true;
        user.Save();

        int id = user.UserID;
        Assert.That(id, Is.GreaterThan(0), "User failed to insert during setup!"); 

        // ========== ACT ==========
        bool result = ClsUserBuiness.DeleteUser(id);

        // ========== ASSERT ==========
        // If this fails, your DAL Delete query has a typo or a Foreign Key constraint violation!
        Assert.That(result,Is.True, "DeleteUser returned false from the Data Access Layer.");

        // FIX: Changed from FindPersonByID to FindUserByID
        var verify = ClsUserBuiness.FindUserByID(id);

        // Note: If your system uses Soft Delete (IsActive = 0), change this assertion to:
        // Assert.IsFalse(verify.IsActive, "User was not soft-deleted!");
        Assert.That(verify, Is.Null,"User record still exists in the database after hard deletion!");
    }

    /// <summary>
    /// Verifies that GetAllUsers returns a non-null, populated DataTable containing user records.
    /// </summary>
    /// <remarks>Unit test using NUnit; calls ClsUserBuiness.GetAllUsers and asserts the returned DataTable is
    /// not null and has at least one DataRow.</remarks>
    [Test]
    [DocInfo("Ensures GetAllUsers returns a populated DataTable containing user records.")]
    public void GetAllUsers_ShouldReturnDataTable()
    {
        DataTable dt = ClsUserBuiness.GetAllUsers();

        Assert.That(dt,Is.Not.Null);
        Assert.That(dt.Rows.Count, Is.GreaterThan(0), "Users table is empty!"); 
    }

    #endregion
}
