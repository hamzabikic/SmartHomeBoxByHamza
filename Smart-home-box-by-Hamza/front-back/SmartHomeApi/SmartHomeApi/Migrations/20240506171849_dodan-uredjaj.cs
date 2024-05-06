using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHomeApi.Migrations
{
    public partial class dodanuredjaj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "JeUredjaj",
                table: "Prijave",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JeUredjaj",
                table: "Prijave");
        }
    }
}
