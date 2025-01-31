using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_v4.Migrations
{
    /// <inheritdoc />
    public partial class eliminarAutorEditorialGenero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Generos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Editoriales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Autores",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Generos");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Editoriales");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Autores");
        }
    }
}
