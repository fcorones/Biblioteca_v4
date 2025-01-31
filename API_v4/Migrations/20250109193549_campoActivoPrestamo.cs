using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_v4.Migrations
{
    /// <inheritdoc />
    public partial class campoActivoPrestamo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Prestamos",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos",
                column: "LibroId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos");

            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Prestamos");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos",
                column: "LibroId",
                unique: true);
        }
    }
}
