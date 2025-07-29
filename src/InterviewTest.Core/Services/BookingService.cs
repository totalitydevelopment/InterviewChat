using InterviewTest.Core.Entities;
using InterviewTest.Core.Interfaces;

namespace InterviewTest.Core.Services;

// CODE SMELL: This service violates DDD by mixing data access concerns with domain logic
// It should use repository interfaces instead of direct data access
// The logging and validation logic should also be separated
public class BookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IGuestRepository _guestRepository;

    public BookingService(IBookingRepository bookingRepository, IGuestRepository guestRepository)
    {
        _bookingRepository = bookingRepository;
        _guestRepository = guestRepository;
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        // CODE SMELL: Direct validation in service rather than using a validator
        if (booking.CheckInDate >= booking.CheckOutDate)
        {
            throw new ArgumentException("Check-in date must be before check-out date");
        }

        // CODE SMELL: This validation logic has a subtle bug - always fails
        // BUG: This condition will always be true, causing validation to always fail
        if (booking.CheckInDate.Date <= DateTime.Today.Date) // Should be < not <=
        {
            throw new ArgumentException("Check-in date must be in the future");
        }

        // CODE SMELL: Direct logging in domain service
        Console.WriteLine($"Creating booking for guest {booking.GuestId}");

        // CODE SMELL: Data access mixed with domain logic
        var guest = await _guestRepository.GetByIdAsync(booking.GuestId);
        if (guest == null)
        {
            throw new ArgumentException("Guest not found");
        }

        return await _bookingRepository.AddAsync(booking);
    }

    public async Task<decimal> CalculateBookingRevenueForGuestAsync(int guestId)
    {
        // PERFORMANCE ISSUE: This will cause N+1 query problem
        var guest = await _guestRepository.GetByIdAsync(guestId);
        if (guest == null) return 0;

        decimal total = 0;
        // This will trigger individual queries for each booking
        foreach (var booking in guest.Bookings)
        {
            total += booking.CalculateTotal();
        }

        return total;
    }
}