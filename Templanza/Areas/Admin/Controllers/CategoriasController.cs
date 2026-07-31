using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador,Operador")]
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria is null) return NotFound();

            return View(categoria);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Categoría creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria is null) return NotFound();

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion")] Categoria categoria)
        {
            if (id != categoria.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Categoría actualizada correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Categorias.AnyAsync(c => c.Id == categoria.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var categoria = await _context.Categorias.FirstOrDefaultAsync(c => c.Id == id);
            if (categoria is null) return NotFound();

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria is not null)
            {
                try
                {
                    _context.Categorias.Remove(categoria);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Categoría eliminada correctamente.";
                }
                catch (DbUpdateException)
                {
                    TempData["Error"] = "No se puede eliminar la categoría porque tiene plantas o blends asociados.";
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
