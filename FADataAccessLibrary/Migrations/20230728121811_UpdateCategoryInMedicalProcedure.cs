using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateCategoryInMedicalProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MedicalProcedureCategoryId",
                table: "MedicalProcedures",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MedicalProcedureCategory",
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
                    IsSubMedicalProcedureCategory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentMedicalProcedureCategoryId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_MedicalProcedureCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalProcedureCategory_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalProcedureCategory_MedicalTestCategorys_ParentMedicalP~",
                        column: x => x.ParentMedicalProcedureCategoryId,
                        principalTable: "MedicalTestCategorys",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProcedures_MedicalProcedureCategoryId",
                table: "MedicalProcedures",
                column: "MedicalProcedureCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProcedureCategory_CompanyId",
                table: "MedicalProcedureCategory",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalProcedureCategory_ParentMedicalProcedureCategoryId",
                table: "MedicalProcedureCategory",
                column: "ParentMedicalProcedureCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalProcedures_MedicalProcedureCategory_MedicalProcedureC~",
                table: "MedicalProcedures",
                column: "MedicalProcedureCategoryId",
                principalTable: "MedicalProcedureCategory",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalProcedures_MedicalProcedureCategory_MedicalProcedureC~",
                table: "MedicalProcedures");

            migrationBuilder.DropTable(
                name: "MedicalProcedureCategory");

            migrationBuilder.DropIndex(
                name: "IX_MedicalProcedures_MedicalProcedureCategoryId",
                table: "MedicalProcedures");

            migrationBuilder.DropColumn(
                name: "MedicalProcedureCategoryId",
                table: "MedicalProcedures");
        }
    }
}
