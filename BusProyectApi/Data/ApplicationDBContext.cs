using BusProyectApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BusProyectApi.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users { get; set; }

        public DbSet<BusInfo> buses { get; set; }

        public DbSet<RouteInfo> routes { get; set; }

        public DbSet<BusSchedule> schedules { get; set; }

        public DbSet<Booking> bookings { get; set; }

    }
}
