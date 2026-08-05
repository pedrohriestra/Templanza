using Microsoft.AspNetCore.Mvc;
using Templanza.Services;

namespace Templanza.ViewComponents
{
    // Muestra en el navbar cuántos items hay en el carrito.
    public class CarritoBadgeViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var items = CarritoSesion.Obtener(HttpContext.Session);
            var cantidad = items.Sum(i => i.Cantidad);
            return View(cantidad);
        }
    }
}
