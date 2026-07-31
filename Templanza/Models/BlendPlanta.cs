using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class BlendPlanta
    {
        public int BlendId { get; set; }
        public Blend? Blend { get; set; }

        public int PlantaId { get; set; }
        public Planta? Planta { get; set; }

        [Required]
        [Range(0.01, 10000)]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Required]
        [StringLength(30)]
        public string Unidad { get; set; } = string.Empty;
    }
}
