using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAdditionalTransactionWithInvoiceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "AdditionalTransactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 30, 14, 51, 54, 10, DateTimeKind.Local).AddTicks(2637));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 30, 14, 51, 54, 10, DateTimeKind.Local).AddTicks(2661));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 30, 14, 51, 54, 10, DateTimeKind.Local).AddTicks(2666));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 30, 14, 51, 54, 10, DateTimeKind.Local).AddTicks(2670));

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalTransactions_InvoiceId",
                table: "AdditionalTransactions",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdditionalTransactions_Invoices_InvoiceId",
                table: "AdditionalTransactions",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdditionalTransactions_Invoices_InvoiceId",
                table: "AdditionalTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AdditionalTransactions_InvoiceId",
                table: "AdditionalTransactions");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "AdditionalTransactions");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 30, 14, 50, 21, 59, DateTimeKind.Local).AddTicks(663));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 30, 14, 50, 21, 59, DateTimeKind.Local).AddTicks(696));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 30, 14, 50, 21, 59, DateTimeKind.Local).AddTicks(701));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 12, 30, 14, 50, 21, 59, DateTimeKind.Local).AddTicks(706));
        }
    }
}
