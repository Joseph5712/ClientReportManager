using ClientReportManager.Models;

namespace ClientReportManager.ViewModels
{
    public class ReporteClientesViewModel
    {
        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public int? IdEstadoCliente { get; set; }

        public int? IdTipoCliente { get; set; }

        public string? Buscar { get; set; }

        public int TotalClientes { get; set; }

        public List<Cliente> Clientes { get; set; } = new List<Cliente>();

        public List<EstadoCliente> EstadosCliente { get; set; } = new List<EstadoCliente>();

        public List<TipoCliente> TiposCliente { get; set; } = new List<TipoCliente>();

        public List<ResumenReporteItemViewModel> ResumenPorEstado { get; set; } = new List<ResumenReporteItemViewModel>();

        public List<ResumenReporteItemViewModel> ResumenPorTipo { get; set; } = new List<ResumenReporteItemViewModel>();
    }

    public class ResumenReporteItemViewModel
    {
        public string Nombre { get; set; } = string.Empty;

        public int Total { get; set; }
    }
}