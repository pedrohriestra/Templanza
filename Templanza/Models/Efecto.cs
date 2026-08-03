using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class Efecto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(100, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Como máximo {1} caracteres.")]
        public string? Descripcion { get; set; }

        public ICollection<PlantaEfecto> PlantaEfectos { get; set; } = new List<PlantaEfecto>();
    }
}
