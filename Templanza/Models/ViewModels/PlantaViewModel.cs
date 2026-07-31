using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Templanza.Models.ViewModels
{
    public class PlantaViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        [Display(Name = "Nombre común")]
        public string NombreComun { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [Display(Name = "Nombre científico")]
        public string NombreCientifico { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Contraindicaciones { get; set; }

        [StringLength(100)]
        [Display(Name = "Parte usada")]
        public string? ParteUsada { get; set; }

        [StringLength(100)]
        public string? Origen { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Precio { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        public List<SelectListItem> Categorias { get; set; } = new();

        public List<PlantaEfectoItemViewModel> Efectos { get; set; } = new();
    }

    public class PlantaEfectoItemViewModel
    {
        public int EfectoId { get; set; }
        public string NombreEfecto { get; set; } = string.Empty;
        public bool Seleccionado { get; set; }

        [Range(1, 5)]
        public int Intensidad { get; set; } = 1;
    }
}
