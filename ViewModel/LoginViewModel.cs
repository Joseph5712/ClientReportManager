using System.ComponentModel.DataAnnotations;

namespace ClientReportManager.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100)]
        public string Clave { get; set; } = string.Empty;

        public bool Recordarme { get; set; }

        public string? ReturnUrl { get; set; }
    }
}