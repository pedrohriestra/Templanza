using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;
using Templanza.Models.ViewModels;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Administrador)]
    public class UsuariosController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UsuariosController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.OrderBy(u => u.Email).ToListAsync();

            var rolesPorUsuario = await (from ur in _context.UserRoles
                                          join r in _context.Roles on ur.RoleId equals r.Id
                                          select new { ur.UserId, r.Name })
                .ToListAsync();

            var rolesPorUsuarioId = rolesPorUsuario
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => string.Join(", ", g.Select(x => x.Name)));

            var lista = usuarios.Select(usuario => new UsuarioListItemViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email ?? string.Empty,
                Roles = rolesPorUsuarioId.TryGetValue(usuario.Id, out var roles) ? roles : string.Empty,
                EmailConfirmed = usuario.EmailConfirmed
            }).ToList();

            return View(lista);
        }

        public async Task<IActionResult> Details(string? id)
        {
            if (id is null) return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario is null) return NotFound();

            ViewBag.Roles = await _userManager.GetRolesAsync(usuario);
            return View(usuario);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new UsuarioViewModel();
            await CargarRolesAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Password))
            {
                ModelState.AddModelError(nameof(viewModel.Password), "La contraseña es obligatoria para crear un usuario.");
            }

            if (!await _roleManager.RoleExistsAsync(viewModel.Rol))
            {
                ModelState.AddModelError(nameof(viewModel.Rol), "El rol seleccionado no es válido.");
            }

            if (ModelState.IsValid)
            {
                var usuario = new ApplicationUser
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    Nombre = viewModel.Nombre,
                    EmailConfirmed = true
                };

                var resultado = await _userManager.CreateAsync(usuario, viewModel.Password!);
                if (resultado.Succeeded)
                {
                    var resultadoRol = await _userManager.AddToRoleAsync(usuario, viewModel.Rol);
                    if (resultadoRol.Succeeded)
                    {
                        TempData["Exito"] = "Usuario creado correctamente.";
                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in resultadoRol.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    foreach (var error in resultado.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            await CargarRolesAsync(viewModel);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id is null) return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario is null) return NotFound();

            var roles = await _userManager.GetRolesAsync(usuario);
            var viewModel = new UsuarioViewModel
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email ?? string.Empty,
                Rol = roles.FirstOrDefault() ?? string.Empty
            };

            await CargarRolesAsync(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UsuarioViewModel viewModel)
        {
            if (id != viewModel.Id) return NotFound();

            if (!string.IsNullOrWhiteSpace(viewModel.Password) && viewModel.Password.Length < 6)
            {
                ModelState.AddModelError(nameof(viewModel.Password), "La contraseña debe tener al menos 6 caracteres.");
            }

            if (!await _roleManager.RoleExistsAsync(viewModel.Rol))
            {
                ModelState.AddModelError(nameof(viewModel.Rol), "El rol seleccionado no es válido.");
            }

            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByIdAsync(id);
                if (usuario is null) return NotFound();

                usuario.Nombre = viewModel.Nombre;

                if (!string.Equals(usuario.Email, viewModel.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var resultadoEmail = await _userManager.SetEmailAsync(usuario, viewModel.Email);
                    var resultadoUserName = resultadoEmail.Succeeded
                        ? await _userManager.SetUserNameAsync(usuario, viewModel.Email)
                        : IdentityResult.Success;

                    if (!resultadoEmail.Succeeded || !resultadoUserName.Succeeded)
                    {
                        foreach (var error in resultadoEmail.Errors.Concat(resultadoUserName.Errors))
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        await CargarRolesAsync(viewModel);
                        return View(viewModel);
                    }
                }
                else
                {
                    await _userManager.UpdateAsync(usuario);
                }

                if (!string.IsNullOrWhiteSpace(viewModel.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                    var resultadoPassword = await _userManager.ResetPasswordAsync(usuario, token, viewModel.Password);
                    if (!resultadoPassword.Succeeded)
                    {
                        foreach (var error in resultadoPassword.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        await CargarRolesAsync(viewModel);
                        return View(viewModel);
                    }
                }

                var rolesActuales = await _userManager.GetRolesAsync(usuario);
                if (!rolesActuales.Contains(viewModel.Rol))
                {
                    if (rolesActuales.Any())
                    {
                        var resultadoQuitarRoles = await _userManager.RemoveFromRolesAsync(usuario, rolesActuales);
                        if (!resultadoQuitarRoles.Succeeded)
                        {
                            foreach (var error in resultadoQuitarRoles.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                            await CargarRolesAsync(viewModel);
                            return View(viewModel);
                        }
                    }

                    var resultadoRol = await _userManager.AddToRoleAsync(usuario, viewModel.Rol);
                    if (!resultadoRol.Succeeded)
                    {
                        foreach (var error in resultadoRol.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        await CargarRolesAsync(viewModel);
                        return View(viewModel);
                    }
                }

                TempData["Exito"] = "Usuario actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarRolesAsync(viewModel);
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id is null) return NotFound();

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario is null) return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "No podés eliminar tu propia cuenta.";
                return RedirectToAction(nameof(Index));
            }

            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario is not null)
            {
                await _userManager.DeleteAsync(usuario);
                TempData["Exito"] = "Usuario eliminado correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task CargarRolesAsync(UsuarioViewModel viewModel)
        {
            viewModel.Roles = await _roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => new SelectListItem { Value = r.Name, Text = r.Name })
                .ToListAsync();
        }
    }
}
