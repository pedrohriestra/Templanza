using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Templanza.Models.ViewModels
{
    // Formulario de alta/edición de una Planta.
    public class PlantaViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(150, ErrorMessage = "Como máximo {1} caracteres.")]
        [Display(Name = "Nombre común")]
        public string NombreComun { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(150, ErrorMessage = "Como máximo {1} caracteres.")]
        [Display(Name = "Nombre científico")]
        public string NombreCientifico { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Como máximo {1} caracteres.")]
        public string? Contraindicaciones { get; set; }

        [StringLength(100, ErrorMessage = "Como máximo {1} caracteres.")]
        [Display(Name = "Parte usada")]
        public string? ParteUsada { get; set; }

        [StringLength(100, ErrorMessage = "Como máximo {1} caracteres.")]
        public string? Origen { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Range(0.01, 100000, ErrorMessage = "Debe estar entre {1} y {2}.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "Debe estar entre {1} y {2}.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        public List<SelectListItem> Categorias { get; set; } = new();

        public List<PlantaEfectoItemViewModel> Efectos { get; set; } = new();
    }

    // Un renglón del checklist de efectos dentro del formulario de Planta.
    public class PlantaEfectoItemViewModel
    {
        public int EfectoId { get; set; }
        public string NombreEfecto { get; set; } = string.Empty;
        public bool Seleccionado { get; set; }

        [Range(1, 5, ErrorMessage = "Debe estar entre {1} y {2}.")]
        public int Intensidad { get; set; } = 1;
    }
}
