using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class modelchangeforsaletax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CountrySaleTaxId",
                table: "CompanySalesTaxAccountMaps",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CountrySaleTaxs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rule = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountrySaleTaxs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountrySaleTaxs_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "CountrySaleTaxs",
                columns: new[] { "Id", "CountryId", "CreatedBy", "CreatedDate", "Discription", "LastModifiedBy", "LastModifiedDate", "Name", "Rule" },
                values: new object[,]
                {
                    { 1L, 99L, null, null, "Integrated Sales Tax Payable Account", null, null, "IGST", "RunIGST()" },
                    { 2L, 99L, null, null, "Central Sales Tax Payable Account", null, null, "CGST", "RunCGST()" },
                    { 3L, 99L, null, null, "State Sales Tax Payable Account", null, null, "SGST", "RunSGST()" },
                    { 4L, 99L, null, null, "Tax at Source Payable Account", null, null, "TCS", "RunTCS()" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanySalesTaxAccountMaps_CountrySaleTaxId",
                table: "CompanySalesTaxAccountMaps",
                column: "CountrySaleTaxId");

            migrationBuilder.CreateIndex(
                name: "IX_CountrySaleTaxs_CountryId",
                table: "CountrySaleTaxs",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySalesTaxAccountMaps_CountrySaleTaxs_CountrySaleTaxId",
                table: "CompanySalesTaxAccountMaps",
                column: "CountrySaleTaxId",
                principalTable: "CountrySaleTaxs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanySalesTaxAccountMaps_CountrySaleTaxs_CountrySaleTaxId",
                table: "CompanySalesTaxAccountMaps");

            migrationBuilder.DropTable(
                name: "CountrySaleTaxs");

            migrationBuilder.DropIndex(
                name: "IX_CompanySalesTaxAccountMaps_CountrySaleTaxId",
                table: "CompanySalesTaxAccountMaps");

            migrationBuilder.DropColumn(
                name: "CountrySaleTaxId",
                table: "CompanySalesTaxAccountMaps");
        }
    }
}
