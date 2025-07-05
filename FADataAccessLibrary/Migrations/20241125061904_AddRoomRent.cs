using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class AddRoomRent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientRoomRents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntryDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    InPatientAdmissionId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientRoomRents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientRoomRents_Patient_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientRoomRents_InPatientAdmission_InPatientAdmissionId",
                        column: x => x.InPatientAdmissionId,
                        principalTable: "InPatientAdmissions",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientRoomRents");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientRoomRents_Patient_PatientId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientRoomRents_InPatientAdmission_InPatientAdmissionId",
                table: "InPatientAdmissions");
        }
    }
}
