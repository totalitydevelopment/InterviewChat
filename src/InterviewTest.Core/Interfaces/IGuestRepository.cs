using InterviewTest.Core.Entities;

namespace InterviewTest.Core.Interfaces;

public interface IGuestRepository : IRepository<Guest>
{
    Task<Guest?> GetByEmailAsync(string email);
    Task<IEnumerable<Guest>> GetGuestsWithBookingsAsync();
    Task<IEnumerable<Guest>> SearchGuestsAsync(string searchTerm);
}