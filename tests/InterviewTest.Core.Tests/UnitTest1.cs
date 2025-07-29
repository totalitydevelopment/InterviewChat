using Xunit;
using InterviewTest.Core.Entities;

namespace InterviewTest.Core.Tests;

public class BookingTests
{
    [Fact]
    public void CalculateTotal_ShouldIncludeTax()
    {
        // Arrange
        var booking = new Booking
        {
            RoomRate = 100.00m,
            TaxRate = 0.10m,
            CheckInDate = DateTime.Today.AddDays(1),
            CheckOutDate = DateTime.Today.AddDays(3) // 2 nights
        };

        // Act
        var total = booking.CalculateTotal();

        // Assert
        // FAILING TEST: This test will fail because the CalculateTotal method has a bug
        // It doesn't include tax in the calculation
        // Expected: (100 * 2) + (100 * 2 * 0.10) = 200 + 20 = 220
        // Actual: 200 (tax not included)
        Assert.Equal(220.00m, total);
    }

    [Fact]
    public void NumberOfNights_ShouldCalculateCorrectly()
    {
        // Arrange
        var booking = new Booking
        {
            CheckInDate = new DateTime(2024, 1, 1),
            CheckOutDate = new DateTime(2024, 1, 5)
        };

        // Act
        var nights = booking.NumberOfNights;

        // Assert
        Assert.Equal(4, nights);
    }

    [Fact]
    public void IsValidBooking_WithValidData_ShouldReturnTrue()
    {
        // Arrange
        var booking = new Booking
        {
            CheckInDate = DateTime.Today.AddDays(1),
            CheckOutDate = DateTime.Today.AddDays(3),
            RoomNumber = "101"
        };

        // Act
        var isValid = booking.IsValidBooking();

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValidBooking_WithInvalidDates_ShouldReturnFalse()
    {
        // Arrange
        var booking = new Booking
        {
            CheckInDate = DateTime.Today.AddDays(3),
            CheckOutDate = DateTime.Today.AddDays(1), // Check-out before check-in
            RoomNumber = "101"
        };

        // Act
        var isValid = booking.IsValidBooking();

        // Assert
        Assert.False(isValid);
    }
}