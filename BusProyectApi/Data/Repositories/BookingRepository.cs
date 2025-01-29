using BusProyectApi.Data.Interfaces;
using BusProyectApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusProyectApi.Data.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDBContext _context;
        public BookingRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsyc()
        {
            var bookings = await _context.bookings.ToArrayAsync();
            return bookings;
        }

        public async Task<Booking> GetBookingByIdAsyc(int id)
        {
            return await _context.bookings.FindAsync(id);
        }

        public async Task<Booking> CreateBookingAsync(Booking booking)
        {
            _context.bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _context.bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(Booking booking)
        {
            _context.bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<Booking> GetBookingByCriteriaAsync(int userId, int scheduleId, int? excludeId = null)
        {
            var query = _context.bookings.Where(b =>
                b.UserId == userId &&
                b.BusScheduleId == scheduleId);

            if (excludeId.HasValue)
            {
                query = query.Where(b => b.Id != excludeId.Value);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}
