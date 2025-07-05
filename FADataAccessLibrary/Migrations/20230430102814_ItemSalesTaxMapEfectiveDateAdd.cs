using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class ItemSalesTaxMapEfectiveDateAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveFromDate",
                table: "ItemSalesTaxMaps",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveToDate",
                table: "ItemSalesTaxMaps",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveFromDate",
                table: "ItemSalesTaxMaps");

            migrationBuilder.DropColumn(
                name: "EffectiveToDate",
                table: "ItemSalesTaxMaps");

            
        }
    }
}
