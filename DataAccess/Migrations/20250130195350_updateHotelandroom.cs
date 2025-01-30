using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateHotelandroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hotels_HotelManagers_HotelManagerId",
                table: "hotels");

            migrationBuilder.AlterColumn<int>(
                name: "HotelManagerId",
                table: "hotels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "HotelImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HotelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HotelImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HotelImages_hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HotelImages_HotelId",
                table: "HotelImages",
                column: "HotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_hotels_HotelManagers_HotelManagerId",
                table: "hotels",
                column: "HotelManagerId",
                principalTable: "HotelManagers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hotels_HotelManagers_HotelManagerId",
                table: "hotels");

            migrationBuilder.DropTable(
                name: "HotelImages");

            migrationBuilder.AlterColumn<int>(
                name: "HotelManagerId",
                table: "hotels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_hotels_HotelManagers_HotelManagerId",
                table: "hotels",
                column: "HotelManagerId",
                principalTable: "HotelManagers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
