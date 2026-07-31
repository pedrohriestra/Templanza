namespace Templanza.Models
{
    public class ReporteVentasItem
    {
        public int PlantaId { get; set; }
        public string NombreComun { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalVendido { get; set; }
    }
}
