using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Templanza.Models;

namespace Templanza.Services
{
    public static class CarritoSesion
    {
        private const string ClaveSesion = "Carrito";

        public static List<ItemCarrito> Obtener(ISession session)
        {
            var json = session.GetString(ClaveSesion);
            if (string.IsNullOrEmpty(json)) return new List<ItemCarrito>();
            return JsonSerializer.Deserialize<List<ItemCarrito>>(json) ?? new List<ItemCarrito>();
        }

        private static void Guardar(ISession session, List<ItemCarrito> items)
        {
            session.SetString(ClaveSesion, JsonSerializer.Serialize(items));
        }

        public static void Agregar(ISession session, int plantaId, int cantidad)
        {
            var items = Obtener(session);
            var item = items.FirstOrDefault(i => i.PlantaId == plantaId);
            if (item is null)
            {
                items.Add(new ItemCarrito { PlantaId = plantaId, Cantidad = cantidad });
            }
            else
            {
                item.Cantidad += cantidad;
            }
            Guardar(session, items);
        }

        public static void ActualizarCantidad(ISession session, int plantaId, int cantidad)
        {
            var items = Obtener(session);
            var item = items.FirstOrDefault(i => i.PlantaId == plantaId);
            if (item is not null)
            {
                item.Cantidad = cantidad;
            }
            Guardar(session, items);
        }

        public static void Eliminar(ISession session, int plantaId)
        {
            var items = Obtener(session);
            items.RemoveAll(i => i.PlantaId == plantaId);
            Guardar(session, items);
        }

        public static void Vaciar(ISession session)
        {
            session.Remove(ClaveSesion);
        }
    }
}
