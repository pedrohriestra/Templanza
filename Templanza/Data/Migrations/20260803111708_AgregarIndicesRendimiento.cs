using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Templanza.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarIndicesRendimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_FechaCreacion",
                table: "Ordenes",
                column: "FechaCreacion");

            migrationBuilder.CreateIndex(
                name: "IX_Blends_EsPublicado_EsRecomendado",
                table: "Blends",
                columns: new[] { "EsPublicado", "EsRecomendado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ordenes_FechaCreacion",
                table: "Ordenes");

            migrationBuilder.DropIndex(
                name: "IX_Blends_EsPublicado_EsRecomendado",
                table: "Blends");
        }
    }
}
