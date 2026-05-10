using ClientReportManager.Data;
using ClientReportManager.Models;
using ClientReportManager.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ClientReportManager.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClienteFiltroViewModel> ObtenerClientesAsync(string? buscar, int? idEstadoCliente, int? idTipoCliente)
        {
            var query = _context.Clientes
                .Include(c => c.EstadoCliente)
                .Include(c => c.TipoCliente)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var textoBusqueda = buscar.Trim();

                // La búsqueda se aplica sobre los campos más usados por un usuario administrativo.
                query = query.Where(c =>
                    c.NombreCompleto.Contains(textoBusqueda) ||
                    c.Identificacion.Contains(textoBusqueda) ||
                    (c.Correo != null && c.Correo.Contains(textoBusqueda)) ||
                    (c.Telefono != null && c.Telefono.Contains(textoBusqueda)) ||
                    (c.Empresa != null && c.Empresa.Contains(textoBusqueda))
                );
            }

            if (idEstadoCliente.HasValue && idEstadoCliente.Value > 0)
            {
                query = query.Where(c => c.IdEstadoCliente == idEstadoCliente.Value);
            }

            if (idTipoCliente.HasValue && idTipoCliente.Value > 0)
            {
                query = query.Where(c => c.IdTipoCliente == idTipoCliente.Value);
            }

            var clientes = await query
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();

            return new ClienteFiltroViewModel
            {
                Buscar = buscar,
                IdEstadoCliente = idEstadoCliente,
                IdTipoCliente = idTipoCliente,
                Clientes = clientes,
                EstadosCliente = await ObtenerEstadosActivosAsync(),
                TiposCliente = await ObtenerTiposActivosAsync()
            };
        }

        public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            return await _context.Clientes
                .Include(c => c.EstadoCliente)
                .Include(c => c.TipoCliente)
                .FirstOrDefaultAsync(c => c.IdCliente == id);
        }

        public async Task<List<EstadoCliente>> ObtenerEstadosActivosAsync()
        {
            return await _context.EstadosCliente
                .Where(e => e.Activo)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }

        public async Task<List<TipoCliente>> ObtenerTiposActivosAsync()
        {
            return await _context.TiposCliente
                .Where(t => t.Activo)
                .OrderBy(t => t.Nombre)
                .ToListAsync();
        }

        public async Task<bool> ExisteIdentificacionAsync(string identificacion, int? idClienteExcluir = null)
        {
            var query = _context.Clientes
                .Where(c => c.Identificacion == identificacion);

            if (idClienteExcluir.HasValue)
            {
                // Al editar, se excluye el cliente actual para no comparar el registro contra sí mismo.
                query = query.Where(c => c.IdCliente != idClienteExcluir.Value);
            }

            return await query.AnyAsync();
        }

        public async Task CrearClienteAsync(Cliente cliente)
        {
            // La fecha de registro se controla desde el sistema para mantener trazabilidad del alta.
            cliente.FechaRegistro = DateTime.Now;

            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ActualizarClienteAsync(Cliente cliente)
        {
            var clienteActual = await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == cliente.IdCliente);

            if (clienteActual == null)
            {
                return false;
            }

            // Se actualizan solo los campos editables del cliente.
            // La fecha de registro original se conserva como dato histórico.
            clienteActual.NombreCompleto = cliente.NombreCompleto;
            clienteActual.Identificacion = cliente.Identificacion;
            clienteActual.Correo = cliente.Correo;
            clienteActual.Telefono = cliente.Telefono;
            clienteActual.Empresa = cliente.Empresa;
            clienteActual.IdTipoCliente = cliente.IdTipoCliente;
            clienteActual.IdEstadoCliente = cliente.IdEstadoCliente;
            clienteActual.Observaciones = cliente.Observaciones;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DesactivarClienteAsync(int id)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (cliente == null)
            {
                return false;
            }

            // No se elimina el registro físicamente.
            // Se marca como inactivo para conservar historial y evitar pérdida de información.
            cliente.IdEstadoCliente = 2;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}