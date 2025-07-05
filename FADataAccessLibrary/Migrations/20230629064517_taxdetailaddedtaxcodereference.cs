using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class taxdetailaddedtaxcodereference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ItemCodeTaxMapId",
                table: "TaxDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxDetails_ItemCodeTaxMapId",
                table: "TaxDetails",
                column: "ItemCodeTaxMapId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaxDetails_ItemSalesTaxMaps_ItemCodeTaxMapId",
                table: "TaxDetails",
                column: "ItemCodeTaxMapId",
                principalTable: "ItemSalesTaxMaps",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxDetails_ItemSalesTaxMaps_ItemCodeTaxMapId",
                table: "TaxDetails");

            migrationBuilder.DropIndex(
                name: "IX_TaxDetails_ItemCodeTaxMapId",
                table: "TaxDetails");
            
            migrationBuilder.DropColumn(
                name: "ItemCodeTaxMapId",
                table: "TaxDetails");           
        }
    }
}
