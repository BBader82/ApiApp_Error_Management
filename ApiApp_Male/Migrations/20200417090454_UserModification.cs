using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiApp_Male.Migrations
{
    public partial class UserModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "Passwordhash",
                table: "Users",
                maxLength: 200,
                nullable: false,
                defaultValue: new byte[] {  });

            migrationBuilder.AddColumn<byte[]>(
                name: "Passwordsalt",
                table: "Users",
                maxLength: 200,
                nullable: false,
                defaultValue: new byte[] {  });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Passwordhash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Passwordsalt",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
