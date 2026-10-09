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
/// Unit test fixture for ClsApplicationTypeBusiness lookups, fee updates, and application type listing operations.
/// </summary>
/// <remarks>Uses NUnit TestFixture. Includes tests for finding application types by ID and title, updating
/// application fees (with rollback to restore original state), and retrieving all application types. Tests assume
/// specific test data exists (for example an application type with ID 1) and may interact with persistent
/// storage.</remarks>
[ArchitectureLayer(enArchLayer.DVLD_UintTest)]
[DocInfo("Unit test suite for ClsApplicationTypeBusiness lookups, fee updates, and type listing operations.", Module = "Application Types", Version = "1.0")]
[TestFixture]
public class clsApplicationTypes_Tests
{
    #region Test Cases

    /// <summary>
    /// Verifies retrieval of the application type for a valid primary key.
    /// </summary>
    /// <remarks>Assumes an entity with primary key 1 exists; asserts the result is not null and its
    /// ApplicationTypeID equals 1.</remarks>
    [Test]
    [DocInfo("Verifies FindApplicationTypeByID retrieves the expected application type entity when supplied with a valid primary key.")]
    public void FindApplicationTypeByID_WhenValid_ShouldReturnType()
    {
        // Act (Assuming ID 1 exists)
        var result = ClsApplicationTypeBusiness.FindApplicationTypeByID(1);

        // Assert
        Assert.That(result,Is.Not.Null);
        Assert.That(result.ApplicationTypeID, Is.EqualTo(1)); 
    }

    /// <summary>
    /// Verifies that FindApplicationTypeByTitle returns the application type entity matching the provided title string.
    /// </summary>
    /// <remarks>Uses the title 'Replacement for a Lost Driving License' and asserts the returned entity is
    /// not null and has an ApplicationTypeTitle equal to the provided title.</remarks>
    [Test]
    [DocInfo("Verifies FindApplicationTypeByTitle returns the target application type entity matching the provided title string.")]
    public void FindApplicationTypeByTitle_WhenValid_ShouldReturnType()
    {
        // Act
        var result = ClsApplicationTypeBusiness._FindApplicationTypeByTitle("Replacement for a Lost Driving License");

        // Assert
        Assert.That(result,Is.Not.Null);
        Assert.That(result.ApplicationTypeTitle, Is.EqualTo("Replacement for a Lost Driving License"));
    }

    /// <summary>
    /// Updates the application fee for the application type with ID 1, saves the change, asserts the update succeeded
    /// and the persisted value matches, and restores the original fee to maintain database state.
    /// </summary>
    /// <remarks>Mutates persistent state and performs verification against the database; restores the
    /// original ApplicationFees value to avoid side effects. Requires an application type record with ID 1 to
    /// exist.</remarks>
    [Test]
    [DocInfo("Tests application fee mutation, verifies database persistence, and guarantees restoration of original fee state in a finally block.")]
    public void UpdateApplicationFees_ShouldModifyRecord()
    {
        // Arrange
        var app = ClsApplicationTypeBusiness.FindApplicationTypeByID(1);
        decimal originalFees = app.ApplicationFees;
        decimal newFees = originalFees + 10;

        // Act
        app.ApplicationFees = newFees;
        bool result = app.Save();

        // Assert
        Assert.That(result,Is.True);
        var verify = ClsApplicationTypeBusiness.FindApplicationTypeByID(1);
        Assert.That(newFees, Is.EqualTo(verify.ApplicationFees)); 

        // Rollback
        verify.ApplicationFees = originalFees;
        verify.Save();
    }

    /// <summary>
    /// Verifies GetAllApplicationTypes returns a non-null, populated DataTable of application type definitions.
    /// </summary>
    /// <remarks>Calls ClsApplicationTypeBusiness.GetAllApplicationTypes and asserts the returned DataTable is
    /// not null and contains one or more rows.</remarks>
    [Test]
    [DocInfo("Verifies GetAllApplicationTypes yields a populated DataTable containing application type definitions.")]
    public void GetAllApplicationTypes_ShouldReturnDataTable()
    {
        // Act
        DataTable dt = ClsApplicationTypeBusiness.GetAllApplicationTypes();

        // Assert
        Assert.That(dt,Is.Not.Null);
        Assert.That(dt.Rows.Count, Is.GreaterThan(0)); 
    }

    #endregion
}
