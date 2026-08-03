using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador,Operador")]
    public class ComentariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComentariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var comentarios = await _context.Comentarios
                .Include(c => c.Blend)
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();

            return View(comentarios);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var comentario = await _context.Comentarios
                .Include(c => c.Blend)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comentario is null) return NotFound();

            return View(comentario);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var comentario = await _context.Comentarios
                .Include(c => c.Blend)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comentario is null) return NotFound();

            return View(comentario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comentario = await _context.Comentarios.FindAsync(id);
            if (comentario is not null)
            {
                _context.Comentarios.Remove(comentario);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Comentario eliminado correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
