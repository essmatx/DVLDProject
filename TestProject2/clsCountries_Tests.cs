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
/// Integration tests that validate clsCountry CRUD operations, persistence, and catalog query behavior.
/// </summary>
/// <remarks>Requires a database-backed integration test environment. Tests create, retrieve, update, and delete a
/// test country and verify persistence and catalog query results; TearDown removes the test country to restore
/// state.</remarks>
[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Integration test suite for clsCountry CRUD operations, database persistence, and country catalog queries.", Module = "Countries Management", Version = "1.0")]
public class clsCountries_Tests
{
    #region Private Constants & Fields

    /// <summary>
    /// Test country name used in unit tests.
    /// </summary>
    /// <remarks>Unique marker for creating and locating test country entities; intended for test use
    /// only.</remarks>
    private const string TestCountryName = "TEST_Country_UnitTest";
    #endregion

    #region Setup & Teardown

    /// <summary>
    /// Removes transient test country records from the database to ensure a clean test environment.
    /// </summary>
    /// <remarks>Executed after each test; locates the country by TestCountryName and deletes it if
    /// present.</remarks>
    [TearDown]
    [DocInfo("Purges transient test country records from the database to ensure clean test environment state.")]
    public void Cleanup()
    {
        // Delete the test country if it exists
        var existing = clsCountry._FindCountry(TestCountryName);
        if (existing != null)
            clsCountry.DeleteCountry(existing.CountryID);
    }
    #endregion

    #region Test Cases

    /// <summary>
    /// Verifies that a valid country is created, persisted, and retrievable by its generated ID.
    /// </summary>
    /// <remarks>Performs an arrange-act-assert sequence: creates a clsCountry with TestCountryName, calls
    /// Save(), asserts a successful save, then retrieves the entity by CountryID and asserts the CountryName
    /// matches.</remarks>
    [Test]
    [DocInfo("Verifies successful creation and persistence of a new country record, confirming positive ID generation and lookup payload.")]
    public void AddCountry_WhenValid_ShouldSaveToDatabase()
    {
        // Arrange
        var newCountry = new clsCountry();
        newCountry.CountryName = TestCountryName;

        // Act (READ)
        bool result = newCountry.Save();

        // Assert (COMPARE)
        Assert.That(result, Is.True,"Save failed.");

        // Verify it exists
        var found = clsCountry.FindCountryByID(newCountry.CountryID);
        Assert.That(found,Is.Not.Null);
        Assert.That(found.CountryName,Is.EqualTo(TestCountryName));
    }

    /// <summary>
    /// Verifies that FindCountryByID returns the expected clsCountry instance when provided a valid CountryID.
    /// </summary>
    /// <remarks>Creates and saves a clsCountry, retrieves it by its CountryID, and asserts the result is not
    /// null and its CountryName matches the expected value.</remarks>
    [Test]
    [DocInfo("Ensures FindCountryByID returns the correct country business entity when provided a valid primary key.")]
    public void FindCountryByID_WhenValid_ShouldReturnCountry()
    {
        // Arrange
        var newCountry = new clsCountry();
        newCountry.CountryName = TestCountryName;
        newCountry.Save();
        int id = newCountry.CountryID;

        // Act (READ)
        var found = clsCountry.FindCountryByID(id);

        // Assert (COMPARE)
        Assert.That(found,Is.Not.Null);
        Assert.That(found.CountryName, Is.EqualTo(TestCountryName));
    }

    /// <summary>
    /// Verifies that updating a country's CountryName and saving persists the change to the database.
    /// </summary>
    /// <remarks>Creates a country, reads and updates its CountryName, calls Save(), asserts the updated value
    /// is persisted, and deletes the test record to clean up. Mutates database state.</remarks>
    [Test]
    [DocInfo("Ensures updating country attributes mutates database state correctly.")]
    public void UpdateCountry_WhenValid_ShouldModifyRecord()
    {
        // Arrange
        var newCountry = new clsCountry();
        newCountry.CountryName = "Old_Name";
        newCountry.Save();
        int id = newCountry.CountryID;

        // Act (READ)
        var toUpdate = clsCountry.FindCountryByID(id);
        toUpdate.CountryName = "Updated_Name";
        bool result = toUpdate.Save();

        // Assert (COMPARE)
        Assert.That(result,Is.True);
        var verify = clsCountry.FindCountryByID(id);
        Assert.That(verify.CountryName, Is.EqualTo("Updated_Name")); 

        // Cleanup specific
        clsCountry.DeleteCountry(id);
    }

    /// <summary>
    /// Verifies that deleting an existing country removes it from persistent storage and that subsequent
    /// FindCountryByID calls with the same ID return null.
    /// </summary>
    /// <remarks>Creates and saves a country to obtain an ID, deletes it via clsCountry.DeleteCountry(id), and
    /// asserts deletion succeeded and retrieval returns null. Modifies persistent state and should be run in isolation
    /// or with transactional cleanup.</remarks>
    [Test]
    [DocInfo("Ensures DeleteCountry removes the record permanently, causing subsequent FindCountryByID calls to return null.")]
    public void DeleteCountry_WhenValid_ShouldRemoveRecord()
    {
        // Arrange
        var newCountry = new clsCountry();
        newCountry.CountryName = TestCountryName;
        newCountry.Save();
        int id = newCountry.CountryID;

        // Act (READ)
        bool result = clsCountry.DeleteCountry(id);

        // Assert (COMPARE)
        Assert.That(result,Is.True);
        var verify = clsCountry.FindCountryByID(id);
        Assert.That(verify,Is.Null);
    }

    /// <summary>
    /// Verifies that clsCountry.GetAllCountries returns a non-null DataTable containing one or more country records.
    /// </summary>
    /// <remarks>Invokes clsCountry.GetAllCountries and asserts the returned DataTable is not null and
    /// contains at least one row.</remarks>
    [Test]
    [DocInfo("Verifies GetAllCountries yields a populated DataTable containing country records.")]
    public void GetAllCountries_ShouldReturnDataTable()
    {
        // Act (READ)
        DataTable dt = clsCountry.GetAllCountries();

        // Assert (COMPARE)
        Assert.That(dt,Is.Not.Null);
        Assert.That(dt.Rows.Count, Is.GreaterThan(0));
    }

    #endregion
}
