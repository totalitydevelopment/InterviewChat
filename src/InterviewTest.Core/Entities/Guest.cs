using InterviewTest.Core.ValueObjects;

namespace InterviewTest.Core.Entities;

public class Guest : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Address Address { get; set; } = new();
    
    // Navigation property
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    
    public string FullName => $"{FirstName} {LastName}";
    
    public int Age => DateTime.Today.Year - DateOfBirth.Year - 
        (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
}