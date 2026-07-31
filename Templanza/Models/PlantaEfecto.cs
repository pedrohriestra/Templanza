using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class PlantaEfecto
    {
        public int PlantaId { get; set; }
        public Planta? Planta { get; set; }

        public int EfectoId { get; set; }
        public Efecto? Efecto { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name = "Intensidad")]
        public int Intensidad { get; set; }
    }
}
