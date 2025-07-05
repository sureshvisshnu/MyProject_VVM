using System;
using fa.model.Catalog;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class CatalogItemIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CompanyId_Type_Name",
                table: "CatalogItems",
                columns: new[] { "CompanyId", "Type", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CompanyId_Type_Name_ParentId",
                table: "CatalogItems",
                columns: new[] { "CompanyId", "Type", "Name", "ParentId" });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CompanyId_material",
                table: "CatalogItems",
                columns: new[] { "CompanyId", "materialid" });
            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_CompanyId_Type_Name_ParentId_Id",
                table: "CatalogItems",
                columns: new[] { "CompanyId", "Type", "Name", "ParentId", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogItemSalesTaxMap_CatalogItemId_SalesTaxMapId_EffectiveFrom_EffectiveTo",
                table: "CatalogItemSalesTaxMaps",
                columns: new[] { "CatalogItemId", "SalesTaxMapId", "EffectiveFrom", "EffectiveTo" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_CatalogItems_CompanyId_Type_Name", table: "CatalogItems");
            migrationBuilder.DropIndex(name: "IX_CatalogItems_CompanyId_Type_Name_ParentId", table: "CatalogItems");
            migrationBuilder.DropIndex(name: "IX_CatalogItems_CompanyId_material", table: "CatalogItems");
            migrationBuilder.DropIndex(name: "IX_CatalogItems_CompanyId_Type_Name_ParentId_Id", table: "CatalogItems");
            migrationBuilder.DropIndex(name: "IX_CatalogItemSalesTaxMap_CatalogItemId_SalesTaxMapId_EffectiveFrom_EffectiveTo", table: "CatalogItemSalesTaxMaps");
        }
    }
}
