using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace App.WebInfo.DataAccess.Migrations
{
    public partial class Cemq01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Bedeni",
                table: "Personal",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AcikAdres",
                table: "Personal",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcikAdres",
                table: "Personal");

            migrationBuilder.AlterColumn<int>(
                name: "Bedeni",
                table: "Personal",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
