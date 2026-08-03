using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Templanza.Data;
using Templanza.Models;
using Templanza.Models.ViewModels;

namespace Templanza.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.AdministradorOperador)]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> VentasPorRango(DateTime? desde, DateTime? hasta)
        {
            var hoy = DateTime.Today;
            var fechaDesde = desde ?? hoy.AddDays(-30);
            var fechaHasta = hasta ?? hoy;

            var items = await _context.ReporteVentas
                .FromSqlInterpolated($"EXEC dbo.ReporteVentasPorRango @FechaInicio = {fechaDesde}, @FechaFin = {fechaHasta}")
                .ToListAsync();

            var viewModel = new ReporteVentasViewModel
            {
                Desde = fechaDesde,
                Hasta = fechaHasta,
                Items = items
            };

            return View(viewModel);
        }
    }
}
