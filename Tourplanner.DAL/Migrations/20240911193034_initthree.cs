using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourplanner.DAL.Migrations
{
    /// <inheritdoc />
    public partial class initthree : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavorite",
                table: "Tours",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavorite",
                table: "Tours");
        }
    }
}
