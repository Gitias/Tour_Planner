using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tourplanner.DAL.Migrations
{
    /// <inheritdoc />
    public partial class inittwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChildFriendliness",
                table: "Tours",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Popularity",
                table: "Tours",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourLogs_Tours_TourId",
                table: "TourLogs");

            migrationBuilder.DropIndex(
                name: "IX_TourLogs_TourId",
                table: "TourLogs");

            migrationBuilder.DropColumn(
                name: "ChildFriendliness",
                table: "Tours");

            migrationBuilder.DropColumn(
                name: "Popularity",
                table: "Tours");
        }
    }
}
