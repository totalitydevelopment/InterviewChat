using Microsoft.EntityFrameworkCore;
using InterviewTest.Core.Entities;
using InterviewTest.Core.Interfaces;
using InterviewTest.Infrastructure.Data;

namespace InterviewTest.Infrastructure.Repositories;

public class BookingRepository : Repository<Booking>, IBookingRepository
{
    public BookingRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(int guestId)
    {
        return await _dbSet
            .Where(b => b.GuestId == guestId)
            .Include(b => b.Guest)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Where(b => b.CheckInDate >= startDate && b.CheckOutDate <= endDate)
            .Include(b => b.Guest)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
    {
        return await _dbSet
            .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn)
            .Include(b => b.Guest)
            .ToListAsync();
    }
}