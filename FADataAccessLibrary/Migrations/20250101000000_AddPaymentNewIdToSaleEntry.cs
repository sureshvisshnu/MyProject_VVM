using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class AddPaymentNewIdToSaleEntry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PaymentNewId",
                table: "saleentries",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_PaymentNewId",
                table: "saleentries",
                column: "PaymentNewId");

            migrationBuilder.AddForeignKey(
                name: "FK_saleentries_PaymentsNew_PaymentNewId",
                table: "saleentries",
                column: "PaymentNewId",
                principalTable: "PaymentsNew",
                principalColumn: "PaymentNewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_saleentries_PaymentsNew_PaymentNewId",
                table: "saleentries");

            migrationBuilder.DropIndex(
                name: "IX_saleentries_PaymentNewId",
                table: "saleentries");

            migrationBuilder.DropColumn(
                name: "PaymentNewId",
                table: "saleentries");
        }
    }
}
