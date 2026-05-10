using ClientReportManager.Models;

namespace ClientReportManager.ViewModel
{
    public class ClienteFiltroViewModel
    {
        public string? Buscar { get; set; }

        public int? IdEstadoCliente { get; set; }

        public int? IdTipoCliente { get; set; }

        public List<Cliente> Clientes { get; set; } = new List<Cliente>();

        public List<EstadoCliente> EstadosCliente { get; set; } = new List<EstadoCliente>();

        public List<TipoCliente> TiposCliente { get; set; } = new List<TipoCliente>();
    }
}