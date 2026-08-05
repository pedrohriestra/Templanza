using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    // Compra confirmada por un usuario, con su lista de ItemOrden.
    public class Orden
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public string UsuarioId { get; set; } = string.Empty;
        public ApplicationUser? Usuario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe estar entre {1} y {2}.")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

        public ICollection<ItemOrden> ItemOrdenes { get; set; } = new List<ItemOrden>();
    }
}
