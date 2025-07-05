using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class DebiteNoteChangeCustomertoAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditNotes_Customers_CustomerId",
                table: "CreditNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_DebitNotes_Suppliers_SupplierId",
                table: "DebitNotes");

            migrationBuilder.RenameColumn(
                name: "SupplierId",
                table: "DebitNotes",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_DebitNotes_SupplierId",
                table: "DebitNotes",
                newName: "IX_DebitNotes_AccountId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CreditNotes",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_CreditNotes_CustomerId",
                table: "CreditNotes",
                newName: "IX_CreditNotes_AccountId");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 8, 23, 15, 34, 55, 714, DateTimeKind.Local).AddTicks(6876));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 8, 23, 15, 34, 55, 714, DateTimeKind.Local).AddTicks(6897));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 8, 23, 15, 34, 55, 714, DateTimeKind.Local).AddTicks(6902));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 8, 23, 15, 34, 55, 714, DateTimeKind.Local).AddTicks(6906));

            migrationBuilder.AddForeignKey(
                name: "FK_CreditNotes_Accounts_AccountId",
                table: "CreditNotes",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNotes_Accounts_AccountId",
                table: "DebitNotes",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditNotes_Accounts_AccountId",
                table: "CreditNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_DebitNotes_Accounts_AccountId",
                table: "DebitNotes");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "DebitNotes",
                newName: "SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_DebitNotes_AccountId",
                table: "DebitNotes",
                newName: "IX_DebitNotes_SupplierId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "CreditNotes",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_CreditNotes_AccountId",
                table: "CreditNotes",
                newName: "IX_CreditNotes_CustomerId");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 8, 23, 13, 5, 9, 189, DateTimeKind.Local).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 8, 23, 13, 5, 9, 189, DateTimeKind.Local).AddTicks(7759));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 8, 23, 13, 5, 9, 189, DateTimeKind.Local).AddTicks(7764));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 8, 23, 13, 5, 9, 189, DateTimeKind.Local).AddTicks(7769));

            migrationBuilder.AddForeignKey(
                name: "FK_CreditNotes_Customers_CustomerId",
                table: "CreditNotes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DebitNotes_Suppliers_SupplierId",
                table: "DebitNotes",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }
    }
}
