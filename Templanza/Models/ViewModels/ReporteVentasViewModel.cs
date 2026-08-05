using System.ComponentModel.DataAnnotations;

namespace Templanza.Models.ViewModels
{
    // Filtro de fechas + resultados del reporte de ventas.
    public class ReporteVentasViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [DataType(DataType.Date)]
        [Display(Name = "Desde")]
        public DateTime Desde { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [DataType(DataType.Date)]
        [Display(Name = "Hasta")]
        public DateTime Hasta { get; set; }

        public List<ReporteVentasItem> Items { get; set; } = new();

        public decimal TotalGeneral => Items.Sum(i => i.TotalVendido);
        public int UnidadesTotales => Items.Sum(i => i.CantidadVendida);
    }
}
