using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VentaProductoAPI.Migrations
{
    /// <inheritdoc />
    public partial class Agregardatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Detalle", "FechaAcualizacion", "FechaCreacion", "Nombre", "Precio" },
                values: new object[] { "C", new DateTime(2024, 4, 2, 16, 43, 52, 862, DateTimeKind.Local).AddTicks(2631), new DateTime(2024, 4, 2, 16, 43, 52, 862, DateTimeKind.Local).AddTicks(2618), "Colibri", 3000 });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Cantidad", "Detalle", "FechaAcualizacion", "FechaCreacion", "ImageUrl", "Nombre", "Precio", "Tamaño" },
                values: new object[] { 2, 0, "C", new DateTime(2024, 4, 2, 16, 43, 52, 862, DateTimeKind.Local).AddTicks(2633), new DateTime(2024, 4, 2, 16, 43, 52, 862, DateTimeKind.Local).AddTicks(2633), "", "Corazon", 2000, "Mediano" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Detalle", "FechaAcualizacion", "FechaCreacion", "Nombre", "Precio" },
                values: new object[] { "", new DateTime(2024, 4, 2, 16, 27, 30, 166, DateTimeKind.Local).AddTicks(1625), new DateTime(2024, 4, 2, 16, 27, 30, 166, DateTimeKind.Local).AddTicks(1616), "", 0 });
        }
    }
}
