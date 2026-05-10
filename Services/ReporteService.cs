using ClientReportManager.Data;
using ClientReportManager.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ClientReportManager.Services
{
    public class ReporteService : IReporteService
    {
        private readonly ApplicationDbContext _context;

        public ReporteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReporteClientesViewModel> ObtenerReporteClientesAsync(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int? idEstadoCliente,
            int? idTipoCliente,
            string? buscar)
        {
            var query = _context.Clientes
                .AsQueryable();

            if (fechaInicio.HasValue)
            {
                var inicio = fechaInicio.Value.Date;

                // Se toma la fecha inicial desde las 00:00 para incluir todos los registros de ese día.
                query = query.Where(c => c.FechaRegistro >= inicio);
            }

            if (fechaFin.HasValue)
            {
                var finExclusivo = fechaFin.Value.Date.AddDays(1);

                // Se usa fecha final exclusiva para incluir todos los registros del día seleccionado.
                query = query.Where(c => c.FechaRegistro < finExclusivo);
            }

            if (idEstadoCliente.HasValue && idEstadoCliente.Value > 0)
            {
                query = query.Where(c => c.IdEstadoCliente == idEstadoCliente.Value);
            }

            if (idTipoCliente.HasValue && idTipoCliente.Value > 0)
            {
                query = query.Where(c => c.IdTipoCliente == idTipoCliente.Value);
            }

            if (!string.IsNullOrWhiteSpace(buscar))
            {
                var textoBusqueda = buscar.Trim();

                // La búsqueda cubre campos comunes para consultas administrativas.
                query = query.Where(c =>
                    c.NombreCompleto.Contains(textoBusqueda) ||
                    c.Identificacion.Contains(textoBusqueda) ||
                    (c.Correo != null && c.Correo.Contains(textoBusqueda)) ||
                    (c.Telefono != null && c.Telefono.Contains(textoBusqueda)) ||
                    (c.Empresa != null && c.Empresa.Contains(textoBusqueda))
                );
            }

            var clientes = await query
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();

            var estados = await _context.EstadosCliente
                .Where(e => e.Activo)
                .OrderBy(e => e.Nombre)
                .ToListAsync();

            var tipos = await _context.TiposCliente
                .Where(t => t.Activo)
                .OrderBy(t => t.Nombre)
                .ToListAsync();

            var estadosPorId = estados.ToDictionary(e => e.IdEstadoCliente, e => e.Nombre);
            var tiposPorId = tipos.ToDictionary(t => t.IdTipoCliente, t => t.Nombre);

            // Se asignan las propiedades de navegación manualmente para que la vista pueda mostrar los nombres.
            // Esto mantiene consistente el detalle del reporte aunque las relaciones no estén cargadas por Include.
            foreach (var cliente in clientes)
            {
                cliente.EstadoCliente = estados.FirstOrDefault(e => e.IdEstadoCliente == cliente.IdEstadoCliente);
                cliente.TipoCliente = tipos.FirstOrDefault(t => t.IdTipoCliente == cliente.IdTipoCliente);
            }

            // El resumen se calcula contra los catálogos usando los IDs del cliente.
            // Así evitamos que el reporte dependa de propiedades de navegación nulas.
            var resumenPorEstado = clientes
                .GroupBy(c => estadosPorId.ContainsKey(c.IdEstadoCliente)
                    ? estadosPorId[c.IdEstadoCliente]
                    : "Sin estado")
                .Select(g => new ResumenReporteItemViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(r => r.Total)
                .ToList();

            var resumenPorTipo = clientes
                .GroupBy(c => tiposPorId.ContainsKey(c.IdTipoCliente)
                    ? tiposPorId[c.IdTipoCliente]
                    : "Sin tipo")
                .Select(g => new ResumenReporteItemViewModel
                {
                    Nombre = g.Key,
                    Total = g.Count()
                })
                .OrderByDescending(r => r.Total)
                .ToList();

            return new ReporteClientesViewModel
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                IdEstadoCliente = idEstadoCliente,
                IdTipoCliente = idTipoCliente,
                Buscar = buscar,
                TotalClientes = clientes.Count,
                Clientes = clientes,
                EstadosCliente = estados,
                TiposCliente = tipos,
                ResumenPorEstado = resumenPorEstado,
                ResumenPorTipo = resumenPorTipo
            };
        }

        public async Task<byte[]> GenerarCsvClientesAsync(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            int? idEstadoCliente,
            int? idTipoCliente,
            string? buscar)
        {
            var reporte = await ObtenerReporteClientesAsync(
                fechaInicio,
                fechaFin,
                idEstadoCliente,
                idTipoCliente,
                buscar
            );

            var csv = new StringBuilder();

            // Se genera el encabezado del archivo con columnas entendibles para el usuario final.
            csv.AppendLine("Nombre completo;Identificación;Correo;Teléfono;Empresa;Tipo;Estado;Fecha registro;Observaciones");

            foreach (var cliente in reporte.Clientes)
            {
                csv.AppendLine(string.Join(";",
                    EscaparCsv(cliente.NombreCompleto),
                    EscaparCsv(cliente.Identificacion),
                    EscaparCsv(cliente.Correo),
                    EscaparCsv(cliente.Telefono),
                    EscaparCsv(cliente.Empresa),
                    EscaparCsv(cliente.TipoCliente?.Nombre),
                    EscaparCsv(cliente.EstadoCliente?.Nombre),
                    EscaparCsv(cliente.FechaRegistro.ToString("dd/MM/yyyy HH:mm")),
                    EscaparCsv(cliente.Observaciones)
                ));
            }

            // Se agrega BOM para mejorar compatibilidad al abrir el CSV directamente en Excel.
            var contenido = Encoding.UTF8.GetBytes(csv.ToString());
            var bom = Encoding.UTF8.GetPreamble();

            return bom.Concat(contenido).ToArray();
        }

        private static string EscaparCsv(string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                return string.Empty;
            }

            var texto = valor.Trim();

            // Como el separador es punto y coma, se escapan esos caracteres especiales.
            if (texto.Contains(";") || texto.Contains("\"") || texto.Contains("\n") || texto.Contains("\r"))
            {
                texto = texto.Replace("\"", "\"\"");
                return $"\"{texto}\"";
            }

            return texto;
        }
    }
}