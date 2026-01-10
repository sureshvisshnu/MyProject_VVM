using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ADDEDRemovedFeild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Accounts_AccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Companies_CompanyId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CostCenters_CostCenterId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_saleentries_SalesId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CostCenterId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_SalesId",
                table: "Payments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "ProductPercentages",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<long>(
                name: "SalesId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CostCenterId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CompanyId1",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "DeletedById",
                table: "Payments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Payments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletionReason",
                table: "Payments",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Payments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDueDate",
                table: "Payments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDueDateUtc",
                table: "Payments",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "SalesId",
                table: "PaymentDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 10, 14, 37, 41, 33, DateTimeKind.Local).AddTicks(25));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 10, 14, 37, 41, 33, DateTimeKind.Local).AddTicks(67));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 10, 14, 37, 41, 33, DateTimeKind.Local).AddTicks(78));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 10, 14, 37, 41, 33, DateTimeKind.Local).AddTicks(88));

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CompanyId1",
                table: "Payments",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DeletedById",
                table: "Payments",
                column: "DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentDetails_SalesId",
                table: "PaymentDetails",
                column: "SalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentDetails_saleentries_SalesId",
                table: "PaymentDetails",
                column: "SalesId",
                principalTable: "saleentries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Accounts_AccountId",
                table: "Payments",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Companies_CompanyId1",
                table: "Payments",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CostCenters_CompanyId",
                table: "Payments",
                column: "CompanyId",
                principalTable: "CostCenters",
                principalColumn: "CostCenterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_DeletedById",
                table: "Payments",
                column: "DeletedById",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentDetails_saleentries_SalesId",
                table: "PaymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Accounts_AccountId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Companies_CompanyId1",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CostCenters_CompanyId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_DeletedById",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CompanyId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_DeletedById",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_PaymentDetails_SalesId",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletionReason",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentDueDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentDueDateUtc",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SalesId",
                table: "PaymentDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RowVersion",
                table: "ProductPercentages",
                type: "timestamp(6)",
                rowVersion: true,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(6)",
                oldRowVersion: true,
                oldNullable: true)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

            migrationBuilder.AlterColumn<long>(
                name: "SalesId",
                table: "Payments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CostCenterId",
                table: "Payments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "AccountId",
                table: "Payments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 1L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 5, 21, 33, 40, 550, DateTimeKind.Local).AddTicks(2070));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 2L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 5, 21, 33, 40, 550, DateTimeKind.Local).AddTicks(2115));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 3L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 5, 21, 33, 40, 550, DateTimeKind.Local).AddTicks(2126));

            migrationBuilder.UpdateData(
                table: "CountrySaleTaxs",
                keyColumn: "Id",
                keyValue: 4L,
                column: "EffectiveTo",
                value: new DateTime(2400, 1, 5, 21, 33, 40, 550, DateTimeKind.Local).AddTicks(2135));

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CostCenterId",
                table: "Payments",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_SalesId",
                table: "Payments",
                column: "SalesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Accounts_AccountId",
                table: "Payments",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Companies_CompanyId",
                table: "Payments",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CostCenters_CostCenterId",
                table: "Payments",
                column: "CostCenterId",
                principalTable: "CostCenters",
                principalColumn: "CostCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_saleentries_SalesId",
                table: "Payments",
                column: "SalesId",
                principalTable: "saleentries",
                principalColumn: "Id");
        }
    }
}
