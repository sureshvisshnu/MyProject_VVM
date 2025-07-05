using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class addcatalogitemsaletaxeffectivedate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveFrom",
                table: "CatalogItemSalesTaxMaps",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveTo",
                table: "CatalogItemSalesTaxMaps",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EffectiveFrom",
                table: "CatalogItemSalesTaxMaps");

            migrationBuilder.DropColumn(
                name: "EffectiveTo",
                table: "CatalogItemSalesTaxMaps");

           
        }
    }
}
