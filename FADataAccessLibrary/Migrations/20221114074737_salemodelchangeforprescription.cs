using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class salemodelchangeforprescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PatientId",
                table: "saleentries",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_PatientId",
                table: "saleentries",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_saleentries_Patients_PatientId",
                table: "saleentries",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_saleentries_Patients_PatientId",
                table: "saleentries");

            migrationBuilder.DropIndex(
                name: "IX_saleentries_PatientId",
                table: "saleentries");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "saleentries");
        }
    }
}
