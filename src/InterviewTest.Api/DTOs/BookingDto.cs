using InterviewTest.Core.Entities;

namespace InterviewTest.Api.DTOs;

public class BookingDto
{
    public int Id { get; set; }
    public int GuestId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal RoomRate { get; set; }
    public decimal TaxRate { get; set; }
    public BookingStatus Status { get; set; }
    public int NumberOfNights { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBookingDto
{
    public int GuestId { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal RoomRate { get; set; }
}

public class UpdateBookingDto
{
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public decimal RoomRate { get; set; }
    public BookingStatus Status { get; set; }
}