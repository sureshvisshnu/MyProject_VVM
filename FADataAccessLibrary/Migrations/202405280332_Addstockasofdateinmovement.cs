using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class Addstockasofdateinmovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.AddColumn<string>(
                 name: "StockDate",
                 table: "StockMovementDetails",
                 type: "DATETIME(6)",
                 nullable: true);          
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        migrationBuilder.DropColumn(
                name: "StockDate",
                table: "StockMovementDetails");
        }
    }
}
