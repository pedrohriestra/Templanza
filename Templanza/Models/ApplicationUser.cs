using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Templanza.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? ImagenUrl { get; set; }
    }
}
