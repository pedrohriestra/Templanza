using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    // Línea de receta: qué planta y cuánta cantidad lleva un blend.
    public class BlendPlanta
    {
        public int BlendId { get; set; }
        public Blend? Blend { get; set; }

        public int PlantaId { get; set; }
        public Planta? Planta { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Range(0.01, 10000, ErrorMessage = "Debe estar entre {1} y {2}.")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(30, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Unidad { get; set; } = string.Empty;
    }
}
