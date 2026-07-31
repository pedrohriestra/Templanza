using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;
using Templanza.Models.ViewModels;
using Templanza.Services;

namespace Templanza.Controllers
{
    [Authorize]
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CarritoController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string UsuarioActualId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public async Task<IActionResult> Index()
        {
            var viewModel = await ConstruirViewModelAsync();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(int plantaId, int cantidad)
        {
            var planta = await _context.Plantas.FindAsync(plantaId);
            if (planta is null) return NotFound();

            if (cantidad < 1) cantidad = 1;
            if (cantidad > planta.Stock) cantidad = planta.Stock;

            if (cantidad > 0)
            {
                CarritoSesion.Agregar(HttpContext.Session, plantaId, cantidad);
                TempData["Exito"] = $"{planta.NombreComun} agregada al carrito.";
            }
            else
            {
                TempData["Error"] = "Esa planta no tiene stock disponible.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ActualizarCantidad(int plantaId, int cantidad)
        {
            if (cantidad < 1)
            {
                CarritoSesion.Eliminar(HttpContext.Session, plantaId);
            }
            else
            {
                CarritoSesion.ActualizarCantidad(HttpContext.Session, plantaId, cantidad);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int plantaId)
        {
            CarritoSesion.Eliminar(HttpContext.Session, plantaId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmar()
        {
            var items = CarritoSesion.Obtener(HttpContext.Session);
            if (items.Count == 0)
            {
                TempData["Error"] = "Tu carrito está vacío.";
                return RedirectToAction(nameof(Index));
            }

            var plantaIds = items.Select(i => i.PlantaId).ToList();
            var plantas = await _context.Plantas
                .Where(p => plantaIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            foreach (var item in items)
            {
                if (!plantas.TryGetValue(item.PlantaId, out var planta) || planta.Stock < item.Cantidad)
                {
                    TempData["Error"] = $"No hay stock suficiente para completar la compra. Revisá tu carrito.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var orden = new Orden
            {
                UsuarioId = UsuarioActualId,
                FechaCreacion = DateTime.Now,
                Estado = EstadoOrden.Confirmada
            };

            foreach (var item in items)
            {
                var planta = plantas[item.PlantaId];
                orden.ItemOrdenes.Add(new ItemOrden
                {
                    PlantaId = planta.Id,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = planta.Precio
                });
                planta.Stock -= item.Cantidad;
            }

            orden.Total = orden.ItemOrdenes.Sum(io => io.Cantidad * io.PrecioUnitario);

            _context.Ordenes.Add(orden);
            await _context.SaveChangesAsync();

            CarritoSesion.Vaciar(HttpContext.Session);
            TempData["Exito"] = "¡Compra confirmada! Gracias por tu pedido.";
            return RedirectToAction(nameof(DetalleOrden), new { id = orden.Id });
        }

        public async Task<IActionResult> MisOrdenes()
        {
            var ordenes = await _context.Ordenes
                .Where(o => o.UsuarioId == UsuarioActualId)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return View(ordenes);
        }

        public async Task<IActionResult> DetalleOrden(int id)
        {
            var orden = await _context.Ordenes
                .Include(o => o.ItemOrdenes)
                    .ThenInclude(io => io.Planta)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden is null) return NotFound();

            if (orden.UsuarioId != UsuarioActualId && !User.IsInRole("Administrador") && !User.IsInRole("Operador"))
            {
                return Forbid();
            }

            return View(orden);
        }

        private async Task<CarritoViewModel> ConstruirViewModelAsync()
        {
            var items = CarritoSesion.Obtener(HttpContext.Session);
            var viewModel = new CarritoViewModel();

            if (items.Count == 0) return viewModel;

            var plantaIds = items.Select(i => i.PlantaId).ToList();
            var plantas = await _context.Plantas
                .Where(p => plantaIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            foreach (var item in items)
            {
                if (!plantas.TryGetValue(item.PlantaId, out var planta)) continue;

                viewModel.Items.Add(new CarritoItemViewModel
                {
                    PlantaId = planta.Id,
                    NombreComun = planta.NombreComun,
                    Precio = planta.Precio,
                    Cantidad = item.Cantidad,
                    StockDisponible = planta.Stock
                });
            }

            return viewModel;
        }
    }
}
