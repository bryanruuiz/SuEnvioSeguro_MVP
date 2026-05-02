using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuEnvioSeguro.API.Migrations
{
    /// <inheritdoc />
    public partial class IdentitiesAndBusinessRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Envios_Facturas_CodigoFactura",
                table: "Envios");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Personas_ClienteDocumento",
                table: "Facturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Personas_ClienteDocumentoIdentidad",
                table: "Facturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Personas_UsuarioDocumento",
                table: "Facturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Personas_UsuarioDocumentoIdentidad",
                table: "Facturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personas",
                table: "Personas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facturas",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_ClienteDocumento",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_ClienteDocumentoIdentidad",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_UsuarioDocumento",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_UsuarioDocumentoIdentidad",
                table: "Facturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Envios",
                table: "Envios");

            migrationBuilder.DropIndex(
                name: "IX_Envios_CodigoFactura",
                table: "Envios");

            migrationBuilder.DropColumn(
                name: "ClienteDocumentoIdentidad",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "UsuarioDocumentoIdentidad",
                table: "Facturas");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Personas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Personas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Personas",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "Personas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Personas",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Personas",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentoIdentidad",
                table: "Personas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Personas",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Personas",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioDocumento",
                table: "Facturas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ClienteDocumento",
                table: "Facturas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoFactura",
                table: "Facturas",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ClienteId",
                table: "Facturas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Facturas",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MunicipioDestino",
                table: "Envios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Envios",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DireccionEnvio",
                table: "Envios",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DescripcionContenido",
                table: "Envios",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoFactura",
                table: "Envios",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CodigoEnvio",
                table: "Envios",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Envios",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "FacturaId",
                table: "Envios",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE Personas
                SET Activo = 1
                WHERE Discriminator = 'Usuario' AND Activo IS NULL;

                UPDATE f
                SET ClienteId = p.Id
                FROM Facturas f
                INNER JOIN Personas p ON p.DocumentoIdentidad = f.ClienteDocumento;

                UPDATE f
                SET UsuarioId = p.Id
                FROM Facturas f
                INNER JOIN Personas p ON p.DocumentoIdentidad = f.UsuarioDocumento;

                UPDATE e
                SET FacturaId = f.Id
                FROM Envios e
                INNER JOIN Facturas f ON f.CodigoFactura = e.CodigoFactura;
            ");

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "Facturas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UsuarioId",
                table: "Facturas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FacturaId",
                table: "Envios",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personas",
                table: "Personas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facturas",
                table: "Facturas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Envios",
                table: "Envios",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_DocumentoIdentidad",
                table: "Personas",
                column: "DocumentoIdentidad",
                unique: true);

            migrationBuilder.Sql(@"
                ;WITH UsuariosDuplicados AS
                (
                    SELECT
                        Id,
                        IdUsuario,
                        ROW_NUMBER() OVER (PARTITION BY IdUsuario ORDER BY Id) AS Fila
                    FROM Personas
                    WHERE IdUsuario IS NOT NULL
                )
                UPDATE UsuariosDuplicados
                SET IdUsuario = LEFT(CONCAT(IdUsuario, '-', Id), 50)
                WHERE Fila > 1;
            ");

            migrationBuilder.CreateIndex(
                name: "IX_Personas_IdUsuario",
                table: "Personas",
                column: "IdUsuario",
                unique: true,
                filter: "[IdUsuario] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_ClienteId",
                table: "Facturas",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_CodigoFactura",
                table: "Facturas",
                column: "CodigoFactura",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioId",
                table: "Facturas",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Envios_CodigoEnvio",
                table: "Envios",
                column: "CodigoEnvio",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Envios_FacturaId",
                table: "Envios",
                column: "FacturaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Envios_Facturas_FacturaId",
                table: "Envios",
                column: "FacturaId",
                principalTable: "Facturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Personas_ClienteId",
                table: "Facturas",
                column: "ClienteId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Personas_UsuarioId",
                table: "Facturas",
                column: "UsuarioId",
                principalTable: "Personas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Envios_Facturas_FacturaId",
                table: "Envios");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Personas_ClienteId",
                table: "Facturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Facturas_Personas_UsuarioId",
                table: "Facturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Personas",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Personas_DocumentoIdentidad",
                table: "Personas");

            migrationBuilder.DropIndex(
                name: "IX_Personas_IdUsuario",
                table: "Personas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Facturas",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_ClienteId",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_CodigoFactura",
                table: "Facturas");

            migrationBuilder.DropIndex(
                name: "IX_Facturas_UsuarioId",
                table: "Facturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Envios",
                table: "Envios");

            migrationBuilder.DropIndex(
                name: "IX_Envios_CodigoEnvio",
                table: "Envios");

            migrationBuilder.DropIndex(
                name: "IX_Envios_FacturaId",
                table: "Envios");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Personas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Envios");

            migrationBuilder.DropColumn(
                name: "FacturaId",
                table: "Envios");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "IdUsuario",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DocumentoIdentidad",
                table: "Personas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Direccion",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Correo",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioDocumento",
                table: "Facturas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoFactura",
                table: "Facturas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ClienteDocumento",
                table: "Facturas",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ClienteDocumentoIdentidad",
                table: "Facturas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioDocumentoIdentidad",
                table: "Facturas",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MunicipioDestino",
                table: "Envios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "Envios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "DireccionEnvio",
                table: "Envios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "DescripcionContenido",
                table: "Envios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoFactura",
                table: "Envios",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoEnvio",
                table: "Envios",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Personas",
                table: "Personas",
                column: "DocumentoIdentidad");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Facturas",
                table: "Facturas",
                column: "CodigoFactura");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Envios",
                table: "Envios",
                column: "CodigoEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_ClienteDocumento",
                table: "Facturas",
                column: "ClienteDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_ClienteDocumentoIdentidad",
                table: "Facturas",
                column: "ClienteDocumentoIdentidad");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioDocumento",
                table: "Facturas",
                column: "UsuarioDocumento");

            migrationBuilder.CreateIndex(
                name: "IX_Facturas_UsuarioDocumentoIdentidad",
                table: "Facturas",
                column: "UsuarioDocumentoIdentidad");

            migrationBuilder.CreateIndex(
                name: "IX_Envios_CodigoFactura",
                table: "Envios",
                column: "CodigoFactura");

            migrationBuilder.AddForeignKey(
                name: "FK_Envios_Facturas_CodigoFactura",
                table: "Envios",
                column: "CodigoFactura",
                principalTable: "Facturas",
                principalColumn: "CodigoFactura",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Personas_ClienteDocumento",
                table: "Facturas",
                column: "ClienteDocumento",
                principalTable: "Personas",
                principalColumn: "DocumentoIdentidad",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Personas_ClienteDocumentoIdentidad",
                table: "Facturas",
                column: "ClienteDocumentoIdentidad",
                principalTable: "Personas",
                principalColumn: "DocumentoIdentidad");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Personas_UsuarioDocumento",
                table: "Facturas",
                column: "UsuarioDocumento",
                principalTable: "Personas",
                principalColumn: "DocumentoIdentidad");

            migrationBuilder.AddForeignKey(
                name: "FK_Facturas_Personas_UsuarioDocumentoIdentidad",
                table: "Facturas",
                column: "UsuarioDocumentoIdentidad",
                principalTable: "Personas",
                principalColumn: "DocumentoIdentidad");
        }
    }
}
