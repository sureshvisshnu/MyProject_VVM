using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class Updateconsultedconsultation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                 name: "ConsultationNoteId",
                 table: "ConsultedConsultationFees",
                 type: "bigint",
                 nullable: true,
                 oldClrType: typeof(long),
                 oldType: "bigint");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
               name: "ConsultationNoteId",
               table: "ConsultedConsultationFees");

        }
    }
}
