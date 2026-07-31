using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class Efecto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public ICollection<PlantaEfecto> PlantaEfectos { get; set; } = new List<PlantaEfecto>();
    }
}
