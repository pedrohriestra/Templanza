using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Templanza.Data.Migrations
{
    /// <inheritdoc />
    public partial class DominioInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "AspNetUsers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CorreosEnviados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Destinatario = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Asunto = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Cuerpo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Exito = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorreosEnviados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Efectos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Efectos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ordenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ordenes_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Blends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    EsPublicado = table.Column<bool>(type: "bit", nullable: false),
                    EsRecomendado = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blends_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blends_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Plantas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreComun = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NombreCientifico = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Contraindicaciones = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ParteUsada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Origen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plantas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plantas_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlendLikes",
                columns: table => new
                {
                    BlendId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlendLikes", x => new { x.BlendId, x.UsuarioId });
                    table.ForeignKey(
                        name: "FK_BlendLikes_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlendLikes_Blends_BlendId",
                        column: x => x.BlendId,
                        principalTable: "Blends",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlendId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Texto = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentarios_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comentarios_Blends_BlendId",
                        column: x => x.BlendId,
                        principalTable: "Blends",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlendPlantas",
                columns: table => new
                {
                    BlendId = table.Column<int>(type: "int", nullable: false),
                    PlantaId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Unidad = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlendPlantas", x => new { x.BlendId, x.PlantaId });
                    table.ForeignKey(
                        name: "FK_BlendPlantas_Blends_BlendId",
                        column: x => x.BlendId,
                        principalTable: "Blends",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlendPlantas_Plantas_PlantaId",
                        column: x => x.PlantaId,
                        principalTable: "Plantas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemOrdenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenId = table.Column<int>(type: "int", nullable: false),
                    PlantaId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemOrdenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemOrdenes_Ordenes_OrdenId",
                        column: x => x.OrdenId,
                        principalTable: "Ordenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemOrdenes_Plantas_PlantaId",
                        column: x => x.PlantaId,
                        principalTable: "Plantas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlantaEfectos",
                columns: table => new
                {
                    PlantaId = table.Column<int>(type: "int", nullable: false),
                    EfectoId = table.Column<int>(type: "int", nullable: false),
                    Intensidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantaEfectos", x => new { x.PlantaId, x.EfectoId });
                    table.ForeignKey(
                        name: "FK_PlantaEfectos_Efectos_EfectoId",
                        column: x => x.EfectoId,
                        principalTable: "Efectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlantaEfectos_Plantas_PlantaId",
                        column: x => x.PlantaId,
                        principalTable: "Plantas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Hierbas que favorecen la digestión", "Digestivas" },
                    { 2, "Hierbas con efecto calmante", "Relajantes" },
                    { 3, "Hierbas estimulantes", "Energizantes" },
                    { 4, "Hierbas depurativas y diuréticas", "Depurativas" }
                });

            migrationBuilder.InsertData(
                table: "Efectos",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Induce calma y reduce el estrés", "Relajante" },
                    { 2, "Favorece la digestión", "Digestivo" },
                    { 3, "Aumenta la energía y el estado de alerta", "Estimulante" },
                    { 4, "Reduce inflamación", "Antiinflamatorio" },
                    { 5, "Favorece la eliminación de líquidos", "Diurético" }
                });

            migrationBuilder.InsertData(
                table: "Plantas",
                columns: new[] { "Id", "CategoriaId", "Contraindicaciones", "NombreCientifico", "NombreComun", "Origen", "ParteUsada", "Precio", "Stock" },
                values: new object[,]
                {
                    { 1, 2, "Alergia a plantas de la familia Asteraceae", "Matricaria chamomilla", "Manzanilla", "Europa", "Flor", 850m, 100 },
                    { 2, 1, "Reflujo gastroesofágico", "Mentha spicata", "Menta", "Asia", "Hoja", 700m, 120 },
                    { 3, 3, "Cálculos biliares", "Zingiber officinale", "Jengibre", "Sudeste asiático", "Raíz", 950m, 80 },
                    { 4, 4, "Obstrucción de vías biliares", "Taraxacum officinale", "Diente de león", "Europa", "Raíz y hoja", 780m, 60 }
                });

            migrationBuilder.InsertData(
                table: "PlantaEfectos",
                columns: new[] { "EfectoId", "PlantaId", "Intensidad" },
                values: new object[,]
                {
                    { 1, 1, 4 },
                    { 2, 2, 3 },
                    { 2, 3, 4 },
                    { 3, 3, 3 },
                    { 5, 4, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlendLikes_UsuarioId",
                table: "BlendLikes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_BlendPlantas_PlantaId",
                table: "BlendPlantas",
                column: "PlantaId");

            migrationBuilder.CreateIndex(
                name: "IX_Blends_CategoriaId",
                table: "Blends",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Blends_UsuarioId",
                table: "Blends",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_BlendId",
                table: "Comentarios",
                column: "BlendId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_UsuarioId",
                table: "Comentarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrdenes_OrdenId",
                table: "ItemOrdenes",
                column: "OrdenId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemOrdenes_PlantaId",
                table: "ItemOrdenes",
                column: "PlantaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_UsuarioId",
                table: "Ordenes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantaEfectos_EfectoId",
                table: "PlantaEfectos",
                column: "EfectoId");

            migrationBuilder.CreateIndex(
                name: "IX_Plantas_CategoriaId",
                table: "Plantas",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlendLikes");

            migrationBuilder.DropTable(
                name: "BlendPlantas");

            migrationBuilder.DropTable(
                name: "Comentarios");

            migrationBuilder.DropTable(
                name: "CorreosEnviados");

            migrationBuilder.DropTable(
                name: "ItemOrdenes");

            migrationBuilder.DropTable(
                name: "PlantaEfectos");

            migrationBuilder.DropTable(
                name: "Blends");

            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropTable(
                name: "Efectos");

            migrationBuilder.DropTable(
                name: "Plantas");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "AspNetUsers");
        }
    }
}
