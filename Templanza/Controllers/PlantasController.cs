using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;

namespace Templanza.Controllers
{
    public class PlantasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlantasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoriaId)
        {
            var query = _context.Plantas
                .Include(p => p.Categoria)
                .AsQueryable();

            if (categoriaId is not null)
            {
                query = query.Where(p => p.CategoriaId == categoriaId);
            }

            ViewBag.Categorias = await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync();
            ViewBag.CategoriaSeleccionada = categoriaId;

            var plantas = await query.OrderBy(p => p.NombreComun).ToListAsync();
            return View(plantas);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var planta = await _context.Plantas
                .Include(p => p.Categoria)
                .Include(p => p.PlantaEfectos)
                    .ThenInclude(pe => pe.Efecto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (planta is null) return NotFound();

            return View(planta);
        }
    }
}
