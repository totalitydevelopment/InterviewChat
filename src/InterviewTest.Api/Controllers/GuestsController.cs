using InterviewTest.Api.DTOs;
using InterviewTest.Core.Entities;
using InterviewTest.Core.Interfaces;
using InterviewTest.Core.Services;
using InterviewTest.Core.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace InterviewTest.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuestsController : ControllerBase
{
    private readonly IGuestRepository _guestRepository;
    private readonly BookingService _bookingService;

    public GuestsController(IGuestRepository guestRepository, BookingService bookingService)
    {
        _guestRepository = guestRepository;
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GuestDto>>> GetGuests()
    {
        var guests = await _guestRepository.GetAllAsync();
        var guestDtos = guests.Select(MapToDto);
        return Ok(guestDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GuestDto>> GetGuest(int id)
    {
        var guest = await _guestRepository.GetByIdAsync(id);
        if (guest == null)
            return NotFound();

        return Ok(MapToDto(guest));
    }

    [HttpGet("with-bookings")]
    public async Task<ActionResult<IEnumerable<GuestDto>>> GetGuestsWithBookings()
    {
        var guests = await _guestRepository.GetGuestsWithBookingsAsync();
        var guestDtos = new List<GuestDto>();

        foreach (var guest in guests)
        {
            var dto = MapToDto(guest);
            var revenue = await _bookingService.CalculateBookingRevenueForGuestAsync(guest.Id);
            guestDtos.Add(dto);
        }

        return Ok(guestDtos);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<GuestDto>>> SearchGuests([FromQuery] string term)
    {
        if (string.IsNullOrWhiteSpace(term))
            return BadRequest("Search term is required");

        var guests = await _guestRepository.SearchGuestsAsync(term);
        var guestDtos = guests.Select(MapToDto);
        return Ok(guestDtos);
    }

    [HttpPost]
    public async Task<ActionResult<GuestDto>> CreateGuest(CreateGuestDto createGuestDto)
    {
        // BUG
        if (string.IsNullOrWhiteSpace(createGuestDto.Email) ||
            !createGuestDto.Email.Contains("@") ||
            createGuestDto.DateOfBirth >= DateTime.Today) 
        {
            return BadRequest("Invalid guest data: Email must contain @ and date of birth must be in the past");
        }

        var existingGuest = await _guestRepository.GetByEmailAsync(createGuestDto.Email);
        if (existingGuest != null)
            return Conflict("A guest with this email already exists");

        var guest = new Guest
        {
            FirstName = createGuestDto.FirstName,
            LastName = createGuestDto.LastName,
            Email = createGuestDto.Email,
            PhoneNumber = createGuestDto.PhoneNumber,
            DateOfBirth = createGuestDto.DateOfBirth,
            Address = new Address
            {
                Street = createGuestDto.Address.Street,
                City = createGuestDto.Address.City,
                State = createGuestDto.Address.State,
                PostalCode = createGuestDto.Address.PostalCode,
                Country = createGuestDto.Address.Country
            }
        };

        var createdGuest = await _guestRepository.AddAsync(guest);
        return CreatedAtAction(nameof(GetGuest), new { id = createdGuest.Id }, MapToDto(createdGuest));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<GuestDto>> UpdateGuest(int id, UpdateGuestDto updateGuestDto)
    {
        var guest = await _guestRepository.GetByIdAsync(id);
        if (guest == null)
            return NotFound();

        guest.FirstName = updateGuestDto.FirstName;
        guest.LastName = updateGuestDto.LastName;
        guest.Email = updateGuestDto.Email;
        guest.PhoneNumber = updateGuestDto.PhoneNumber;
        guest.DateOfBirth = updateGuestDto.DateOfBirth;
        guest.Address.Street = updateGuestDto.Address.Street;
        guest.Address.City = updateGuestDto.Address.City;
        guest.Address.State = updateGuestDto.Address.State;
        guest.Address.PostalCode = updateGuestDto.Address.PostalCode;
        guest.Address.Country = updateGuestDto.Address.Country;

        var updatedGuest = await _guestRepository.UpdateAsync(guest);
        return Ok(MapToDto(updatedGuest));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGuest(int id)
    {
        var guest = await _guestRepository.GetByIdAsync(id);
        if (guest == null)
            return NotFound();

        await _guestRepository.DeleteAsync(id);
        return NoContent();
    }

    [HttpGet("{id}/bookings")]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetGuestBookings(int id)
    {
        var guest = await _guestRepository.GetByIdAsync(id);
        if (guest == null)
            return NotFound();

        var bookings = guest.Bookings.Select(MapBookingToDto);
        return Ok(bookings);
    }

    private static GuestDto MapToDto(Guest guest)
    {
        return new GuestDto
        {
            Id = guest.Id,
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            Email = guest.Email,
            PhoneNumber = guest.PhoneNumber,
            DateOfBirth = guest.DateOfBirth,
            Address = new AddressDto
            {
                Street = guest.Address.Street,
                City = guest.Address.City,
                State = guest.Address.State,
                PostalCode = guest.Address.PostalCode,
                Country = guest.Address.Country
            },
            FullName = guest.FullName,
            Age = guest.Age,
            CreatedAt = guest.CreatedAt
        };
    }

    private static BookingDto MapBookingToDto(Booking booking)
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