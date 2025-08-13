using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddLinePriceToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<float>(
                name: "LinePrice",
                table: "CatalogItems",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "SpecialPrice",
                table: "CatalogItems",
                type: "float",
                nullable: true);

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinePrice",
                table: "CatalogItems");

            migrationBuilder.DropColumn(
                name: "SpecialPrice",
                table: "CatalogItems");

        }
    }
}
