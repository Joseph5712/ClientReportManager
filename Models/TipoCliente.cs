using System.ComponentModel.DataAnnotations;

namespace ClientReportManager.Models
{
    public class TipoCliente
    {
        [Key]
        public int IdTipoCliente { get; set; }

        [Required(ErrorMessage = "El nombre del tipo de cliente es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;

        // Un tipo de cliente puede estar relacionado con muchos clientes.
        // Esta clasificación facilita filtros y reportes administrativos.
        public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    }
}