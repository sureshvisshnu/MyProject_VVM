using System;
using fa.model.Catalog;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class InventoryStockIndexUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
            name: "IX_Inventories_CompanyId_InventoryLocationId_ProductId",
            table: "Inventories",
            columns: new[] { "CompanyId", "InventoryLocationId", "ProductId" });

            migrationBuilder.CreateIndex(
            name: "IX_InventoryBatches_CompanyId_InventoryId",
            table: "InventoryBatches",
            columns: new[] { "CompanyId", "InventoryId" });

            migrationBuilder.CreateIndex(
            name: "IX_stockmovements_CompanyId_InventoryStockLocationId",
            table: "stockmovements",
            columns: new[] { "CompanyId", "InventoryStockLocationId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Inventories_CompanyId_InventoryLocationId_ProductId", table: "Inventories");
            migrationBuilder.DropIndex(name: "IX_InventoryBatches_CompanyId_InventoryId", table: "InventoryBatches");
            migrationBuilder.DropIndex(name: "IX_stockmovements_CompanyId_InventoryStockLocationId", table: "stockmovements");

        }
    }
}

