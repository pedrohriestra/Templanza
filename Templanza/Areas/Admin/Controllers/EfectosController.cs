using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdministradorOperador)]
    public class EfectosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EfectosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Efectos.OrderBy(e => e.Nombre).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var efecto = await _context.Efectos.FirstOrDefaultAsync(e => e.Id == id);
            if (efecto is null) return NotFound();

            return View(efecto);
        }

        [Authorize(Roles = Roles.Administrador)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion")] Efecto efecto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(efecto);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Efecto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(efecto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var efecto = await _context.Efectos.FindAsync(id);
            if (efecto is null) return NotFound();

            return View(efecto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion")] Efecto efecto)
        {
            if (id != efecto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(efecto);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Efecto actualizado correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Efectos.AnyAsync(e => e.Id == efecto.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(efecto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var efecto = await _context.Efectos.FirstOrDefaultAsync(e => e.Id == id);
            if (efecto is null) return NotFound();

            return View(efecto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var efecto = await _context.Efectos.FindAsync(id);
            if (efecto is not null)
            {
                try
                {
                    _context.Efectos.Remove(efecto);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Efecto eliminado correctamente.";
                }
                catch (DbUpdateException)
                {
                    TempData["Error"] = "No se puede eliminar el efecto porque está asociado a alguna planta.";
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
