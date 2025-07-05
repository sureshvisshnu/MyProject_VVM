using System;
using fa.model.Catalog;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class ItemReportIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CatalogItems_Type_ParentId",
                table: "CatalogItems",
                columns: new[] { "Type", "ParentId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.DropIndex(name: "IX_CatalogItems_Type_ParentId", table: "CatalogItems");
        }
    }
}
