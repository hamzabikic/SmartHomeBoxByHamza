using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHomeApi.Migrations
{
    public partial class Migracija1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CO2FireAlarmi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CO2Value = table.Column<int>(type: "int", nullable: false),
                    FireDetected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CO2FireAlarmi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkeniraniPokreti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkeniraniPokreti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemperatureVlaznosti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Temperatura = table.Column<int>(type: "int", nullable: false),
                    Vlaznost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemperatureVlaznosti", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CO2FireAlarmi");

            migrationBuilder.DropTable(
                name: "SkeniraniPokreti");

            migrationBuilder.DropTable(
                name: "TemperatureVlaznosti");
        }
    }
}
