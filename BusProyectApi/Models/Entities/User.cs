using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BusProyectApi.Models.Entities
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; } // Identifier

        [Required(ErrorMessage = "User Name is required")]
        [StringLength(50)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [DefaultValue(false)]
        public bool IsAdmin { get; set; } // Is_Admin
    }
}
