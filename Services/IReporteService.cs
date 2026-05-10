using ClientReportManager.ViewModels;

namespace ClientReportManager.Services
{
    public interface IReporteService
    {
        Task<ReporteClientesViewModel> ObtenerReporteClientesAsync(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int? idEstadoCliente,
            int? idTipoCliente,
            string? buscar
        );

        Task<byte[]> GenerarCsvClientesAsync(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int? idEstadoCliente,
            int? idTipoCliente,
            string? buscar
        );
    }
}