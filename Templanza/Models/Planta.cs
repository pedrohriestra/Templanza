using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class Planta
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
        public Categoria? Categoria { get; set; }

        public ICollection<PlantaEfecto> PlantaEfectos { get; set; } = new List<PlantaEfecto>();
        public ICollection<BlendPlanta> BlendPlantas { get; set; } = new List<BlendPlanta>();
        public ICollection<ItemOrden> ItemOrdenes { get; set; } = new List<ItemOrden>();
    }
}
