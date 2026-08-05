using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    // Línea de una Orden ya confirmada, con precio congelado.
    public class ItemOrden
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public int OrdenId { get; set; }
        public Orden? Orden { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        public int PlantaId { get; set; }
        public Planta? Planta { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe estar entre {1} y {2}.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Range(0.01, 100000, ErrorMessage = "Debe estar entre {1} y {2}.")]
        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }
    }
}
