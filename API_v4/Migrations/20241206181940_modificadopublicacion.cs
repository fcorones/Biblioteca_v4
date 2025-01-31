using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_v4.Migrations
{
    /// <inheritdoc />
    public partial class modificadopublicacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publicacion",
                table: "Libros");

            migrationBuilder.AddColumn<int>(
                name: "AnioPublicacion",
                table: "Libros",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnioPublicacion",
                table: "Libros");

            migrationBuilder.AddColumn<DateTime>(
                name: "Publicacion",
                table: "Libros",
                type: "datetime2",
                nullable: true);
        }
    }
}
