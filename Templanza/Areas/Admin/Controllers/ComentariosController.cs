using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdministradorOperador)]
    // Moderación de comentarios del foro (solo listar y borrar).
    public class ComentariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComentariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listado de comentarios de todo el foro.
        public async Task<IActionResult> Index()
        {
            var comentarios = await _context.Comentarios
                .AsNoTracking()
                .Include(c => c.Blend)
                .Include(c => c.Usuario)
                .OrderByDescending(c => c.FechaCreacion)
                .ToListAsync();

            return View(comentarios);
        }

        // Detalle de un comentario.
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var comentario = await _context.Comentarios
                .AsNoTracking()
                .Include(c => c.Blend)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comentario is null) return NotFound();

            return View(comentario);
        }

        // Pantalla de confirmación de borrado.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var comentario = await _context.Comentarios
                .AsNoTracking()
                .Include(c => c.Blend)
                .Include(c => c.Usuario)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comentario is null) return NotFound();

            return View(comentario);
        }

        // Borra el comentario moderado.
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
