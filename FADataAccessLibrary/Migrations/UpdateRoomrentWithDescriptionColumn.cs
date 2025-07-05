using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateRoomrentWithDescriptionColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PatientRoomRents",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "RentStartDate",
                table: "PatientRoomRents",
                type: "datetime(6)",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RentEndDate",
                table: "PatientRoomRents",
                type: "datetime(6)",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "PatientRoomRents");

            migrationBuilder.DropColumn(
                name: "RentStartDate",
                table: "PatientRoomRents");

            migrationBuilder.DropColumn(
                name: "RentEndDate",
                table: "PatientRoomRents");
        }
    }
}
