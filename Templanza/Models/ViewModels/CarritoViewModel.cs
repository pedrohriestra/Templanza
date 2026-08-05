namespace Templanza.Models.ViewModels
{
    // Contenido del carrito armado a partir de la Sesión, para mostrar en pantalla.
    public class CarritoViewModel
    {
        public List<CarritoItemViewModel> Items { get; set; } = new();
        public decimal Total => Items.Sum(i => i.Subtotal);
    }

    // Un renglón del carrito con el precio/stock actual de la planta.
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
