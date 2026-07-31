using System.ComponentModel.DataAnnotations;

namespace Templanza.Models.ViewModels
{
    public class ReporteVentasViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Desde")]
        public DateTime Desde { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Hasta")]
        public DateTime Hasta { get; set; }

        public List<ReporteVentasItem> Items { get; set; } = new();

        public decimal TotalGeneral => Items.Sum(i => i.TotalVendido);
        public int UnidadesTotales => Items.Sum(i => i.CantidadVendida);
    }
}
