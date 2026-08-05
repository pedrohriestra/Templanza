using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Templanza.Models;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdministradorOperador)]
    // Dashboard del backoffice.
    public class HomeController : Controller
    {
        // Tarjetas de acceso a cada sección, filtradas según el rol.
        public IActionResult Index()
        {
            return View();
        }
    }
}
