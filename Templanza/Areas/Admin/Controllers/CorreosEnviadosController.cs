using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrador,Operador")]
    public class CorreosEnviadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CorreosEnviadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var correos = await _context.CorreosEnviados
                .OrderByDescending(c => c.FechaEnvio)
                .ToListAsync();

            return View(correos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var correo = await _context.CorreosEnviados.FirstOrDefaultAsync(c => c.Id == id);
            if (correo is null) return NotFound();

            return View(correo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var correo = await _context.CorreosEnviados.FirstOrDefaultAsync(c => c.Id == id);
            if (correo is null) return NotFound();

            return View(correo);
        }

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
