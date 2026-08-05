using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;
using Templanza.Models.ViewModels;

namespace Templanza.Controllers
{
    // Foro público de blends: listado, detalle, creación, comentarios y likes.
    public class BlendsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlendsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string? UsuarioActualId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Foro comunitario: blends publicados y no recomendados.
        public async Task<IActionResult> Index()
        {
            var blends = await _context.Blends
                .Include(b => b.Categoria)
                .Include(b => b.Usuario)
                .Include(b => b.Comentarios)
                .Include(b => b.BlendLikes)
                .Where(b => b.EsPublicado && !b.EsRecomendado)
                .OrderByDescending(b => b.FechaCreacion)
                .ToListAsync();

            return View(blends);
        }

        // Vitrina de recetas oficiales curadas por el Admin.
        public async Task<IActionResult> Recomendados()
        {
            var blends = await _context.Blends
                .Include(b => b.Categoria)
                .Include(b => b.BlendPlantas)
                    .ThenInclude(bp => bp.Planta)
                .Where(b => b.EsRecomendado)
                .OrderBy(b => b.Nombre)
                .ToListAsync();

            return View(blends);
        }

        // Detalle de un blend, con receta, comentarios y likes.
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return NotFound();

            var blend = await _context.Blends
                .Include(b => b.Categoria)
                .Include(b => b.Usuario)
                .Include(b => b.BlendPlantas)
                    .ThenInclude(bp => bp.Planta)
                .Include(b => b.Comentarios)
                    .ThenInclude(c => c.Usuario)
                .Include(b => b.BlendLikes)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (blend is null) return NotFound();

            // Solo el autor o un moderador pueden ver un blend todavía no publicado.
            if (!blend.EsPublicado && blend.UsuarioId != UsuarioActualId && !User.IsInRole("Administrador") && !User.IsInRole("Operador"))
            {
                return NotFound();
            }

            ViewBag.YaLeDioLike = UsuarioActualId is not null && blend.BlendLikes.Any(l => l.UsuarioId == UsuarioActualId);
            return View(blend);
        }

        // Blends creados por el usuario logueado.
        [Authorize]
        public async Task<IActionResult> MisBlends()
        {
            var blends = await _context.Blends
                .Include(b => b.Categoria)
                .Where(b => b.UsuarioId == UsuarioActualId)
                .OrderByDescending(b => b.FechaCreacion)
                .ToListAsync();

            return View(blends);
        }

        // Formulario para armar un blend nuevo.
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var viewModel = new BlendViewModel();
            await CargarListasAsync(viewModel);
            return View(viewModel);
        }

        // Guarda el blend nuevo, pendiente de moderación.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlendViewModel viewModel)
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
                    EsPublicado = false,
                    EsRecomendado = false,
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
                TempData["Exito"] = "Tu blend fue enviado y quedará visible cuando un moderador lo apruebe.";
                return RedirectToAction(nameof(MisBlends));
            }

            await CargarListasAsync(viewModel);
            return View(viewModel);
        }

        // Agrega un comentario a un blend publicado.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comentar(int blendId, string texto)
        {
            var blend = await _context.Blends.FirstOrDefaultAsync(b => b.Id == blendId);
            if (blend is null || !blend.EsPublicado) return NotFound();

            if (!string.IsNullOrWhiteSpace(texto))
            {
                _context.Comentarios.Add(new Comentario
                {
                    BlendId = blendId,
                    UsuarioId = UsuarioActualId!,
                    Texto = texto.Trim(),
                    FechaCreacion = DateTime.Now
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = blendId });
        }

        // Da o saca el like del usuario actual (toggle).
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Like(int blendId)
        {
            var blend = await _context.Blends.FirstOrDefaultAsync(b => b.Id == blendId);
            if (blend is null || !blend.EsPublicado) return NotFound();

            var usuarioId = UsuarioActualId!;
            var like = await _context.BlendLikes
                .FirstOrDefaultAsync(l => l.BlendId == blendId && l.UsuarioId == usuarioId);

            if (like is null)
            {
                _context.BlendLikes.Add(new BlendLike { BlendId = blendId, UsuarioId = usuarioId });
            }
            else
            {
                _context.BlendLikes.Remove(like);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = blendId });
        }

        // Llena las listas de Categorías y Plantas para el formulario.
        private async Task CargarListasAsync(BlendViewModel viewModel)
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

                viewModel.Plantas = plantas.Select(p => new BlendPlantaItemViewModel
                {
                    PlantaId = p.Id,
                    NombrePlanta = p.NombreComun
                }).ToList();
            }
        }
    }
}
