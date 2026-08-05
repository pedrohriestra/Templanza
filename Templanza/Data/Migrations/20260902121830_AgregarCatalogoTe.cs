using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Templanza.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCatalogoTe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 5, "Flores e infusiones aromáticas", "Florales y aromáticas" },
                    { 6, "Hierbas que refuerzan las defensas", "Inmunológicas" }
                });

            migrationBuilder.InsertData(
                table: "Efectos",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 6, "Refuerza las defensas del organismo", "Inmunoestimulante" },
                    { 7, "Ayuda a combatir el daño celular", "Antioxidante" }
                });

            migrationBuilder.InsertData(
                table: "Plantas",
                columns: new[] { "Id", "CategoriaId", "Contraindicaciones", "NombreCientifico", "NombreComun", "Origen", "ParteUsada", "Precio", "Stock" },
                values: new object[,]
                {
                    { 5, 3, "Sensibilidad a la cafeína, insomnio", "Camellia sinensis", "Té verde", "China", "Hoja", 1100m, 140 },
                    { 6, 3, "Sensibilidad a la cafeína, hipertensión", "Camellia sinensis", "Té negro", "China", "Hoja", 1050m, 130 },
                    { 7, 3, "Sensibilidad a la cafeína", "Camellia sinensis", "Té blanco", "China", "Brote y hoja joven", 1350m, 70 },
                    { 8, 3, "Sensibilidad a la cafeína", "Camellia sinensis", "Té oolong", "China", "Hoja", 1200m, 90 },
                    { 9, 2, "Sin contraindicaciones relevantes conocidas", "Aspalathus linearis", "Rooibos", "Sudáfrica", "Hoja", 900m, 110 },
                    { 10, 4, "Embarazo, presión arterial baja", "Hibiscus sabdariffa", "Hibisco", "África", "Flor (cáliz)", 820m, 100 },
                    { 11, 2, "Somnolencia, combinar con sedantes", "Lavandula angustifolia", "Lavanda", "Región mediterránea", "Flor", 880m, 90 },
                    { 12, 1, "Uso excesivo en niños pequeños", "Illicium verum", "Anís estrella", "China", "Fruto", 990m, 65 },
                    { 13, 1, "Embarazo en altas dosis, trastornos hepáticos", "Cinnamomum verum", "Canela", "Sri Lanka", "Corteza", 850m, 120 },
                    { 14, 1, "Cálculos biliares", "Elettaria cardamomum", "Cardamomo", "India", "Semilla", 1150m, 55 },
                    { 15, 1, "Trastornos de la coagulación", "Syzygium aromaticum", "Clavo de olor", "Indonesia", "Botón floral", 980m, 60 },
                    { 16, 4, "Cálculos biliares, anticoagulantes", "Curcuma longa", "Cúrcuma", "India", "Rizoma", 920m, 100 },
                    { 17, 1, "Hipertensión, embarazo", "Glycyrrhiza glabra", "Regaliz", "Europa y Asia", "Raíz", 860m, 70 },
                    { 18, 2, "Hipotiroidismo", "Melissa officinalis", "Melisa (Toronjil)", "Europa", "Hoja", 780m, 95 },
                    { 19, 2, "Combinar con sedantes o alcohol", "Valeriana officinalis", "Valeriana", "Europa y Asia", "Raíz", 940m, 60 },
                    { 20, 2, "Embarazo, combinar con sedantes", "Passiflora incarnata", "Pasiflora", "América", "Hoja y flor", 830m, 75 },
                    { 21, 2, "Uso prolongado sin supervisión", "Tilia platyphyllos", "Tilo", "Europa", "Flor", 750m, 100 },
                    { 22, 1, "Obstrucción de vías biliares, embarazo", "Peumus boldus", "Boldo", "Chile", "Hoja", 800m, 65 },
                    { 23, 2, "Sin contraindicaciones relevantes conocidas", "Aloysia citrodora", "Cedrón", "Sudamérica", "Hoja", 720m, 85 },
                    { 24, 4, "Insuficiencia renal o cardíaca", "Urtica dioica", "Ortiga", "Europa", "Hoja", 690m, 80 },
                    { 26, 3, "Embarazo en altas dosis, epilepsia", "Rosmarinus officinalis", "Romero", "Región mediterránea", "Hoja", 760m, 110 },
                    { 29, 3, "Hipertensión, insomnio", "Panax ginseng", "Ginseng", "Asia", "Raíz", 1450m, 40 },
                    { 30, 3, "Sensibilidad a la cafeína, hipertensión", "Ilex paraguariensis", "Yerba mate", "Sudamérica", "Hoja", 700m, 150 }
                });

            migrationBuilder.InsertData(
                table: "PlantaEfectos",
                columns: new[] { "EfectoId", "PlantaId", "Intensidad" },
                values: new object[,]
                {
                    { 3, 5, 3 },
                    { 7, 5, 4 },
                    { 3, 6, 4 },
                    { 7, 7, 3 },
                    { 3, 8, 3 },
                    { 7, 9, 3 },
                    { 5, 10, 3 },
                    { 1, 11, 4 },
                    { 2, 12, 3 },
                    { 2, 13, 3 },
                    { 2, 14, 3 },
                    { 4, 15, 3 },
                    { 4, 16, 4 },
                    { 2, 17, 3 },
                    { 1, 18, 4 },
                    { 1, 19, 5 },
                    { 1, 20, 4 },
                    { 1, 21, 3 },
                    { 2, 22, 4 },
                    { 1, 23, 3 },
                    { 5, 24, 4 },
                    { 3, 26, 3 },
                    { 3, 29, 5 },
                    { 3, 30, 4 }
                });

            migrationBuilder.InsertData(
                table: "Plantas",
                columns: new[] { "Id", "CategoriaId", "Contraindicaciones", "NombreCientifico", "NombreComun", "Origen", "ParteUsada", "Precio", "Stock" },
                values: new object[,]
                {
                    { 25, 6, "Enfermedades autoinmunes", "Echinacea purpurea", "Equinácea", "Norteamérica", "Raíz y flor", 1000m, 55 },
                    { 27, 5, "Sin contraindicaciones relevantes conocidas", "Jasminum officinale", "Jazmín", "Asia", "Flor", 950m, 60 },
                    { 28, 6, "Cálculos renales", "Rosa canina", "Rosa mosqueta (Escaramujo)", "Europa", "Fruto", 1080m, 50 }
                });

            migrationBuilder.InsertData(
                table: "PlantaEfectos",
                columns: new[] { "EfectoId", "PlantaId", "Intensidad" },
                values: new object[,]
                {
                    { 6, 25, 4 },
                    { 1, 27, 3 },
                    { 6, 28, 3 },
                    { 7, 28, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 7, 5 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 3, 6 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 7, 7 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 3, 8 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 7, 9 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 5, 10 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 1, 11 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 2, 12 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 2, 13 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 2, 14 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 4, 15 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 4, 16 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 2, 17 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 1, 18 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 1, 19 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 1, 20 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 1, 21 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 2, 22 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 1, 23 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 5, 24 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 6, 25 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 3, 26 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 1, 27 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 6, 28 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 7, 28 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 3, 29 });

            migrationBuilder.DeleteData(
                table: "PlantaEfectos",
                keyColumns: new[] { "EfectoId", "PlantaId" },
                keyValues: new object[] { 3, 30 });

            migrationBuilder.DeleteData(
                table: "Efectos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Efectos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Plantas",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
