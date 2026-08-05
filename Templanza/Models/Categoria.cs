using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    // Clasificación compartida por Plantas y Blends.
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(100, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Como máximo {1} caracteres.")]
        public string? Descripcion { get; set; }

        public ICollection<Planta> Plantas { get; set; } = new List<Planta>();
        public ICollection<Blend> Blends { get; set; } = new List<Blend>();
    }
}
