using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    // Comentario de un usuario sobre un blend publicado.
    public class Comentario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public int BlendId { get; set; }
        public Blend? Blend { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string UsuarioId { get; set; } = string.Empty;
        public ApplicationUser? Usuario { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(1000, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Texto { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
