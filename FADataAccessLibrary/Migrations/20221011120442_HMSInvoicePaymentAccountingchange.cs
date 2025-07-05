using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class HMSInvoicePaymentAccountingchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "PatientPaymentDetails",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PatientId",
                table: "daybooks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientPaymentDetails_AccountId",
                table: "PatientPaymentDetails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_daybooks_PatientId",
                table: "daybooks",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_daybooks_Patients_PatientId",
                table: "daybooks",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientPaymentDetails_Accounts_AccountId",
                table: "PatientPaymentDetails",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_daybooks_Patients_PatientId",
                table: "daybooks");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientPaymentDetails_Accounts_AccountId",
                table: "PatientPaymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_PatientPaymentDetails_AccountId",
                table: "PatientPaymentDetails");

            migrationBuilder.DropIndex(
                name: "IX_daybooks_PatientId",
                table: "daybooks");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "PatientPaymentDetails");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "daybooks");
        }
    }
}
