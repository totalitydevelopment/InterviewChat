using Microsoft.AspNetCore.Mvc;
using InterviewTest.Core.Entities;
using InterviewTest.Core.Interfaces;
using InterviewTest.Core.Services;
using InterviewTest.Api.DTOs;

namespace InterviewTest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly BookingService _bookingService;

    public BookingsController(IBookingRepository bookingRepository, BookingService bookingService)
    {
        _bookingRepository = bookingRepository;
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        var bookingDtos = bookings.Select(MapToDto);
        return Ok(bookingDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBooking(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
            return NotFound();

        return Ok(MapToDto(booking));
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetActiveBookings()
    {
        var bookings = await _bookingRepository.GetActiveBookingsAsync();
        var bookingDtos = bookings.Select(MapToDto);
        return Ok(bookingDtos);
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> CreateBooking(CreateBookingDto createBookingDto)
    {
        var booking = new Booking
        {
            GuestId = createBookingDto.GuestId,
            RoomNumber = createBookingDto.RoomNumber,
            CheckInDate = createBookingDto.CheckInDate,
            CheckOutDate = createBookingDto.CheckOutDate,
            RoomRate = createBookingDto.RoomRate
        };

        try
        {
            var createdBooking = await _bookingService.CreateBookingAsync(booking);
            return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, MapToDto(createdBooking));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<BookingDto>> UpdateBooking(int id, UpdateBookingDto updateBookingDto)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
            return NotFound();

        booking.RoomNumber = updateBookingDto.RoomNumber;
        booking.CheckInDate = updateBookingDto.CheckInDate;
        booking.CheckOutDate = updateBookingDto.CheckOutDate;
        booking.RoomRate = updateBookingDto.RoomRate;
        booking.Status = updateBookingDto.Status;

        var updatedBooking = await _bookingRepository.UpdateAsync(booking);
        return Ok(MapToDto(updatedBooking));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
            return NotFound();

        await _bookingRepository.DeleteAsync(id);
        return NoContent();
    }

    private static BookingDto MapToDto(Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            GuestId = booking.GuestId,
            GuestName = booking.Guest?.FullName ?? "",
            RoomNumber = booking.RoomNumber,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            RoomRate = booking.RoomRate,
            TaxRate = booking.TaxRate,
            Status = booking.Status,
            NumberOfNights = booking.NumberOfNights,
            Total = booking.CalculateTotal(),
            CreatedAt = booking.CreatedAt
        };
    }
}