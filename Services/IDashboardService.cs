using ClientReportManager.ViewModels;

namespace ClientReportManager.Services
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> ObtenerResumenAsync();
    }
}