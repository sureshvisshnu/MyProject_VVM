using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddUpiTransactionPaymentInPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpiTransactionDate",
                table: "Payments",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpiTransactionId",
                table: "Payments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpiTransactionNumber",
                table: "Payments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 7, 11, 21, 22, 817, DateTimeKind.Local).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 7, 11, 21, 22, 817, DateTimeKind.Local).AddTicks(7079));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 7, 11, 21, 22, 817, DateTimeKind.Local).AddTicks(7084));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 7, 11, 21, 22, 817, DateTimeKind.Local).AddTicks(7088));

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UpiTransactionId",
                table: "Payments",
                column: "UpiTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Accounts_UpiTransactionId",
                table: "Payments",
                column: "UpiTransactionId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Accounts_UpiTransactionId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UpiTransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpiTransactionDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpiTransactionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpiTransactionNumber",
                table: "Payments");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 6, 16, 9, 9, 797, DateTimeKind.Local).AddTicks(7677));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 6, 16, 9, 9, 797, DateTimeKind.Local).AddTicks(7699));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 6, 16, 9, 9, 797, DateTimeKind.Local).AddTicks(7704));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 3, 6, 16, 9, 9, 797, DateTimeKind.Local).AddTicks(7709));
        }
    }
}
