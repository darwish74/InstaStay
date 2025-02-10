using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatetableproblem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ProblemReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Request",
                table: "ProblemReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProblemReports",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserImgRequest",
                table: "ProblemReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProblemReports_UserId",
                table: "ProblemReports",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProblemReports_AspNetUsers_UserId",
                table: "ProblemReports",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProblemReports_AspNetUsers_UserId",
                table: "ProblemReports");

            migrationBuilder.DropIndex(
                name: "IX_ProblemReports_UserId",
                table: "ProblemReports");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ProblemReports");

            migrationBuilder.DropColumn(
                name: "Request",
                table: "ProblemReports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProblemReports");

            migrationBuilder.DropColumn(
                name: "UserImgRequest",
                table: "ProblemReports");
        }
    }
}
