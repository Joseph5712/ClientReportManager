using ClientReportManager.Models;
using ClientReportManager.ViewModel;

namespace ClientReportManager.Services
{
    public interface IClienteService
    {
        Task<ClienteFiltroViewModel> ObtenerClientesAsync(string? buscar, int? idEstadoCliente, int? idTipoCliente);

        Task<Cliente?> ObtenerClientePorIdAsync(int id);

        Task<List<EstadoCliente>> ObtenerEstadosActivosAsync();

        Task<List<TipoCliente>> ObtenerTiposActivosAsync();

        Task<bool> ExisteIdentificacionAsync(string identificacion, int? idClienteExcluir = null);

        Task CrearClienteAsync(Cliente cliente);

        Task<bool> ActualizarClienteAsync(Cliente cliente);

        Task<bool> DesactivarClienteAsync(int id);

        Task<bool> ActivarClienteAsync(int id);

        Task<bool> EliminarClienteFisicoAsync(int id);
    }
}