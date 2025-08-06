using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    /// <inheritdoc />
    public partial class BarCodeLabelCounter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "RibbonUsages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RibbonName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RibbonType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalPrintLengthM = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsedLengthM = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RibbonUsages", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LabelStockMasters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LabelType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LabelSizeCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RollName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalLabelCount = table.Column<int>(type: "int", nullable: false),
                    LabelsPerRow = table.Column<int>(type: "int", nullable: false),
                    LabelsUsed = table.Column<int>(type: "int", nullable: false),
                    WastedLabelCount = table.Column<int>(type: "int", nullable: false),
                    RemainingCount = table.Column<int>(type: "int", nullable: false),
                    ThresholdWarning = table.Column<int>(type: "int", nullable: false),
                    DateLoaded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateEnded = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RibbonId = table.Column<long>(type: "bigint", nullable: true),
                    Comments = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelStockMasters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabelStockMasters_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelStockMasters_RibbonUsages_RibbonId",
                        column: x => x.RibbonId,
                        principalTable: "RibbonUsages",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LabelStockUsages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LabelStockId = table.Column<long>(type: "bigint", nullable: false),
                    DatePrinted = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PrintedCount = table.Column<int>(type: "int", nullable: false),
                    WastedCount = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrintedBy = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RibbonUsedLengthM = table.Column<decimal>(type: "decimal(65,30)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelStockUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabelStockUsages_LabelStockMasters_LabelStockId",
                        column: x => x.LabelStockId,
                        principalTable: "LabelStockMasters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
            

            migrationBuilder.CreateIndex(
                name: "IX_LabelStockMasters_CompanyId",
                table: "LabelStockMasters",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelStockMasters_RibbonId",
                table: "LabelStockMasters",
                column: "RibbonId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelStockUsages_LabelStockId",
                table: "LabelStockUsages",
                column: "LabelStockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelStockUsages");

            migrationBuilder.DropTable(
                name: "LabelStockMasters");

            migrationBuilder.DropTable(
                name: "RibbonUsages");


        }
    }
}
