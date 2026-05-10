using ClientReportManager.Data;
using ClientReportManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClientReportManager.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioService(ApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<Usuario?> ValidarCredencialesAsync(string nombreUsuario, string clave)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.NombreUsuario == nombreUsuario &&
                    u.Activo);

            if (usuario == null)
            {
                return null;
            }

            // Compatibilidad inicial para el usuario semilla creado durante desarrollo.
            // Si la clave todavía está en texto plano y coincide, se actualiza a formato hash.
            if (usuario.Clave == clave)
            {
                usuario.Clave = _passwordHasher.HashPassword(usuario, clave);
                await _context.SaveChangesAsync();

                return usuario;
            }

            // Validación normal para contraseñas almacenadas con hash.
            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.Clave, clave);

            if (resultado == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return usuario;
        }
    }
}