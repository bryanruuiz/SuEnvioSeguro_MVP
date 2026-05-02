using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SuEnvioSeguro.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMunicipios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TarifaBase = table.Column<float>(type: "real", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Municipios",
                columns: new[] { "Id", "Activo", "FechaCreacion", "Nombre", "TarifaBase" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medellín", 5000f },
                    { 2, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Envigado", 6000f },
                    { 3, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Itagüí", 6000f },
                    { 4, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sabaneta", 6500f },
                    { 5, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bello", 7000f },
                    { 6, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Caldas", 8000f },
                    { 7, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "La Estrella", 7000f },
                    { 8, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Copacabana", 8500f },
                    { 9, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Girardota", 9500f },
                    { 10, true, new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Barbosa", 10000f }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Municipios");
        }
    }
}
