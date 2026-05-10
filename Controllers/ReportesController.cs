using ClientReportManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientReportManager.Controllers
{
    [Authorize]
    public class ReportesController : Controller
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        public async Task<IActionResult> Clientes(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int? idEstadoCliente,
            int? idTipoCliente,
            string? buscar)
        {
            if (RangoFechasInvalido(fechaInicio, fechaFin))
            {
                ModelState.AddModelError(string.Empty, "La fecha de inicio no puede ser mayor que la fecha final.");
            }

            // El controlador recibe los filtros desde la pantalla y solicita el reporte al servicio.
            var modelo = await _reporteService.ObtenerReporteClientesAsync(
                fechaInicio,
                fechaFin,
                idEstadoCliente,
                idTipoCliente,
                buscar
            );

            return View(modelo);
        }

        public async Task<IActionResult> ExportarClientesCsv(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int? idEstadoCliente,
            int? idTipoCliente,
            string? buscar)
        {
            if (RangoFechasInvalido(fechaInicio, fechaFin))
            {
                TempData["ErrorMessage"] = "No se puede exportar porque la fecha de inicio es mayor que la fecha final.";

                return RedirectToAction(nameof(Clientes), new
                {
                    fechaInicio,
                    fechaFin,
                    idEstadoCliente,
                    idTipoCliente,
                    buscar
                });
            }

            var archivo = await _reporteService.GenerarCsvClientesAsync(
                fechaInicio,
                fechaFin,
                idEstadoCliente,
                idTipoCliente,
                buscar
            );

            var nombreArchivo = $"reporte-clientes-{DateTime.Now:yyyyMMdd-HHmm}.csv";

            // Se devuelve el archivo al navegador sin guardarlo físicamente en el servidor.
            return File(archivo, "text/csv; charset=utf-8", nombreArchivo);
        }

        private static bool RangoFechasInvalido(DateTime? fechaInicio, DateTime? fechaFin)
        {
            return fechaInicio.HasValue &&
                   fechaFin.HasValue &&
                   fechaInicio.Value.Date > fechaFin.Value.Date;
        }
    }
}