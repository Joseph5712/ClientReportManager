using System.ComponentModel.DataAnnotations;

namespace ClientReportManager.Models
{
    public class Cliente
    {
        [Key]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre completo no puede superar los 150 caracteres.")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La identificación es obligatoria.")]
        [StringLength(30, ErrorMessage = "La identificación no puede superar los 30 caracteres.")]
        public string Identificacion { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        [StringLength(120, ErrorMessage = "El correo electrónico no puede superar los 120 caracteres.")]
        public string? Correo { get; set; }

        [StringLength(30, ErrorMessage = "El teléfono no puede superar los 30 caracteres.")]
        public string? Telefono { get; set; }

        [StringLength(120, ErrorMessage = "La empresa no puede superar los 120 caracteres.")]
        public string? Empresa { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de cliente.")]
        public int IdTipoCliente { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un estado.")]
        public int IdEstadoCliente { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [StringLength(500, ErrorMessage = "Las observaciones no pueden superar los 500 caracteres.")]
        public string? Observaciones { get; set; }

        // Relación con el catálogo de tipos de cliente.
        // Se utiliza para mostrar el nombre del tipo en listados y reportes.
        public TipoCliente? TipoCliente { get; set; }

        // Relación con el catálogo de estados de cliente.
        // Permite filtrar clientes por estado sin duplicar información.
        public EstadoCliente? EstadoCliente { get; set; }
    }
}