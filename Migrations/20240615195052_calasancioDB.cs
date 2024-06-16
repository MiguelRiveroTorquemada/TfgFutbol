using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CpCalasancio.Migrations
{
    public partial class calasancioDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carritos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    enviado = table.Column<bool>(type: "bit", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StripeSessionId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carritos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lugar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    asiste = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Partidos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    jornadaId = table.Column<int>(type: "int", nullable: false),
                    nombrePartido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    primerEquipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    segundoEquipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    puntuacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ganado = table.Column<bool>(type: "bit", nullable: false),
                    fecha = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isAdmin = table.Column<bool>(type: "bit", nullable: false),
                    Carritoid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Clientes_Carritos_Carritoid",
                        column: x => x.Carritoid,
                        principalTable: "Carritos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreProducto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Carritoid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.id);
                    table.ForeignKey(
                        name: "FK_Productos_Carritos_Carritoid",
                        column: x => x.Carritoid,
                        principalTable: "Carritos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eventoid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Eventos_Eventoid",
                        column: x => x.Eventoid,
                        principalTable: "Eventos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Jugadores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    apellidos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    posicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    altura = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    pie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numeroCamiseta = table.Column<int>(type: "int", nullable: false),
                    partidosJugados = table.Column<bool>(type: "bit", nullable: false),
                    goles = table.Column<int>(type: "int", nullable: false),
                    Partidoid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugadores", x => x.id);
                    table.ForeignKey(
                        name: "FK_Jugadores_Partidos_Partidoid",
                        column: x => x.Partidoid,
                        principalTable: "Partidos",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CarnetSocios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    precio = table.Column<int>(type: "int", nullable: false),
                    clienteid = table.Column<int>(type: "int", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StripeSessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Carritoid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarnetSocios", x => x.id);
                    table.ForeignKey(
                        name: "FK_CarnetSocios_Carritos_Carritoid",
                        column: x => x.Carritoid,
                        principalTable: "Carritos",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CarnetSocios_Clientes_clienteid",
                        column: x => x.clienteid,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarnetSocios_Carritoid",
                table: "CarnetSocios",
                column: "Carritoid");

            migrationBuilder.CreateIndex(
                name: "IX_CarnetSocios_clienteid",
                table: "CarnetSocios",
                column: "clienteid");

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Carritoid",
                table: "Clientes",
                column: "Carritoid");

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_Partidoid",
                table: "Jugadores",
                column: "Partidoid");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Carritoid",
                table: "Productos",
                column: "Carritoid");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Eventoid",
                table: "Usuarios",
                column: "Eventoid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarnetSocios");

            migrationBuilder.DropTable(
                name: "Jugadores");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Partidos");

            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Carritos");
        }
    }
}
