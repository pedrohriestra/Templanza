using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    // Relación Planta-Efecto con su intensidad (1 a 5).
    public class PlantaEfecto
    {
        public int PlantaId { get; set; }
        public Planta? Planta { get; set; }

        public int EfectoId { get; set; }
        public Efecto? Efecto { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Range(1, 5, ErrorMessage = "Debe estar entre {1} y {2}.")]
        [Display(Name = "Intensidad")]
        public int Intensidad { get; set; }
    }
}
