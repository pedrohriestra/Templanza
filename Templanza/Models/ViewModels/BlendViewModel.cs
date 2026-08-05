using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Templanza.Models.ViewModels
{
    // Formulario de alta/edición de un Blend.
    public class BlendViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(150, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(1000, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        public List<SelectListItem> Categorias { get; set; } = new();

        public List<BlendPlantaItemViewModel> Plantas { get; set; } = new();
    }

    // Un renglón del checklist de plantas dentro del formulario de Blend.
    public class BlendPlantaItemViewModel
    {
        public int PlantaId { get; set; }
        public string NombrePlanta { get; set; } = string.Empty;
        public bool Seleccionado { get; set; }

        [Range(0.01, 10000, ErrorMessage = "Debe estar entre {1} y {2}.")]
        public decimal Cantidad { get; set; } = 1;

        [StringLength(30, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Unidad { get; set; } = string.Empty;
    }
}
