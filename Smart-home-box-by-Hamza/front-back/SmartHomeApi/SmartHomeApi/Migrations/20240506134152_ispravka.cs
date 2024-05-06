using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHomeApi.Migrations
{
    public partial class ispravka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "apiKey",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "appId",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "authDomain",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "databaseURL",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "messagingSenderId",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "projectID",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "storageBucket",
                table: "Korisnici");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "apiKey",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "appId",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "authDomain",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "databaseURL",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "messagingSenderId",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "projectID",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "storageBucket",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
