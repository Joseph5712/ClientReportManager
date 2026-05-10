using ClientReportManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ClientReportManager.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            // El controlador solicita el resumen al servicio y entrega el modelo a la vista.
            // La lógica de consultas se mantiene fuera del controlador para facilitar mantenimiento.
            var modelo = await _dashboardService.ObtenerResumenAsync();

            return View(modelo);
        }
    }
}