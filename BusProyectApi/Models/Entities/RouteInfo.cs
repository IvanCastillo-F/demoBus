using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BusProyectApi.Models.Entities
{
    [Index(nameof(RouteName), IsUnique = true)]
    public class RouteInfo
    {
        [Key]
        public int Id { get; set; } // Identifier

        [Required(ErrorMessage = "Route Name is Required")]
        [StringLength(150)]
        public string RouteName { get; set; } // Route name

        [Required(ErrorMessage = "Departure Place is Required")]
        [StringLength(100)]
        public string DeparturePlace { get; set; } // Departure place

        [Required(ErrorMessage = "Arriving Place is Required")]
        [StringLength(100)]
        public string ArrivingPlace { get; set; } // Arriving place

        [Required(ErrorMessage = "Distance is Required")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public double Distance { get; set; } // Distance

        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int NumberOfStops { get; set; } // #stops
    }

}
