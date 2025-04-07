using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdilitSoft.app.Migrations
{
    /// <inheritdoc />
    public partial class JuanPa_ClientesProveedores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cotizacion_IdProovedor",
                table: "Cotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizacion_IdProovedor",
                table: "Cotizacion",
                column: "IdProovedor");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cotizacion_IdProovedor",
                table: "Cotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizacion_IdProovedor",
                table: "Cotizacion",
                column: "IdProovedor",
                unique: true);
        }
    }
}
