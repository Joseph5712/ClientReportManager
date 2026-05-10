using ClientReportManager.Models;

namespace ClientReportManager.Services
{
    public interface IUsuarioService
    {
        Task<Usuario?> ValidarCredencialesAsync(string nombreUsuario, string clave);
    }
}