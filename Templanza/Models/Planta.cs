using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class Planta
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
        public Categoria? Categoria { get; set; }

        public ICollection<PlantaEfecto> PlantaEfectos { get; set; } = new List<PlantaEfecto>();
        public ICollection<BlendPlanta> BlendPlantas { get; set; } = new List<BlendPlanta>();
        public ICollection<ItemOrden> ItemOrdenes { get; set; } = new List<ItemOrden>();
    }
}
