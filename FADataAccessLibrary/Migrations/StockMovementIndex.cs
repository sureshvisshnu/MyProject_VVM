using System;
using fa.model.Catalog;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class StockMovementIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.CreateIndex(
            name: "IX_Inventories_InventoryLocationId_ProductId",
            table: "Inventories",
            columns: new[] { "InventoryLocationId", "ProductId" });

            migrationBuilder.AlterColumn<string>(
                name: "BatchNo",
                table: "InventoryBatches",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
            name: "IX_InventoryBatches_InventoryId_BatchNo",
            table: "InventoryBatches",
            columns: new[] { "InventoryId", "BatchNo" });

            migrationBuilder.CreateIndex(
            name: "IX_StockMovement_CompanyId_Type",
            table: "Stockmovements",
            columns: new[] { "CompanyId", "Type" });

            migrationBuilder.AlterColumn<string>(
                name: "BatchNo",
                table: "StockMovementDetails",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.CreateIndex(
            name: "IX_IStockMovementDetails_ProductId_BatchNo_isBatch",
            table: "StockMovementDetails",
            columns: new[] { "ProductId", "BatchNo", "isBatch" });


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Inventories_InventoryLocationId_ProductId", table: "Inventories");
            migrationBuilder.DropIndex(name: "IX_InventoryBatches_InventoryId_BatchNo", table: "InventoryBatches");
            migrationBuilder.DropIndex(name: "IX_StockMovement_CompanyId_Type", table: "Stockmovements");
            migrationBuilder.DropIndex(name: "IX_IStockMovementDetail_ProductId_BatchNo_isBatch", table: "StockMovementDetails");
        }
    }
}
