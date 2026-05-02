using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuEnvioSeguro.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personas",
                columns: table => new
                {
                    DocumentoIdentidad = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    TipoCliente = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contrasena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personas", x => x.DocumentoIdentidad);
                });

            migrationBuilder.CreateTable(
                name: "Facturas",
                columns: table => new
                {
                    CodigoFactura = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValorNeto = table.Column<float>(type: "real", nullable: false),
                    PorcentajeIVA = table.Column<float>(type: "real", nullable: false),
                    TotalAPagar = table.Column<float>(type: "real", nullable: false),
                    ClienteDocumento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsuarioDocumento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClienteDocumentoIdentidad = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UsuarioDocumentoIdentidad = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturas", x => x.CodigoFactura);
                    table.ForeignKey(
                        name: "FK_Facturas_Personas_ClienteDocumento",
                        column: x => x.ClienteDocumento,
                        principalTable: "Personas",
                        principalColumn: "DocumentoIdentidad",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Facturas_Personas_ClienteDocumentoIdentidad",
                        column: x => x.ClienteDocumentoIdentidad,
                        principalTable: "Personas",
                        principalColumn: "DocumentoIdentidad");
                    table.ForeignKey(
                        name: "FK_Facturas_Personas_UsuarioDocumento",
                        column: x => x.UsuarioDocumento,
                        principalTable: "Personas",
                        principalColumn: "DocumentoIdentidad");
                    table.ForeignKey(
                        name: "FK_Facturas_Personas_UsuarioDocumentoIdentidad",
                        column: x => x.UsuarioDocumentoIdentidad,
                        principalTable: "Personas",
                        principalColumn: "DocumentoIdentidad");
                });

            migrationBuilder.CreateTable(
                name: "Envios",
                columns: table => new
                {
                    CodigoEnvio = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DescripcionContenido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MunicipioDestino = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Peso = table.Column<float>(type: "real", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    DireccionEnvio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsDelicado = table.Column<bool>(type: "bit", nullable: false),
                    ValorNetoEnvio = table.Column<float>(type: "real", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoFactura = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Envios", x => x.CodigoEnvio);
                    table.ForeignKey(
                        name: "FK_Envios_Facturas_CodigoFactura",
                        column: x => x.CodigoFactura,
                        principalTable: "Facturas",
                        principalColumn: "CodigoFactura",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Envios_CodigoFactura",
                table: "Envios",
                column: "CodigoFactura");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Envios");

            migrationBuilder.DropTable(
                name: "Facturas");

            migrationBuilder.DropTable(
                name: "Personas");
        }
    }
}
