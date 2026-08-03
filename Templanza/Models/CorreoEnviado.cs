using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class CorreoEnviado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(256, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Destinatario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(200, ErrorMessage = "Como máximo {1} caracteres.")]
        public string Asunto { get; set; } = string.Empty;

        public string? Cuerpo { get; set; }

        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        public bool Exito { get; set; }
    }
}
