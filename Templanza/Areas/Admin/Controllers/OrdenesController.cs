using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador,Operador")]
    public class OrdenesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdenesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = await _context.Ordenes
                .Include(o => o.Usuario)
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return View(ordenes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var orden = await _context.Ordenes
                .Include(o => o.Usuario)
                .Include(o => o.ItemOrdenes)
                    .ThenInclude(io => io.Planta)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden is null) return NotFound();

            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, EstadoOrden estado)
        {
            var orden = await _context.Ordenes.FindAsync(id);
            if (orden is not null)
            {
                orden.Estado = estado;
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Estado de la orden actualizado.";
            }
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
