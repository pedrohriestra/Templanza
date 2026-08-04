using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;
using Templanza.Models.ViewModels;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdministradorOperador)]
    public class PlantasController : Controller
    {
        private const int TamanioPagina = 5;

        private readonly ApplicationDbContext _context;

        public PlantasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Búsqueda y paginación resueltas a mano (sin DataTables), a pedido de la cátedra.
        public async Task<IActionResult> Index(string? busqueda, int pagina = 1)
        {
            var query = _context.Plantas
                .Include(p => p.Categoria)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
            {
                query = query.Where(p =>
                    p.NombreComun.Contains(busqueda) ||
                    p.NombreCientifico.Contains(busqueda));
            }

            var total = await query.CountAsync();
            var totalPaginas = (int)Math.Ceiling(total / (double)TamanioPagina);
            pagina = Math.Max(1, Math.Min(pagina, Math.Max(totalPaginas, 1)));

            var plantas = await query
                .OrderBy(p => p.NombreComun)
                .Skip((pagina - 1) * TamanioPagina)
                .Take(TamanioPagina)
                .ToListAsync();

            ViewBag.Busqueda = busqueda;
            ViewBag.Pagina = pagina;
            ViewBag.TotalPaginas = totalPaginas;

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

        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> Create()
        {
            var viewModel = new PlantaViewModel();
            await CargarListasAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.Administrador)]
        public async Task<IActionResult> Create(PlantaViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var planta = new Planta
                {
                    NombreComun = viewModel.NombreComun,
                    NombreCientifico = viewModel.NombreCientifico,
                    Contraindicaciones = viewModel.Contraindicaciones,
                    ParteUsada = viewModel.ParteUsada,
                    Origen = viewModel.Origen,
                    Precio = viewModel.Precio,
                    Stock = viewModel.Stock,
                    CategoriaId = viewModel.CategoriaId
                };

                foreach (var efecto in viewModel.Efectos.Where(e => e.Seleccionado))
                {
                    planta.PlantaEfectos.Add(new PlantaEfecto
                    {
                        EfectoId = efecto.EfectoId,
                        Intensidad = efecto.Intensidad
                    });
                }

                _context.Add(planta);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Planta creada correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarListasAsync(viewModel);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return NotFound();

            var planta = await _context.Plantas
                .Include(p => p.PlantaEfectos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (planta is null) return NotFound();

            var viewModel = new PlantaViewModel
            {
                Id = planta.Id,
                NombreComun = planta.NombreComun,
                NombreCientifico = planta.NombreCientifico,
                Contraindicaciones = planta.Contraindicaciones,
                ParteUsada = planta.ParteUsada,
                Origen = planta.Origen,
                Precio = planta.Precio,
                Stock = planta.Stock,
                CategoriaId = planta.CategoriaId
            };

            await CargarListasAsync(viewModel, planta.PlantaEfectos);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlantaViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var planta = await _context.Plantas
                    .Include(p => p.PlantaEfectos)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (planta is null) return NotFound();

                planta.NombreComun = viewModel.NombreComun;
                planta.NombreCientifico = viewModel.NombreCientifico;
                planta.Contraindicaciones = viewModel.Contraindicaciones;
                planta.ParteUsada = viewModel.ParteUsada;
                planta.Origen = viewModel.Origen;
                planta.Precio = viewModel.Precio;
                planta.Stock = viewModel.Stock;
                planta.CategoriaId = viewModel.CategoriaId;

                var seleccionados = viewModel.Efectos.Where(e => e.Seleccionado).ToList();
                var seleccionadosIds = seleccionados.Select(e => e.EfectoId).ToHashSet();

                planta.PlantaEfectos
                    .Where(pe => !seleccionadosIds.Contains(pe.EfectoId))
                    .ToList()
                    .ForEach(pe => planta.PlantaEfectos.Remove(pe));

                foreach (var efecto in seleccionados)
                {
                    var existente = planta.PlantaEfectos.FirstOrDefault(pe => pe.EfectoId == efecto.EfectoId);
                    if (existente is not null)
                    {
                        existente.Intensidad = efecto.Intensidad;
                    }
                    else
                    {
                        planta.PlantaEfectos.Add(new PlantaEfecto
                        {
                            EfectoId = efecto.EfectoId,
                            Intensidad = efecto.Intensidad
                        });
                    }
                }

                try
                {
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Planta actualizada correctamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Plantas.AnyAsync(p => p.Id == id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await CargarListasAsync(viewModel);
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();

            var planta = await _context.Plantas
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (planta is null) return NotFound();

            return View(planta);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var planta = await _context.Plantas.FindAsync(id);
            if (planta is not null)
            {
                try
                {
                    _context.Plantas.Remove(planta);
                    await _context.SaveChangesAsync();
                    TempData["Exito"] = "Planta eliminada correctamente.";
                }
                catch (DbUpdateException)
                {
                    TempData["Error"] = "No se puede eliminar la planta porque está asociada a blends u órdenes.";
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListasAsync(PlantaViewModel viewModel, ICollection<PlantaEfecto>? seleccionActual = null)
        {
            viewModel.Categorias = await _context.Categorias
                .OrderBy(c => c.Nombre)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre })
                .ToListAsync();

            var todosLosEfectos = await _context.Efectos
                .OrderBy(e => e.Nombre)
                .ToListAsync();

            viewModel.Efectos = todosLosEfectos.Select(e =>
            {
                var actual = seleccionActual?.FirstOrDefault(pe => pe.EfectoId == e.Id);
                return new PlantaEfectoItemViewModel
                {
                    EfectoId = e.Id,
                    NombreEfecto = e.Nombre,
                    Seleccionado = actual is not null,
                    Intensidad = actual?.Intensidad ?? 1
                };
            }).ToList();
        }
    }
}
