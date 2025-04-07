using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EdilitSoft.app.Migrations
{
    /// <inheritdoc />
    public partial class MDanielCotiCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cotizacion_IdCliente",
                table: "Cotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizacion_IdCliente",
                table: "Cotizacion",
                column: "IdCliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cotizacion_IdCliente",
                table: "Cotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_Cotizacion_IdCliente",
                table: "Cotizacion",
                column: "IdCliente",
                unique: true);
        }
    }
}
