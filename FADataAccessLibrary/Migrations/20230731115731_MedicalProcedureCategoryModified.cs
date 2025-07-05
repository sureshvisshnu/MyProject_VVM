using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class MedicalProcedureCategoryModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalProcedureCategory_MedicalTestCategorys_ParentMedicalP~",
                table: "MedicalProcedureCategory");

           migrationBuilder.AddForeignKey(
                name: "FK_MedicalProcedureCategory_MedicalProcedureCategory_ParentMedi~",
                table: "MedicalProcedureCategory",
                column: "ParentMedicalProcedureCategoryId",
                principalTable: "MedicalProcedureCategory",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalProcedureCategory_MedicalProcedureCategory_ParentMedi~",
                table: "MedicalProcedureCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalProcedureCategory_MedicalTestCategorys_ParentMedicalP~",
                table: "MedicalProcedureCategory",
                column: "ParentMedicalProcedureCategoryId",
                principalTable: "MedicalTestCategorys",
                principalColumn: "Id");
        }
    }
}
