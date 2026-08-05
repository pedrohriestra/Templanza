namespace Templanza.Models
{
    // Línea del carrito, guardada en la Sesión (no en la base).
    public class ItemCarrito
    {
        public int PlantaId { get; set; }
        public int Cantidad { get; set; }
    }
}
