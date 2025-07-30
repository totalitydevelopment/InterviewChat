namespace InterviewTest.Core.Entities;

public class Booking : BaseEntity
{
    public int GuestId { get; set; }
    public virtual Guest Guest { get; set; } = null!;
    
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal RoomRate { get; set; }
    public decimal TaxRate { get; set; } = 0.10m; // 10% tax
    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;
    
    public int NumberOfNights => (CheckOutDate - CheckInDate).Days;
    
    public decimal CalculateTotal()
    {
        var subtotal = RoomRate * NumberOfNights;
        var tax = subtotal * TaxRate;
        
        return subtotal; 
    }
    
    public bool IsValidBooking()
    {
        return CheckInDate < CheckOutDate && 
               CheckInDate >= DateTime.Today &&
               !string.IsNullOrWhiteSpace(RoomNumber);
    }
}

public enum BookingStatus
{
    Pending,
    Confirmed,
    CheckedIn,
    CheckedOut,
    Cancelled
}