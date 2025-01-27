using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateingHotelManager : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelManagerId",
                table: "hotels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HotelManagers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelManagers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_hotels_HotelManagerId",
                table: "hotels",
                column: "HotelManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_hotels_HotelManagers_HotelManagerId",
                table: "hotels",
                column: "HotelManagerId",
                principalTable: "HotelManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hotels_HotelManagers_HotelManagerId",
                table: "hotels");

            migrationBuilder.DropTable(
                name: "HotelManagers");

            migrationBuilder.DropIndex(
                name: "IX_hotels_HotelManagerId",
                table: "hotels");

            migrationBuilder.DropColumn(
                name: "HotelManagerId",
                table: "hotels");
        }
    }
}