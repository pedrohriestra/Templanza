using System.ComponentModel.DataAnnotations;

namespace Templanza.Models
{
    public class ItemOrden
    {
        public int Id { get; set; }

        [Required]
        public int OrdenId { get; set; }
        public Orden? Orden { get; set; }

        [Required]
        public int PlantaId { get; set; }
        public Planta? Planta { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [Range(0.01, 100000)]
        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }
    }
}
