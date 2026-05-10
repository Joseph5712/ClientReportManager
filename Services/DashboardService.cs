using ClientReportManager.Data;
using ClientReportManager.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ClientReportManager.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardViewModel> ObtenerResumenAsync()
        {
            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Se calculan las métricas principales del sistema.
            // Estas cifras permiten tener una lectura rápida del estado general de la cartera de clientes.
            var totalClientes = await _context.Clientes.CountAsync();

            var clientesActivos = await _context.Clientes
                .CountAsync(c => c.IdEstadoCliente == 1);

            var clientesInactivos = await _context.Clientes
                .CountAsync(c => c.IdEstadoCliente == 2);

            var clientesPotenciales = await _context.Clientes
                .CountAsync(c => c.IdEstadoCliente == 3);

            var clientesRegistradosMesActual = await _context.Clientes
                .CountAsync(c => c.FechaRegistro >= inicioMes);

            // Se cargan los últimos clientes para facilitar el seguimiento operativo.
            var ultimosClientes = await _context.Clientes
                .Include(c => c.EstadoCliente)
                .Include(c => c.TipoCliente)
                .OrderByDescending(c => c.FechaRegistro)
                .Take(5)
                .ToListAsync();

            // Se agrupan los clientes por estado para mostrar una distribución general.
            var clientesPorEstado = await _context.Clientes
                .Include(c => c.EstadoCliente)
                .GroupBy(c => c.EstadoCliente!.Nombre)
                .Select(g => new ResumenPorEstadoViewModel
                {
                    Estado = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(r => r.Total)
                .ToListAsync();

            // Se agrupan los clientes por tipo para identificar la composición de la cartera.
            var clientesPorTipo = await _context.Clientes
                .Include(c => c.TipoCliente)
                .GroupBy(c => c.TipoCliente!.Nombre)
                .Select(g => new ResumenPorTipoViewModel
                {
                    Tipo = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(r => r.Total)
                .ToListAsync();

            return new DashboardViewModel
            {
                TotalClientes = totalClientes,
                ClientesActivos = clientesActivos,
                ClientesInactivos = clientesInactivos,
                ClientesPotenciales = clientesPotenciales,
                ClientesRegistradosMesActual = clientesRegistradosMesActual,
                UltimosClientes = ultimosClientes,
                ClientesPorEstado = clientesPorEstado,
                ClientesPorTipo = clientesPorTipo
            };
        }
    }
}