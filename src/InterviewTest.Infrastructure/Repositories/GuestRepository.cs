using Microsoft.EntityFrameworkCore;
using InterviewTest.Core.Entities;
using InterviewTest.Core.Interfaces;
using InterviewTest.Infrastructure.Data;

namespace InterviewTest.Infrastructure.Repositories;

public class GuestRepository : Repository<Guest>, IGuestRepository
{
    public GuestRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Guest?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(g => g.Email == email);
    }

    // PERFORMANCE ISSUE: This method causes N+1 queries when accessing bookings
    // Should use eager loading with Include
    public async Task<IEnumerable<Guest>> GetGuestsWithBookingsAsync()
    {
        // BAD: This will cause N+1 queries when accessing guest.Bookings
        return await _dbSet.ToListAsync();
        
        // GOOD: Should be:
        // return await _dbSet.Include(g => g.Bookings).ToListAsync();
    }

    public async Task<IEnumerable<Guest>> SearchGuestsAsync(string searchTerm)
    {
        return await _dbSet
            .Where(g => g.FirstName.Contains(searchTerm) || 
                       g.LastName.Contains(searchTerm) || 
                       g.Email.Contains(searchTerm))
            .ToListAsync();
    }
}