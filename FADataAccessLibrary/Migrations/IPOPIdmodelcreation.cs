using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class IPOPIdmodelcreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PatientOPNumber",
                table: "Registrations",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientIPNumber",
                table: "InPatientAdmissions",
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PatientOPIds",
                columns: table => new
                {
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NextNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientOPIds", x => new { x.CompanyId, x.Date });
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
               name: "PatientIPIds",
               columns: table => new
               {
                   CompanyId = table.Column<long>(type: "bigint", nullable: false),
                   Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                   NextNumber = table.Column<int>(type: "int", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_PatientIPIds", x => new { x.CompanyId, x.Date });
               })
               .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PatientOPNumber",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "PatientIPNumber",
                table: "InPatientAdmissions");
        }
    }
}
