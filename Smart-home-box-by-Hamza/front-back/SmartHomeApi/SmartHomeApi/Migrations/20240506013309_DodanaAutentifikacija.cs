using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartHomeApi.Migrations
{
    public partial class DodanaAutentifikacija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhatsAppSlanje",
                table: "Korisnici");

            migrationBuilder.AddColumn<int>(
                name: "KorisnikId",
                table: "TemperatureVlaznosti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KorisnikId",
                table: "SkeniraniPokreti",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KorisnikId",
                table: "Lights",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<int>(
                name: "KorisnikId",
                table: "CO2FireAlarmi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Prijave",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumVrijeme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAdresa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prijave", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prijave_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemperatureVlaznosti_KorisnikId",
                table: "TemperatureVlaznosti",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_SkeniraniPokreti_KorisnikId",
                table: "SkeniraniPokreti",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Lights_KorisnikId",
                table: "Lights",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_CO2FireAlarmi_KorisnikId",
                table: "CO2FireAlarmi",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Prijave_KorisnikId",
                table: "Prijave",
                column: "KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_CO2FireAlarmi_Korisnici_KorisnikId",
                table: "CO2FireAlarmi",
                column: "KorisnikId",
                principalTable: "Korisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lights_Korisnici_KorisnikId",
                table: "Lights",
                column: "KorisnikId",
                principalTable: "Korisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SkeniraniPokreti_Korisnici_KorisnikId",
                table: "SkeniraniPokreti",
                column: "KorisnikId",
                principalTable: "Korisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemperatureVlaznosti_Korisnici_KorisnikId",
                table: "TemperatureVlaznosti",
                column: "KorisnikId",
                principalTable: "Korisnici",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CO2FireAlarmi_Korisnici_KorisnikId",
                table: "CO2FireAlarmi");

            migrationBuilder.DropForeignKey(
                name: "FK_Lights_Korisnici_KorisnikId",
                table: "Lights");

            migrationBuilder.DropForeignKey(
                name: "FK_SkeniraniPokreti_Korisnici_KorisnikId",
                table: "SkeniraniPokreti");

            migrationBuilder.DropForeignKey(
                name: "FK_TemperatureVlaznosti_Korisnici_KorisnikId",
                table: "TemperatureVlaznosti");

            migrationBuilder.DropTable(
                name: "Prijave");

            migrationBuilder.DropIndex(
                name: "IX_TemperatureVlaznosti_KorisnikId",
                table: "TemperatureVlaznosti");

            migrationBuilder.DropIndex(
                name: "IX_SkeniraniPokreti_KorisnikId",
                table: "SkeniraniPokreti");

            migrationBuilder.DropIndex(
                name: "IX_Lights_KorisnikId",
                table: "Lights");

            migrationBuilder.DropIndex(
                name: "IX_CO2FireAlarmi_KorisnikId",
                table: "CO2FireAlarmi");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "TemperatureVlaznosti");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "SkeniraniPokreti");

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "Lights");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Korisnici");

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

            migrationBuilder.DropColumn(
                name: "KorisnikId",
                table: "CO2FireAlarmi");

            migrationBuilder.AddColumn<bool>(
                name: "WhatsAppSlanje",
                table: "Korisnici",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
