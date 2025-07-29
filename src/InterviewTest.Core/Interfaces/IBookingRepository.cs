using InterviewTest.Core.Entities;

namespace InterviewTest.Core.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(int guestId);
    Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Booking>> GetActiveBookingsAsync();
}