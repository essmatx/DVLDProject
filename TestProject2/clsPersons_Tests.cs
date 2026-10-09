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

namespace TestProject2;

/// <summary>
/// Integration tests for ClsPerson covering create, read, update, delete, filtering, and list retrieval.
/// </summary>
/// <remarks>Uses a fixed TestNationalNo to create and clean up test records; TearDown removes any record with
/// that national number. CreateTestPerson persists a person with predefined fields (assumes NationalityCountryID 1
/// exists). Tests exercise ClsPerson.Save, FindPersonByID, DeletePerson, FindPeople and FilterPeop and assert expected
/// persistence and DataTable results.</remarks>
[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Integration test suite for ClsPerson CRUD operations, search filtering, and record management.", Module = "People Management", Version = "1.0")]
public class clsPersons_Tests
{
    #region Private Constants & Fields'

    /// <summary>
    /// Test national identification number used for automated tests and test data.
    /// </summary>
    /// <remarks>Intended for non-production use only.</remarks>
    private const string TestNationalNo = "TEST_NAT_999";
    #endregion

    #region Setup & Teardown

    /// <summary>
    /// Purges transient test person records matching TestNationalNo from the database to restore a clean execution
    /// state.
    /// </summary>
    /// <remarks>Executed after each test (TearDown) to remove any person record created during tests and
    /// prevent cross-test interference.</remarks>
    [TearDown]
    [DocInfo("Purges transient test person records from the database to ensure clean execution state.")]
    public void Cleanup()
    {
        var existing = ClsPerson.FindPersonByNationalNo(TestNationalNo);
        if (existing != null)
            ClsPerson.DeletePerson(existing.PersonID);
    }
    #endregion

    #region Private Helper Methods
    /// <summary>
    /// Creates and persists a ClsPerson populated with sample test data for use in unit tests.
    /// </summary>
    /// <remarks>Sets NationalityCountryID to 1 and calls ClsPerson.Save(); intended for use in test
    /// environments only.</remarks>
    /// <returns>A persisted ClsPerson instance populated with the sample values.</returns>
    private ClsPerson CreateTestPerson()
    {
        var p = new ClsPerson();
        p.NationalNo = TestNationalNo;
        p.FirstName = "Unit";
        p.SecondName = "Test";
        p.ThirdName = "Framework";
        p.LastName = "User";
        p.DateOfBirth = new DateTime(1990, 1, 1);
        p.Gendor = 0;
        p.Address = "123 Test St";
        p.Phone = "01000000000";
        p.Email = "test@unit.com";
        p.NationalityCountryID = 1; // Assumes Country ID 1 exists
        p.ImagePath = "";
        p.Save();
        return p;
    }
    #endregion

    #region Test Cases
    /// <summary>
    /// Verifies that saving a valid ClsPerson succeeds and that the saved person can be retrieved by its PersonID.
    /// </summary>
    /// <remarks>Creates a ClsPerson with test data, calls Save, asserts the save returned true, and asserts
    /// FindPersonByID returns a non-null result.</remarks>
    [Test]
    [DocInfo("Ensures successful creation of a new person entity and verifies record existence via primary key lookup.")]
    public void AddPerson_WhenValid_ShouldSave()
    {
        // Arrange
        var p = new ClsPerson();
        p.NationalNo = TestNationalNo;
        p.FirstName = "John";
        p.SecondName = "Middle";
        p.ThirdName = "Test";
        p.LastName = "Doe";
        p.DateOfBirth = DateTime.Now;
        p.Gendor = 0;
        p.NationalityCountryID = 1;

        // Act
        bool result = p.Save();

        // Assert
        Assert.That(result,Is.True);
        var found = ClsPerson.FindPersonByID(p.PersonID);
        Assert.That(found,Is.Not.Null);
    }

    /// <summary>
    /// Verifies that FindPersonByID returns the expected Person business entity when supplied a valid Person primary
    /// key.
    /// </summary>
    /// <remarks>Creates a test Person, obtains its PersonID, calls ClsPerson.FindPersonByID, and asserts the
    /// result is non-null and that the NationalNo matches the expected TestNationalNo.</remarks>
    [Test]
    [DocInfo("Ensures FindPersonByID returns the correct person business entity when provided a valid primary key.")]
    public void FindPersonByID_WhenValid_ShouldReturnPerson()
    {
        // Arrange
        var p = CreateTestPerson();
        int id = p.PersonID;

        // Act
        var found = ClsPerson.FindPersonByID(id);

        // Assert
        Assert.That(found,Is.Not.Null);
        Assert.That(found.NationalNo, Is.EqualTo(TestNationalNo)); 
    }

    /// <summary>
    /// Verifies that updating a person's properties and invoking Save persists the changes to the underlying database.
    /// </summary>
    /// <remarks>Creates a test person, retrieves it by ID, modifies FirstName, saves the entity and asserts
    /// the updated value is returned by a subsequent lookup. Depends on ClsPerson.FindPersonByID and persistent
    /// storage; run in isolation or within a transactional context to avoid side effects.</remarks>
    [Test]
    [DocInfo("Ensures modifying person properties updates database state correctly.")]
    public void UpdatePerson_ShouldModifyRecord()
    {
        // Arrange
        var p = CreateTestPerson();
        int id = p.PersonID;

        // Act
        var toUpdate = ClsPerson.FindPersonByID(id);
        toUpdate.FirstName = "UpdatedName";
        bool result = toUpdate.Save();

        // Assert
        Assert.That(result,Is.True);
        var verify = ClsPerson.FindPersonByID(id);
        Assert.That(verify.FirstName, Is.EqualTo("UpdatedName")); 
    }

    /// <summary>
    /// Verifies that DeletePerson removes the person record and that subsequent FindPersonByID calls return null.
    /// </summary>
    /// <remarks>Creates a test person, deletes it by ID, asserts the deletion returned true, and verifies the
    /// person cannot be found afterward.</remarks>
    [Test]
    [DocInfo("Ensures DeletePerson removes the record, causing subsequent FindPersonByID lookups to return null.")]
    public void DeletePerson_WhenValid_ShouldRemove()
    {
        // Arrange
        var p = CreateTestPerson();
        int id = p.PersonID;

        // Act
        bool result = ClsPerson.DeletePerson(id);

        // Assert
        Assert.That(result,Is.True);
        var verify = ClsPerson.FindPersonByID(id);
        Assert.That(verify,Is.Null);
    }

    /// <summary>
    /// Verifies that ClsPerson.FindPeople returns a non-null DataTable containing at least one person record.
    /// </summary>
    /// <remarks>Asserts the returned DataTable is not null and contains one or more rows; depends on
    /// available test data.</remarks>
    [Test]
    [DocInfo("Verifies FindPeople yields a populated DataTable containing person records.")]
    public void GetAllPeople_ShouldReturnDataTable()
    {
        // Act
        DataTable dt = ClsPerson.FindPeople();

        // Assert
        Assert.That(dt,Is.Not.Null);
        Assert.That(dt.Rows.Count, Is.GreaterThan(0)); 
    }

    /// <summary>
    /// Verifies that ClsPerson.FilterPeop returns one or more rows when filtering by the FirstName column using a
    /// matching search string.
    /// </summary>
    /// <remarks>Creates a test person whose first name contains Unit, invokes FilterPeop for the FirstName
    /// column with the search string Unit, and asserts the returned DataTable is not null and contains at least one
    /// row.</remarks>
    [Test]
    [DocInfo("Ensures FilterPeop correctly filters person records by column criteria and search string.")]
    public void FilterPeople_ByFirstName_ShouldReturnResults()
    {
        // Arrange
        var p = CreateTestPerson(); // Name includes "Unit"

        // Act
        DataTable dt = ClsPerson.FilterPeop("FirstName", "Unit");

        // Assert
        Assert.That(dt,Is.Not.Null);
        Assert.That(dt.Rows.Count, Is.GreaterThan(0)); 


    }

    #endregion
}
