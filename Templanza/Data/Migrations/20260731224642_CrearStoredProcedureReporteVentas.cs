using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Templanza.Data.Migrations
{
    /// <inheritdoc />
    public partial class CrearStoredProcedureReporteVentas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE PROCEDURE dbo.ReporteVentasPorRango
                    @FechaInicio DATE,
                    @FechaFin DATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    SELECT
                        p.Id AS PlantaId,
                        p.NombreComun,
                        SUM(io.Cantidad) AS CantidadVendida,
                        SUM(io.Cantidad * io.PrecioUnitario) AS TotalVendido
                    FROM ItemOrdenes io
                    INNER JOIN Ordenes o ON io.OrdenId = o.Id
                    INNER JOIN Plantas p ON io.PlantaId = p.Id
                    WHERE o.FechaCreacion >= @FechaInicio
                      AND o.FechaCreacion < DATEADD(DAY, 1, @FechaFin)
                      AND o.Estado <> 'Cancelada'
                    GROUP BY p.Id, p.NombreComun
                    ORDER BY TotalVendido DESC;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.ReporteVentasPorRango;");
        }
    }
}
