using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusProyectApi.Models.Entities
{
    public class BusInfo
    {
        [Key]
        public Guid Id { get; set; } // Identifier (bus plates

        [Required(ErrorMessage = "Capacity is needed")]
        public int Capacity { get; set; } // Number of seats

        [DefaultValue(true)]
        public bool IsAvailable { get; set; } // Available
        [Required(ErrorMessage = "Category is needed")]
        public string Category { get; set; } // Category
        [Required(ErrorMessage = "Category is needed")]
        public string Status { get; set; } // Status
    }
}
