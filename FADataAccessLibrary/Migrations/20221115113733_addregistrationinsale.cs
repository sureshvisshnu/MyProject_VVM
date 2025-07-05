using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class addregistrationinsale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RegistrationId",
                table: "saleentries",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_saleentries_RegistrationId",
                table: "saleentries",
                column: "RegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_saleentries_registrations_RegistrationId",
                table: "saleentries",
                column: "RegistrationId",
                principalTable: "registrations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_saleentries_registrations_RegistrationId",
                table: "saleentries");

            migrationBuilder.DropIndex(
                name: "IX_saleentries_RegistrationId",
                table: "saleentries");

            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "saleentries");
        }
    }
}
