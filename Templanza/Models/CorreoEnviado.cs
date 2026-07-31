using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class CorreoEnviado
    {
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Destinatario { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Asunto { get; set; } = string.Empty;

        public string? Cuerpo { get; set; }

        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        public bool Exito { get; set; }
    }
}
