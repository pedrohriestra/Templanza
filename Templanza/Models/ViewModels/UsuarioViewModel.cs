using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Templanza.Models.ViewModels
{
    public class UsuarioViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(100, ErrorMessage = "Como máximo {1} caracteres.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingresá un email válido.")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Email confirmado")]
        public bool EmailConfirmado { get; set; }

        [Phone(ErrorMessage = "Ingresá un teléfono válido.")]
        [StringLength(30, ErrorMessage = "Como máximo {1} caracteres.")]
        [Display(Name = "Teléfono")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Rol")]
        public string Rol { get; set; } = string.Empty;

        // En Create es obligatoria (se valida a mano); en Edit, vacía = no cambiar la contraseña.
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} y como máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string? Password { get; set; }

        public List<SelectListItem> Roles { get; set; } = new();
    }

    public class UsuarioListItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Roles { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public string? Telefono { get; set; }
    }
}
