using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusProyectApi.Models.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; } // Bus ticket

        [Required(ErrorMessage = "Distance is Required")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public double Cost { get; set; } // Cost

        [Required(ErrorMessage = "Number Of Seats is Required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int NumberOfSeats { get; set; }

        // Foreign key to User
        [Required(ErrorMessage = "User Id is Required")]
        [ForeignKeyExists(typeof(User), "User")]
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; } // Navigation property for User

        // Foreign key to BusSchedule
        [Required(ErrorMessage = "Bus Schedule Id is Required")]
        [ForeignKeyExists(typeof(BusSchedule), "BusSchedule")]
        [ForeignKey(nameof(BusScheduleId))]
        public int BusScheduleId { get; set; }
    }
}
