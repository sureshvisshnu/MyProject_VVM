using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateRoundOffAccountInCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RoundOffAccountId",
                table: "Companies",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 15, 17, 57, 52, 987, DateTimeKind.Local).AddTicks(6495));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 15, 17, 57, 52, 987, DateTimeKind.Local).AddTicks(6513));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 15, 17, 57, 52, 987, DateTimeKind.Local).AddTicks(6518));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 15, 17, 57, 52, 987, DateTimeKind.Local).AddTicks(6522));

            migrationBuilder.CreateIndex(
                name: "IX_Companies_RoundOffAccountId",
                table: "Companies",
                column: "RoundOffAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Accounts_RoundOffAccountId",
                table: "Companies",
                column: "RoundOffAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Accounts_RoundOffAccountId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_RoundOffAccountId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "RoundOffAccountId",
                table: "Companies");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 15, 17, 55, 58, 390, DateTimeKind.Local).AddTicks(2156));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 15, 17, 55, 58, 390, DateTimeKind.Local).AddTicks(2178));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 15, 17, 55, 58, 390, DateTimeKind.Local).AddTicks(2183));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 10, 15, 17, 55, 58, 390, DateTimeKind.Local).AddTicks(2188));
        }
    }
}
