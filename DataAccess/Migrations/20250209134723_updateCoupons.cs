﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateCoupons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HotelManagerId",
                table: "Coupons",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_HotelManagerId",
                table: "Coupons",
                column: "HotelManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupons_AspNetUsers_HotelManagerId",
                table: "Coupons",
                column: "HotelManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupons_AspNetUsers_HotelManagerId",
                table: "Coupons");

            migrationBuilder.DropIndex(
                name: "IX_Coupons_HotelManagerId",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "HotelManagerId",
                table: "Coupons");
        }
    }
}
