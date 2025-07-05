using System;
using fa.model.Catalog;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class CreateConsultedAllergieTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsultedAllergies",
                columns: table => new
                {
                    ConsultedAllergieId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConsultationNoteId = table.Column<long>(type: "bigint", nullable: false),
                    AllergieId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
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
                    table.PrimaryKey("PK_ConsultedAllergies", x => x.ConsultedAllergieId);
                    table.ForeignKey(
                        name: "FK_ConsultedAllergies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedAllergies_ConsultationNotes_ConsultationNoteId",
                        column: x => x.ConsultationNoteId,
                        principalTable: "ConsultationNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsultedAllergies_Allergies_AllergieId",
                        column: x => x.AllergieId,
                        principalTable: "Allergies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
              name: "IX_ConsultedAllergies_CompanyId",
              table: "ConsultedAllergies",
              column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedAllergies_ConsultationNoteId",
                table: "ConsultedAllergies",
                column: "ConsultationNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsultedAllergies_AllergieId",
                table: "ConsultedAllergies",
                column: "AllergieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {           
            migrationBuilder.DropTable(
                name: "ConsultedAllergies");

        }
    }
}
