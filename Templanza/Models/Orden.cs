using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class Orden
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } = string.Empty;
        public ApplicationUser? Usuario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Total { get; set; }

        [Required]
        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

        public ICollection<ItemOrden> ItemOrdenes { get; set; } = new List<ItemOrden>();
    }
}
