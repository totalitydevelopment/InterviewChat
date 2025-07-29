using Xunit;
using InterviewTest.Core.Entities;
using InterviewTest.Core.ValueObjects;

namespace InterviewTest.Core.Tests;

public class GuestTests
{
    [Fact]
    public void FullName_ShouldCombineFirstAndLastName()
    {
        // Arrange
        var guest = new Guest
        {
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var fullName = guest.FullName;

        // Assert
        Assert.Equal("John Doe", fullName);
    }

    [Fact]
    public void Age_ShouldCalculateCorrectAge()
    {
        // Arrange
        var guest = new Guest
        {
            DateOfBirth = DateTime.Today.AddYears(-30)
        };

        // Act
        var age = guest.Age;

        // Assert
        Assert.Equal(30, age);
    }

    [Fact]
    public void Address_ShouldFormatCorrectly()
    {
        // Arrange
        var address = new Address
        {
            Street = "123 Main St",
            City = "Anytown",
            State = "CA",
            PostalCode = "12345",
            Country = "USA"
        };

        // Act
        var addressString = address.ToString();

        // Assert
        Assert.Equal("123 Main St, Anytown, CA 12345, USA", addressString);
    }
}