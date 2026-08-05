using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Templanza.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarImagenesPlantas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "Plantas",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 1,
                column: "ImagenUrl",
                value: "/images/plantas/manzanilla.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 2,
                column: "ImagenUrl",
                value: "/images/plantas/menta.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 3,
                column: "ImagenUrl",
                value: "/images/plantas/jengibre.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 4,
                column: "ImagenUrl",
                value: "/images/plantas/diente-de-leon.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 5,
                column: "ImagenUrl",
                value: "/images/plantas/te-verde.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 6,
                column: "ImagenUrl",
                value: "/images/plantas/te-negro.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 7,
                column: "ImagenUrl",
                value: "/images/plantas/te-blanco.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 8,
                column: "ImagenUrl",
                value: "/images/plantas/te-oolong.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 9,
                column: "ImagenUrl",
                value: "/images/plantas/rooibos.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 10,
                column: "ImagenUrl",
                value: "/images/plantas/hibisco.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 11,
                column: "ImagenUrl",
                value: "/images/plantas/lavanda.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 12,
                column: "ImagenUrl",
                value: "/images/plantas/anis-estrella.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 13,
                column: "ImagenUrl",
                value: "/images/plantas/canela.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 14,
                column: "ImagenUrl",
                value: "/images/plantas/cardomomo.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 15,
                column: "ImagenUrl",
                value: "/images/plantas/clavo-de-olor.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 16,
                column: "ImagenUrl",
                value: "/images/plantas/curcuma.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 17,
                column: "ImagenUrl",
                value: "/images/plantas/regaliz.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 18,
                column: "ImagenUrl",
                value: "/images/plantas/melisa.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 19,
                column: "ImagenUrl",
                value: "/images/plantas/valeriana.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 20,
                column: "ImagenUrl",
                value: "/images/plantas/pasiflora.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 21,
                column: "ImagenUrl",
                value: "/images/plantas/tilo.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 22,
                column: "ImagenUrl",
                value: "/images/plantas/boldo.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 23,
                column: "ImagenUrl",
                value: "/images/plantas/cedron.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 24,
                column: "ImagenUrl",
                value: "/images/plantas/ortiga.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 25,
                column: "ImagenUrl",
                value: "/images/plantas/equinacea.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 26,
                column: "ImagenUrl",
                value: "/images/plantas/romero.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 27,
                column: "ImagenUrl",
                value: "/images/plantas/jazmin.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 28,
                column: "ImagenUrl",
                value: "/images/plantas/rosa-mosqueta.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 29,
                column: "ImagenUrl",
                value: "/images/plantas/ginseng.webp");

            migrationBuilder.UpdateData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 30,
                column: "ImagenUrl",
                value: "/images/plantas/yerba-mate.webp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "Plantas");
        }
    }
}
