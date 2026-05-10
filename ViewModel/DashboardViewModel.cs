using ClientReportManager.Models;

namespace ClientReportManager.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalClientes { get; set; }

        public int ClientesActivos { get; set; }

        public int ClientesInactivos { get; set; }

        public int ClientesPotenciales { get; set; }

        public int ClientesRegistradosMesActual { get; set; }

        public List<Cliente> UltimosClientes { get; set; } = new List<Cliente>();

        public List<ResumenPorEstadoViewModel> ClientesPorEstado { get; set; } = new List<ResumenPorEstadoViewModel>();

        public List<ResumenPorTipoViewModel> ClientesPorTipo { get; set; } = new List<ResumenPorTipoViewModel>();
    }

    public class ResumenPorEstadoViewModel
    {
        public string Estado { get; set; } = string.Empty;

        public int Total { get; set; }
    }

    public class ResumenPorTipoViewModel
    {
        public string Tipo { get; set; } = string.Empty;

        public int Total { get; set; }
    }
}
