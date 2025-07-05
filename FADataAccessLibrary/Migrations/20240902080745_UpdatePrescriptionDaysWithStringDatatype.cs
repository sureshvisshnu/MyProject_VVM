using System;
using fa.context;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdatePrescriptionDaysWithStringDatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
            name: "Days",
            table: "Prescriptions",
            type: "longtext",
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
            name: "Days",
            table: "Prescriptions",
            type: "int",
            nullable: false,
            defaultValue: 0,
            oldClrType: typeof(string),
            oldType: "longtext",
            oldNullable: true);
        }
    }
}
