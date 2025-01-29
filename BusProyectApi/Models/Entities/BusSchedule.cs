using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusProyectApi.Models.Entities
{
    public class BusSchedule
    {
        [Key]
        public int Id { get; set; } // Identifier

        [Required(ErrorMessage = "Arriving Time is Required")]
        [FutureDate(ErrorMessage = "Please enter a date greater than or equal to today.")]
        public DateTime ArrivingTime { get; set; } // Arriving time

        [Required(ErrorMessage = "Departing Time is Required")]
        [FutureDate(ErrorMessage = "Please enter a date greater than or equal to today.")]
        public DateTime DepartingTime { get; set; } // Departing time

        [ForeignKeyExists(typeof(BusInfo), "BusInfo")]
        [Required(ErrorMessage = "Bus FK is Required")]
        [StringLength(18)]
        public string BusId { get; set; }

        // Foreign key to RouteInfo
        [ForeignKeyExists(typeof(RouteInfo), "RouteInfo")]
        [Required(ErrorMessage = "Route FK is Required")]
        public int RouteId { get; set; }
    }
}
