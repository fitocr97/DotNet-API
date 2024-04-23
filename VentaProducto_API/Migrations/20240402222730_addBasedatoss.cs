using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VentaProductoAPI.Migrations
{
    /// <inheritdoc />
    public partial class addBasedatoss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Tamaño",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Cantidad", "Detalle", "FechaAcualizacion", "FechaCreacion", "ImageUrl", "Nombre", "Precio", "Tamaño" },
                values: new object[] { 1, 0, "", new DateTime(2024, 4, 2, 16, 27, 30, 166, DateTimeKind.Local).AddTicks(1625), new DateTime(2024, 4, 2, 16, 27, 30, 166, DateTimeKind.Local).AddTicks(1616), "", "", 0, "Mediano" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Productos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Tamaño",
                table: "Productos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ImageUrl",
                table: "Productos",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
