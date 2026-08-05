using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdministradorOperador)]
    // CRUD de Efectos (crear es solo Administrador; ver/editar/borrar, ambos roles).
    public class EfectosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EfectosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listado de efectos.
        public async Task<IActionResult> Index()
        {
            return View(await _context.Efectos.OrderBy(e => e.Nombre).ToListAsync());
        }

        // Ficha de un efecto.
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var efecto = await _context.Efectos.FirstOrDefaultAsync(e => e.Id == id);
            if (efecto is null) return NotFound();

            return View(efecto);
        }

        // Formulario de alta.
        [Authorize(Roles = Roles.Administrador)]
        public IActionResult Create()
        {
            return View();
        }

        // Guarda el efecto nuevo.
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

        // Formulario de edición.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var efecto = await _context.Efectos.FindAsync(id);
            if (efecto is null) return NotFound();

            return View(efecto);
        }

        // Guarda los cambios del efecto.
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

        // Pantalla de confirmación de borrado.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var efecto = await _context.Efectos.FirstOrDefaultAsync(e => e.Id == id);
            if (efecto is null) return NotFound();

            return View(efecto);
        }

        // Borra el efecto (falla controlado si está asociado a alguna planta).
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
