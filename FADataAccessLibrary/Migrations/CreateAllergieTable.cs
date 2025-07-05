using System;
using fa.model.Catalog;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class CreateAllergieTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.CreateTable(
                name: "allergies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    allergieCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    Codes = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Class = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReasonForInactive = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discriminator = table.Column<string>(type: "longtext", nullable: false)
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
                    table.PrimaryKey("PK_allergies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_allergies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");


            migrationBuilder.CreateTable(
                name: "allergieCategorys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Discription = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSuballergieCategory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentallergieCategoryId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_allergieCategorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_allergieCategorys_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_allergieCategorys_allergieCategorys_ParentallergieCategoryId",
                        column: x => x.ParentallergieCategoryId,
                        principalTable: "allergieCategorys",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_allergies_allergieCategoryId",
                table: "allergies",
                column: "allergieCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_allergieCategorys_CompanyId",
                table: "allergieCategorys",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_allergieCategorys_ParentallergieCategoryId",
                table: "allergieCategorys",
                column: "ParentallergieCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_allergies_allergieCategorys_allergieCategoryId",
                table: "allergies",
                column: "allergieCategoryId",
                principalTable: "allergieCategorys",
                principalColumn: "Id");

            migrationBuilder.AddColumn<long>(
                name: "AllergieId",
                table: "Keywords",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Keywords_allergies_allergieId",
                table: "Keywords",
                column: "allergieId",
                principalTable: "Allergies",
                principalColumn: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Keywords_allergieId",
                table: "Keywords",
                column: "allergieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "allergies");

            migrationBuilder.DropForeignKey(
                name: "FK_allergies_allergieCategorys_allergieCategoryId",
                table: "allergies");

            migrationBuilder.DropTable(
                name: "allergieCategorys");

            migrationBuilder.DropIndex(
                name: "IX_allergies_allergieCategoryId",
                table: "allergies");

            migrationBuilder.DropColumn(
                name: "allergieCategoryId",
                table: "allergies");
        }
    }
}
