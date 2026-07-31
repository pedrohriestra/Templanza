using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public ICollection<Planta> Plantas { get; set; } = new List<Planta>();
        public ICollection<Blend> Blends { get; set; } = new List<Blend>();
    }
}
