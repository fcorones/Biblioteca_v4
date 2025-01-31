using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_v4.Migrations
{
    /// <inheritdoc />
    public partial class modificadopublicacion2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnioPublicacion",
                table: "Libros",
                newName: "AnioDePublicacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnioDePublicacion",
                table: "Libros",
                newName: "AnioPublicacion");
        }
    }
}
