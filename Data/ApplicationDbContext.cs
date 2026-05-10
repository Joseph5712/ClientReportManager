using ClientReportManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientReportManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<EstadoCliente> EstadosCliente { get; set; }
        public DbSet<TipoCliente> TiposCliente { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Se asignan nombres de tabla explícitos para mantener consistencia con la base de datos.
            modelBuilder.Entity<Cliente>().ToTable("Clientes");
            modelBuilder.Entity<EstadoCliente>().ToTable("EstadosCliente");
            modelBuilder.Entity<TipoCliente>().ToTable("TiposCliente");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");

            // La identificación del cliente debe ser única para evitar registros duplicados.
            modelBuilder.Entity<Cliente>()
                .HasIndex(c => c.Identificacion)
                .IsUnique();

            // El nombre de usuario debe ser único para evitar conflictos durante el inicio de sesión.
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreUsuario)
                .IsUnique();

            // Datos iniciales para los estados de cliente.
            modelBuilder.Entity<EstadoCliente>().HasData(
                new EstadoCliente { IdEstadoCliente = 1, Nombre = "Activo", Activo = true },
                new EstadoCliente { IdEstadoCliente = 2, Nombre = "Inactivo", Activo = true },
                new EstadoCliente { IdEstadoCliente = 3, Nombre = "Potencial", Activo = true }
            );

            // Datos iniciales para los tipos de cliente.
            modelBuilder.Entity<TipoCliente>().HasData(
                new TipoCliente { IdTipoCliente = 1, Nombre = "Individual", Activo = true },
                new TipoCliente { IdTipoCliente = 2, Nombre = "Empresa", Activo = true },
                new TipoCliente { IdTipoCliente = 3, Nombre = "Proveedor", Activo = true },
                new TipoCliente { IdTipoCliente = 4, Nombre = "Corporativo", Activo = true }
            );

            // Usuario administrativo inicial para ingresar al sistema durante la etapa de desarrollo.
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    IdUsuario = 1,
                    NombreCompleto = "Administrador del Sistema",
                    Correo = "admin@demo.com",
                    NombreUsuario = "admin",
                    Clave = "123456",
                    Activo = true,
                    FechaCreacion = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}