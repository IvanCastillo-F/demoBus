using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusProyectApi.Models.Entities
{
    public class BusInfo
    {
        [Key]
        [StringLength(18)]
        public string BusPlate { get; set; } // Identifier (bus plates)

        [Required(ErrorMessage = "Capacity is required")]
        public int? Capacity { get; set; } // Number of seats

        //[DefaultValue(true)]
        public bool IsAvailable { get; set; } = true; // Available

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } // Category
        [Required(ErrorMessage = "Status is needed")]
        public string Status { get; set; } // Status
        
    }
}
 