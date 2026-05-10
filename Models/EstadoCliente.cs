using System.ComponentModel.DataAnnotations;

namespace ClientReportManager.Models
{
    public class EstadoCliente
    {
        [Key]
        public int IdEstadoCliente { get; set; }

        [Required(ErrorMessage = "El nombre del estado es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        // Un estado puede estar asignado a muchos clientes.
        // Esta relación evita guardar textos repetidos como "Activo" o "Inactivo" en cada cliente.
        public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    }
}