using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_v4.Migrations
{
    /// <inheritdoc />
    public partial class añadidoEliminarColumna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Prestamos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Libros",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Prestamos");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Libros");
        }
    }
}
