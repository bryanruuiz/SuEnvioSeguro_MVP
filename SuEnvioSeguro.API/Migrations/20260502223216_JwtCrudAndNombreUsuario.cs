using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuEnvioSeguro.API.Migrations
{
    /// <inheritdoc />
    public partial class JwtCrudAndNombreUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Personas_IdUsuario",
                table: "Personas");

            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Personas",
                newName: "NombreUsuario");

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 5,
                column: "TarifaBase",
                value: 6500f);

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 6,
                column: "TarifaBase",
                value: 7500f);

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 7,
                column: "TarifaBase",
                value: 6500f);

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 8,
                column: "TarifaBase",
                value: 7500f);

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 9,
                column: "TarifaBase",
                value: 8000f);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_NombreUsuario",
                table: "Personas",
                column: "NombreUsuario",
                unique: true,
                filter: "[NombreUsuario] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Personas_NombreUsuario",
                table: "Personas");

            migrationBuilder.RenameColumn(
                name: "NombreUsuario",
                table: "Personas",
                newName: "IdUsuario");

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 5,
                column: "TarifaBase",
                value: 7000f);

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 6,
                column: "TarifaBase",
                value: 8000f);

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 7,
                column: "TarifaBase",
                value: 7000f);

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 8,
                column: "TarifaBase",
                value: 8500f);

            migrationBuilder.UpdateData(
                table: "Municipios",
                keyColumn: "Id",
                keyValue: 9,
                column: "TarifaBase",
                value: 9500f);

            migrationBuilder.CreateIndex(
                name: "IX_Personas_IdUsuario",
                table: "Personas",
                column: "IdUsuario",
                unique: true,
                filter: "[IdUsuario] IS NOT NULL");
        }
    }
}
