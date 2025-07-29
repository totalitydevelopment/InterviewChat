using InterviewTest.Core.Entities;
using InterviewTest.Core.ValueObjects;
using InterviewTest.Infrastructure.Data;

namespace InterviewTest.Infrastructure.Data;

public class DataSeeder
{
    public static async Task SeedDataAsync(ApplicationDbContext context)
    {
        // Check if data already exists
        if (context.Guests.Any())
            return;

        // Create guests
        var guests = new List<Guest>
        {
            new Guest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "555-0123",
                DateOfBirth = new DateTime(1985, 5, 15),
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "Anytown",
                    State = "CA",
                    PostalCode = "12345",
                    Country = "USA"
                }
            },
            new Guest
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "555-0456",
                DateOfBirth = new DateTime(1990, 8, 22),
                Address = new Address
                {
                    Street = "456 Oak Ave",
                    City = "Somewhere",
                    State = "NY",
                    PostalCode = "67890",
                    Country = "USA"
                }
            },
            new Guest
            {
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob.johnson@example.com",
                PhoneNumber = "555-0789",
                DateOfBirth = new DateTime(1982, 3, 10),
                Address = new Address
                {
                    Street = "789 Pine Rd",
                    City = "Elsewhere",
                    State = "TX",
                    PostalCode = "54321",
                    Country = "USA"
                }
            }
        };

        context.Guests.AddRange(guests);
        await context.SaveChangesAsync();

        // Create bookings
        var bookings = new List<Booking>
        {
            new Booking
            {
                GuestId = guests[0].Id,
                RoomNumber = "101",
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(3),
                RoomRate = 150.00m,
                TaxRate = 0.10m,
                Status = BookingStatus.Confirmed
            },
            new Booking
            {
                GuestId = guests[1].Id,
                RoomNumber = "102",
                CheckInDate = DateTime.Today.AddDays(5),
                CheckOutDate = DateTime.Today.AddDays(8),
                RoomRate = 200.00m,
                TaxRate = 0.10m,
                Status = BookingStatus.Confirmed
            },
            new Booking
            {
                GuestId = guests[0].Id,
                RoomNumber = "201",
                CheckInDate = DateTime.Today.AddDays(10),
                CheckOutDate = DateTime.Today.AddDays(14),
                RoomRate = 180.00m,
                TaxRate = 0.10m,
                Status = BookingStatus.Pending
            }
        };

        context.Bookings.AddRange(bookings);
        await context.SaveChangesAsync();
    }
}