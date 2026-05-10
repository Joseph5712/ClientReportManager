using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ClientReportManager.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EstadosCliente",
                columns: table => new
                {
                    IdEstadoCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCliente", x => x.IdEstadoCliente);
                });

            migrationBuilder.CreateTable(
                name: "TiposCliente",
                columns: table => new
                {
                    IdTipoCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposCliente", x => x.IdTipoCliente);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Identificacion = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Empresa = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    IdTipoCliente = table.Column<int>(type: "int", nullable: false),
                    IdEstadoCliente = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TipoClienteIdTipoCliente = table.Column<int>(type: "int", nullable: true),
                    EstadoClienteIdEstadoCliente = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.IdCliente);
                    table.ForeignKey(
                        name: "FK_Clientes_EstadosCliente_EstadoClienteIdEstadoCliente",
                        column: x => x.EstadoClienteIdEstadoCliente,
                        principalTable: "EstadosCliente",
                        principalColumn: "IdEstadoCliente");
                    table.ForeignKey(
                        name: "FK_Clientes_TiposCliente_TipoClienteIdTipoCliente",
                        column: x => x.TipoClienteIdTipoCliente,
                        principalTable: "TiposCliente",
                        principalColumn: "IdTipoCliente");
                });

            migrationBuilder.InsertData(
                table: "EstadosCliente",
                columns: new[] { "IdEstadoCliente", "Activo", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Activo" },
                    { 2, true, "Inactivo" },
                    { 3, true, "Potencial" }
                });

            migrationBuilder.InsertData(
                table: "TiposCliente",
                columns: new[] { "IdTipoCliente", "Activo", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Individual" },
                    { 2, true, "Empresa" },
                    { 3, true, "Proveedor" },
                    { 4, true, "Corporativo" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "IdUsuario", "Activo", "Clave", "Correo", "FechaCreacion", "NombreCompleto", "NombreUsuario" },
                values: new object[] { 1, true, "123456", "admin@demo.com", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Administrador del Sistema", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_EstadoClienteIdEstadoCliente",
                table: "Clientes",
                column: "EstadoClienteIdEstadoCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Identificacion",
                table: "Clientes",
                column: "Identificacion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_TipoClienteIdTipoCliente",
                table: "Clientes",
                column: "TipoClienteIdTipoCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_NombreUsuario",
                table: "Usuarios",
                column: "NombreUsuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "EstadosCliente");

            migrationBuilder.DropTable(
                name: "TiposCliente");
        }
    }
}
