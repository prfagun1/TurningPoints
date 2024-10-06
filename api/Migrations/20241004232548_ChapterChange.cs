using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ChapterChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "NextChapters",
                table: "Chapter");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "Chapter",
                newName: "IsStart");

            migrationBuilder.AddColumn<Guid>(
                name: "Choice01",
                table: "Chapter",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "Choice02",
                table: "Chapter",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "Choice03",
                table: "Chapter",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "Choice04",
                table: "Chapter",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<string>(
                name: "Conteudo",
                table: "Chapter",
                type: "MEDIUMTEXT",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnd",
                table: "Chapter",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Choice01",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "Choice02",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "Choice03",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "Choice04",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "Conteudo",
                table: "Chapter");

            migrationBuilder.DropColumn(
                name: "IsEnd",
                table: "Chapter");

            migrationBuilder.RenameColumn(
                name: "IsStart",
                table: "Chapter",
                newName: "End");

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "Chapter",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "NextChapters",
                table: "Chapter",
                type: "varchar(30)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
