using Xunit;
using Moq;
using InterviewTest.Core.Entities;
using InterviewTest.Core.Interfaces;
using InterviewTest.Core.Services;

namespace InterviewTest.Core.Tests;

public class BookingServiceTests
{
    [Fact]
    public async Task CreateBookingAsync_WithValidData_ShouldCreateBooking()
    {
        // Arrange
        var mockBookingRepo = new Mock<IBookingRepository>();
        var mockGuestRepo = new Mock<IGuestRepository>();
        
        var guest = new Guest { Id = 1, FirstName = "John", LastName = "Doe" };
        var booking = new Booking
        {
            GuestId = 1,
            CheckInDate = DateTime.Today.AddDays(2),
            CheckOutDate = DateTime.Today.AddDays(4),
            RoomNumber = "101"
        };

        mockGuestRepo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(guest);
        mockBookingRepo.Setup(x => x.AddAsync(It.IsAny<Booking>())).ReturnsAsync(booking);

        var service = new BookingService(mockBookingRepo.Object, mockGuestRepo.Object);

        // Act
        var result = await service.CreateBookingAsync(booking);

        // Assert
        Assert.NotNull(result);
        mockBookingRepo.Verify(x => x.AddAsync(It.IsAny<Booking>()), Times.Once);
    }

    [Fact]
    public async Task CreateBookingAsync_WithTodayCheckIn_ShouldThrowException()
    {
        // Arrange
        var mockBookingRepo = new Mock<IBookingRepository>();
        var mockGuestRepo = new Mock<IGuestRepository>();
        
        var booking = new Booking
        {
            GuestId = 1,
            CheckInDate = DateTime.Today, // Today's date should fail validation
            CheckOutDate = DateTime.Today.AddDays(2),
            RoomNumber = "101"
        };

        var service = new BookingService(mockBookingRepo.Object, mockGuestRepo.Object);

        // Act & Assert
        // This test demonstrates the bug in validation logic
        await Assert.ThrowsAsync<ArgumentException>(() => service.CreateBookingAsync(booking));
    }
}