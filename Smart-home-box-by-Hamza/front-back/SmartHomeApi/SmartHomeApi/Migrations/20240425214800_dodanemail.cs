using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHomeApi.Migrations
{
    public partial class dodanemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailSlanje",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SmsSlanje",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "WhatsAppSlanje",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailSlanje",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "SmsSlanje",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "WhatsAppSlanje",
                table: "Korisnici");
        }
    }
}
