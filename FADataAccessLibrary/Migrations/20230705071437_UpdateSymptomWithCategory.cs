using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateSymptomWithCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SymptomCategoryId",
                table: "Symptoms",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SymptomCategorys",
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
                    IsSubSymptomCategory = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ParentSymptomCategoryId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_SymptomCategorys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SymptomCategorys_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SymptomCategorys_SymptomCategorys_ParentSymptomCategoryId",
                        column: x => x.ParentSymptomCategoryId,
                        principalTable: "SymptomCategorys",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            
            migrationBuilder.CreateIndex(
                name: "IX_Symptoms_SymptomCategoryId",
                table: "Symptoms",
                column: "SymptomCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomCategorys_CompanyId",
                table: "SymptomCategorys",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SymptomCategorys_ParentSymptomCategoryId",
                table: "SymptomCategorys",
                column: "ParentSymptomCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Symptoms_SymptomCategorys_SymptomCategoryId",
                table: "Symptoms",
                column: "SymptomCategoryId",
                principalTable: "SymptomCategorys",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Symptoms_SymptomCategorys_SymptomCategoryId",
                table: "Symptoms");

            migrationBuilder.DropTable(
                name: "SymptomCategorys");

            migrationBuilder.DropIndex(
                name: "IX_Symptoms_SymptomCategoryId",
                table: "Symptoms");

            migrationBuilder.DropColumn(
                name: "SymptomCategoryId",
                table: "Symptoms");

           
        }
    }
}
