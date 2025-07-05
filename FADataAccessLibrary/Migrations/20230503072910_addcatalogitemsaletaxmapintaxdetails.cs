using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class addcatalogitemsaletaxmapintaxdetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ItemTaxMapId",
                table: "TaxDetails",
                type: "bigint",
                nullable: true);

            

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_ItemTaxMapId",
                table: "TaxDetails",
                column: "ItemTaxMapId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxDetails_CatalogItemSalesTaxMaps_ItemTaxMapId",
                table: "TaxDetails",
                column: "ItemTaxMapId",
                principalTable: "CatalogItemSalesTaxMaps",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxDetails_CatalogItemSalesTaxMaps_ItemTaxMapId",
                table: "TaxDetails");

            migrationBuilder.DropIndex(
                name: "IX_TaxDetails_ItemTaxMapId",
                table: "TaxDetails");

            migrationBuilder.DropColumn(
                name: "ItemTaxMapId",
                table: "TaxDetails");

           
        }
    }
}
