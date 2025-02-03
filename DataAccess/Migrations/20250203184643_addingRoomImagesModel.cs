using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addingRoomImagesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HotelImages_hotels_HotelId",
                table: "HotelImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HotelImages",
                table: "HotelImages");

            migrationBuilder.RenameTable(
                name: "HotelImages",
                newName: "hotelImages");

            migrationBuilder.RenameIndex(
                name: "IX_HotelImages_HotelId",
                table: "hotelImages",
                newName: "IX_hotelImages_HotelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hotelImages",
                table: "hotelImages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "roomImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roomImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_roomImages_rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_roomImages_RoomId",
                table: "roomImages",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_hotelImages_hotels_HotelId",
                table: "hotelImages",
                column: "HotelId",
                principalTable: "hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_hotelImages_hotels_HotelId",
                table: "hotelImages");

            migrationBuilder.DropTable(
                name: "roomImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hotelImages",
                table: "hotelImages");

            migrationBuilder.RenameTable(
                name: "hotelImages",
                newName: "HotelImages");

            migrationBuilder.RenameIndex(
                name: "IX_hotelImages_HotelId",
                table: "HotelImages",
                newName: "IX_HotelImages_HotelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HotelImages",
                table: "HotelImages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HotelImages_hotels_HotelId",
                table: "HotelImages",
                column: "HotelId",
                principalTable: "hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
