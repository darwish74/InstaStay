using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReviewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviews_AspNetUsers_UserId1",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_reviews_UserId1",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "reviews");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "reviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewText",
                table: "reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_UserId",
                table: "reviews",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_AspNetUsers_UserId",
                table: "reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviews_AspNetUsers_UserId",
                table: "reviews");

            migrationBuilder.DropIndex(
                name: "IX_reviews_UserId",
                table: "reviews");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReviewText",
                table: "reviews",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reviews_UserId1",
                table: "reviews",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_AspNetUsers_UserId1",
                table: "reviews",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
