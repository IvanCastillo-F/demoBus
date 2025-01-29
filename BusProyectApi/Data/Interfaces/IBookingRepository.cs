using BusProyectApi.Models.Entities;

namespace BusProyectApi.Data.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBookingAsync(Booking booking);
        Task DeleteBookingAsync(Booking booking);
        Task<IEnumerable<Booking>> GetAllBookingsAsyc();
        Task<Booking> GetBookingByIdAsyc(int id);
        Task UpdateBookingAsync(Booking booking);
        Task<Booking> GetBookingByCriteriaAsync(int userId, int scheduleId, int? excludeId = null);
    }
}