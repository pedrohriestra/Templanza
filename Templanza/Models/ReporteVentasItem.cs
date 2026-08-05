namespace Templanza.Models
{
    // Fila de resultado del stored procedure de reporte de ventas.
    public class ReporteVentasItem
    {
        public int PlantaId { get; set; }
        public string NombreComun { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalVendido { get; set; }
    }
}
