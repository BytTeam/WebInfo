using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.WebInfo.DataAccess.Migrations
{
    public partial class WebInfoInstance10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Personal_DogumYeri_DogumYeriId",
                table: "Personal");

            migrationBuilder.DropIndex(
                name: "IX_Personal_DogumYeriId",
                table: "Personal");

            migrationBuilder.DropColumn(
                name: "DogumYeriId",
                table: "Personal");

            migrationBuilder.AddColumn<string>(
                name: "DogumYeri",
                table: "Personal",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DogumYeri",
                table: "Personal");

            migrationBuilder.AddColumn<int>(
                name: "DogumYeriId",
                table: "Personal",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Personal_DogumYeriId",
                table: "Personal",
                column: "DogumYeriId");

            migrationBuilder.AddForeignKey(
                name: "FK_Personal_DogumYeri_DogumYeriId",
                table: "Personal",
                column: "DogumYeriId",
                principalTable: "DogumYeri",
                principalColumn: "DogumYeriId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
