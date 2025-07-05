using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateMedicalTestWithCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MedicalTestCategoryId",
                table: "MedicalTests",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalTestCategorys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayAs = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSubMedicalTestCategory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentMedicalTestCategoryId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_MedicalTestCategorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalTestCategorys_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalTestCategorys_MedicalTestCategorys_ParentMedicalTestC~",
                        column: x => x.ParentMedicalTestCategoryId,
                        principalTable: "MedicalTestCategorys",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTests_MedicalTestCategoryId",
                table: "MedicalTests",
                column: "MedicalTestCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTestCategorys_CompanyId",
                table: "MedicalTestCategorys",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTestCategorys_ParentMedicalTestCategoryId",
                table: "MedicalTestCategorys",
                column: "ParentMedicalTestCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalTests_MedicalTestCategorys_MedicalTestCategoryId",
                table: "MedicalTests",
                column: "MedicalTestCategoryId",
                principalTable: "MedicalTestCategorys",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalTests_MedicalTestCategorys_MedicalTestCategoryId",
                table: "MedicalTests");

            migrationBuilder.DropTable(
                name: "MedicalTestCategorys");

            migrationBuilder.DropIndex(
                name: "IX_MedicalTests_MedicalTestCategoryId",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "MedicalTestCategoryId",
                table: "MedicalTests");
        }
    }
}
