using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_v4.Migrations
{
    /// <inheritdoc />
    public partial class EstadoPrestamo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Prestamos");

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Prestamos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Prestamos");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Prestamos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
