using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class Accountdisplayasandproductopenbalancedate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayAs",
                table: "accounts",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                 name: "StockDate",
                 table: "inventories",
                 type: "DATETIME(6)",
                 nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StockDate",
                table: "inventorybatches",
                type: "DATETIME(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "accounts",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "StockDate",
                table: "inventories");

            migrationBuilder.DropColumn(
                name: "StockDate",
                table: "inventorybatches");
        }
    }
}
