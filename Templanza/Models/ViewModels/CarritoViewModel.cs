namespace Templanza.Models.ViewModels
{
    public class CarritoViewModel
    {
        public List<CarritoItemViewModel> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Subtotal);
    }

    public class CarritoItemViewModel
    {
        public int PlantaId { get; set; }
        public string NombreComun { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public int StockDisponible { get; set; }
        public decimal Subtotal => Precio * Cantidad;
    }
}
