using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Templanza.Models
{
    // Usuario de Identity extendido con datos propios del dominio.
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(100, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Como máximo {1} caracteres.")]
        public string? ImagenUrl { get; set; }
    }
}
