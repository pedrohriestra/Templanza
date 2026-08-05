using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Administrador)]
    // Log de auditoría de los emails enviados por la app.
    public class CorreosEnviadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CorreosEnviadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listado de correos enviados, más recientes primero.
        public async Task<IActionResult> Index()
        {
            var correos = await _context.CorreosEnviados
                .AsNoTracking()
                .OrderByDescending(c => c.FechaEnvio)
                .ToListAsync();

            return View(correos);
        }

        // Detalle de un correo enviado.
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var correo = await _context.CorreosEnviados.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (correo is null) return NotFound();

            return View(correo);
        }

        // Pantalla de confirmación de borrado.
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var correo = await _context.CorreosEnviados.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (correo is null) return NotFound();

            return View(correo);
        }

        // Borra el registro del correo.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var correo = await _context.CorreosEnviados.FindAsync(id);
            if (correo is not null)
            {
                _context.CorreosEnviados.Remove(correo);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Registro de correo eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
