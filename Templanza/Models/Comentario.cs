using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        [Required]
        public int BlendId { get; set; }
        public Blend? Blend { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;
        public ApplicationUser? Usuario { get; set; }

        [Required]
        [StringLength(1000)]
        public string Texto { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
