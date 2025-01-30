using BusProyectApi.Data.Interfaces;
using BusProyectApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BusProyectApi.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingController> _logger;

        public BookingController(IBookingRepository bookingRepository, ILogger<BookingController> logger)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooking()
        {
            try
            {
                var bookings = await _bookingRepository.GetAllBookingsAsyc();
                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "Record Not Found"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            try
            {
                var existingBooking = await _bookingRepository.GetBookingByIdAsyc(id);
                if (existingBooking == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                return Ok(existingBooking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = ex.Message
                });
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddBooking(Booking booking)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Admin User cannot do a Booking " });// Return 403 Forbidden
                }
                booking.Id = 0;
                // Check for duplicate booking
                var duplicateBooking = await _bookingRepository.GetBookingByCriteriaAsync(
                    booking.UserId,
                    booking.BusScheduleId);

                if (duplicateBooking != null)
                {
                    return Conflict(new
                    {
                        StatusCode = 409,
                        message = "Duplicate booking detected."
                    });
                }

                // Create the new booking
                var createdBooking = await _bookingRepository.CreateBookingAsync(booking);

                return CreatedAtAction(nameof(AddBooking),createdBooking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBooking(Booking booking)
        {
            try
            {
                if (User.IsInRole("Admin"))
                {
                    return StatusCode(StatusCodes.Status403Forbidden, new { message = "Admin User cannot do a Booking " });// Return 403 Forbidden
                }
                // Fetch the existing booking
                var existingBooking = await _bookingRepository.GetBookingByIdAsyc(booking.Id);
                if (existingBooking == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }

                // Update the existing booking
                existingBooking.UserId = booking.UserId;
                existingBooking.BusScheduleId = booking.BusScheduleId;
                existingBooking.NumberOfSeats = booking.NumberOfSeats;
                existingBooking.Cost = booking.Cost;

                await _bookingRepository.UpdateBookingAsync(existingBooking);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = "An unexpected error occurred."
                });
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            try
            {
                var existingBooking = await _bookingRepository.GetBookingByIdAsyc(id);
                if (existingBooking == null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        message = "Record Not Found"
                    });
                }
                await _bookingRepository.DeleteBookingAsync(existingBooking);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    StatusCode = 500,
                    message = ex.Message
                });
            }
        }

    }
}
