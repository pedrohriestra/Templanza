using System.Security.Claims;
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
    [Authorize(Roles = "Administrador,Operador")]
    public class BlendsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlendsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string? UsuarioActualId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Recomendados: recetas oficiales curadas por el Admin.
        public async Task<IActionResult> Index()
        {
            var blends = await _context.Blends
                .Include(b => b.Categoria)
                .Where(b => b.EsRecomendado)
                .OrderBy(b => b.Nombre)
                .ToListAsync();

            return View(blends);
        }

        // Blends del foro comunitario a la espera de moderación.
        public async Task<IActionResult> Pendientes()
        {
            var blends = await _context.Blends
                .Include(b => b.Categoria)
                .Include(b => b.Usuario)
                .Include(b => b.BlendPlantas)
                    .ThenInclude(bp => bp.Planta)
                .Where(b => !b.EsPublicado && !b.EsRecomendado)
                .OrderBy(b => b.FechaCreacion)
                .ToListAsync();

            return View(blends);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(int id)
        {
            var blend = await _context.Blends.FindAsync(id);
            if (blend is not null)
            {
                blend.EsPublicado = true;
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Blend aprobado y publicado en el foro.";
            }
            return RedirectToAction(nameof(Pendientes));
        }

        public async Task<IActionResult> Rechazar(int? id)
        {
            if (id is null) return NotFound();

            var blend = await _context.Blends
                .Include(b => b.Usuario)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blend is null) return NotFound();

            return View(blend);
        }

        [HttpPost, ActionName("Rechazar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RechazarConfirmado(int id)
        {
            var blend = await _context.Blends.FindAsync(id);
            if (blend is not null)
            {
                _context.Blends.Remove(blend);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Blend rechazado y eliminado del foro.";
            }
            return RedirectToAction(nameof(Pendientes));
        }

        public async Task<IActionResult> CreateRecomendado()
        {
            var viewModel = new BlendViewModel();
            await CargarListasAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRecomendado(BlendViewModel viewModel)
        {
            if (!viewModel.Plantas.Any(p => p.Seleccionado))
            {
                ModelState.AddModelError(string.Empty, "Elegí al menos una planta para armar la receta.");
            }

            if (ModelState.IsValid)
            {
                var blend = new Blend
                {
                    Nombre = viewModel.Nombre,
                    Descripcion = viewModel.Descripcion,
                    CategoriaId = viewModel.CategoriaId,
                    UsuarioId = UsuarioActualId!,
                    EsPublicado = true,
                    EsRecomendado = true,
                    FechaCreacion = DateTime.Now
                };

                foreach (var planta in viewModel.Plantas.Where(p => p.Seleccionado))
                {
                    blend.BlendPlantas.Add(new BlendPlanta
                    {
                        PlantaId = planta.PlantaId,
                        Cantidad = planta.Cantidad,
                        Unidad = planta.Unidad
                    });
                }

                _context.Add(blend);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Blend recomendado creado y publicado.";
                return RedirectToAction(nameof(Index));
            }

            await CargarListasAsync(viewModel);
            return View(viewModel);
        }

        public async Task<IActionResult> EditRecomendado(int? id)
        {
            if (id is null) return NotFound();

            var blend = await _context.Blends
                .Include(b => b.BlendPlantas)
                .FirstOrDefaultAsync(b => b.Id == id && b.EsRecomendado);

            if (blend is null) return NotFound();

            var viewModel = new BlendViewModel
            {
                Id = blend.Id,
                Nombre = blend.Nombre,
                Descripcion = blend.Descripcion,
                CategoriaId = blend.CategoriaId
            };

            await CargarListasAsync(viewModel, blend.BlendPlantas);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecomendado(int id, BlendViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (!viewModel.Plantas.Any(p => p.Seleccionado))
            {
                ModelState.AddModelError(string.Empty, "Elegí al menos una planta para armar la receta.");
            }

            if (ModelState.IsValid)
            {
                var blend = await _context.Blends
                    .Include(b => b.BlendPlantas)
                    .FirstOrDefaultAsync(b => b.Id == id && b.EsRecomendado);

                if (blend is null) return NotFound();

                blend.Nombre = viewModel.Nombre;
                blend.Descripcion = viewModel.Descripcion;
                blend.CategoriaId = viewModel.CategoriaId;

                var seleccionadas = viewModel.Plantas.Where(p => p.Seleccionado).ToList();
                var seleccionadasIds = seleccionadas.Select(p => p.PlantaId).ToHashSet();

                blend.BlendPlantas
                    .Where(bp => !seleccionadasIds.Contains(bp.PlantaId))
                    .ToList()
                    .ForEach(bp => blend.BlendPlantas.Remove(bp));

                foreach (var planta in seleccionadas)
                {
                    var existente = blend.BlendPlantas.FirstOrDefault(bp => bp.PlantaId == planta.PlantaId);
                    if (existente is not null)
                    {
                        existente.Cantidad = planta.Cantidad;
                        existente.Unidad = planta.Unidad;
                    }
                    else
                    {
                        blend.BlendPlantas.Add(new BlendPlanta
                        {
                            PlantaId = planta.PlantaId,
                            Cantidad = planta.Cantidad,
                            Unidad = planta.Unidad
                        });
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Exito"] = "Blend recomendado actualizado.";
                return RedirectToAction(nameof(Index));
            }

            await CargarListasAsync(viewModel);
            return View(viewModel);
        }

        public async Task<IActionResult> DeleteRecomendado(int? id)
        {
            if (id is null) return NotFound();

            var blend = await _context.Blends
                .Include(b => b.Categoria)
                .FirstOrDefaultAsync(b => b.Id == id && b.EsRecomendado);

            if (blend is null) return NotFound();

            return View(blend);
        }

        [HttpPost, ActionName("DeleteRecomendado")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecomendadoConfirmado(int id)
        {
            var blend = await _context.Blends.FindAsync(id);
            if (blend is not null)
            {
                _context.Blends.Remove(blend);
                await _context.SaveChangesAsync();
                TempData["Exito"] = "Blend recomendado eliminado.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarListasAsync(BlendViewModel viewModel, ICollection<BlendPlanta>? seleccionActual = null)
        {
            viewModel.Categorias = await _context.Categorias
                .OrderBy(c => c.Nombre)
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre })
                .ToListAsync();

            if (viewModel.Plantas.Count == 0)
            {
                var plantas = await _context.Plantas
                    .OrderBy(p => p.NombreComun)
                    .ToListAsync();

                viewModel.Plantas = plantas.Select(p =>
                {
                    var actual = seleccionActual?.FirstOrDefault(bp => bp.PlantaId == p.Id);
                    return new BlendPlantaItemViewModel
                    {
                        PlantaId = p.Id,
                        NombrePlanta = p.NombreComun,
                        Seleccionado = actual is not null,
                        Cantidad = actual?.Cantidad ?? 1,
                        Unidad = actual?.Unidad ?? string.Empty
                    };
                }).ToList();
            }
        }
    }
}
