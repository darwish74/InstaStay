using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "hotels",
                newName: "Stars");

            migrationBuilder.AddColumn<bool>(
                name: "ISBlocked",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBlocked",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Stars",
                table: "hotels",
                newName: "Rate");
        }
    }
}
