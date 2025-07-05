using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class updatetaxcodeuniquebycompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IDX_HsnCode",
                table: "itemtaxes");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "EffectiveFrom", "EffectiveTo" },
                values: new object[] { new DateTime(2017, 9, 1, 22, 23, 57, 322, DateTimeKind.Local).AddTicks(3043), new DateTime(2400, 6, 20, 22, 23, 57, 322, DateTimeKind.Local).AddTicks(3053) });

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "EffectiveFrom", "EffectiveTo" },
                values: new object[] { new DateTime(2017, 9, 1, 22, 23, 57, 322, DateTimeKind.Local).AddTicks(3060), new DateTime(2400, 6, 20, 22, 23, 57, 322, DateTimeKind.Local).AddTicks(3060) });

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "EffectiveFrom", "EffectiveTo" },
                values: new object[] { new DateTime(2017, 9, 1, 22, 23, 57, 322, DateTimeKind.Local).AddTicks(3062), new DateTime(2400, 6, 20, 22, 23, 57, 322, DateTimeKind.Local).AddTicks(3063) });

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "EffectiveFrom", "EffectiveTo" },
                values: new object[] { new DateTime(2017, 9, 1, 22, 23, 57, 322, DateTimeKind.Local).AddTicks(3064), new DateTime(2400, 6, 20, 22, 23, 57, 322, DateTimeKind.Local).AddTicks(3065) });
           
            migrationBuilder.CreateIndex(
                name: "IDX_HsnCode",
                table: "itemtaxes",
                columns: new[] { "Code", "CompanyId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IDX_HsnCode",
                table: "itemtaxes");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "EffectiveFrom", "EffectiveTo" },
                values: new object[] { new DateTime(2023, 6, 20, 12, 59, 58, 95, DateTimeKind.Local).AddTicks(6167), new DateTime(2400, 6, 20, 12, 59, 58, 95, DateTimeKind.Local).AddTicks(6178) });

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "EffectiveFrom", "EffectiveTo" },
                values: new object[] { new DateTime(2023, 6, 20, 12, 59, 58, 95, DateTimeKind.Local).AddTicks(6186), new DateTime(2400, 6, 20, 12, 59, 58, 95, DateTimeKind.Local).AddTicks(6187) });

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "EffectiveFrom", "EffectiveTo" },
                values: new object[] { new DateTime(2023, 6, 20, 12, 59, 58, 95, DateTimeKind.Local).AddTicks(6199), new DateTime(2400, 6, 20, 12, 59, 58, 95, DateTimeKind.Local).AddTicks(6199) });

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "EffectiveFrom", "EffectiveTo" },
                values: new object[] { new DateTime(2023, 6, 20, 12, 59, 58, 95, DateTimeKind.Local).AddTicks(6203), new DateTime(2400, 6, 20, 12, 59, 58, 95, DateTimeKind.Local).AddTicks(6203) });

            migrationBuilder.CreateIndex(
                name: "IDX_HsnCode",
                table: "itemtaxes",
                column: "Code");
        }
    }
}
