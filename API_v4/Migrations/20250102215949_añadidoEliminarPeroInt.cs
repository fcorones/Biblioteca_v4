using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_v4.Migrations
{
    /// <inheritdoc />
    public partial class añadidoEliminarPeroInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Eliminado",
                table: "Usuarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Eliminado",
                table: "Prestamos",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "Eliminado",
                table: "Libros",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Eliminado",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Eliminado",
                table: "Prestamos",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "Eliminado",
                table: "Libros",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
