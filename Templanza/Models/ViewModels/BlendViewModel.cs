using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Templanza.Models.ViewModels
{
    public class BlendViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Categoría")]
        public int CategoriaId { get; set; }

        public List<SelectListItem> Categorias { get; set; } = new();

        public List<BlendPlantaItemViewModel> Plantas { get; set; } = new();
    }

    public class BlendPlantaItemViewModel
    {
        public int PlantaId { get; set; }
        public string NombrePlanta { get; set; } = string.Empty;
        public bool Seleccionado { get; set; }

        [Range(0.01, 10000)]
        public decimal Cantidad { get; set; } = 1;

        [StringLength(30)]
        public string Unidad { get; set; } = string.Empty;
    }
}
