using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class journalmodelchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalDetails_Accounts_ReferenceAccountId",
                table: "JournalDetails");

            migrationBuilder.AlterColumn<long>(
                name: "ReferenceAccountId",
                table: "JournalDetails",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<double>(
                name: "Balance",
                table: "Accounts",
                type: "double",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalDetails_Accounts_ReferenceAccountId",
                table: "JournalDetails",
                column: "ReferenceAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JournalDetails_Accounts_ReferenceAccountId",
                table: "JournalDetails");

            migrationBuilder.AlterColumn<long>(
                name: "ReferenceAccountId",
                table: "JournalDetails",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Balance",
                table: "Accounts",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");

            migrationBuilder.AddForeignKey(
                name: "FK_JournalDetails_Accounts_ReferenceAccountId",
                table: "JournalDetails",
                column: "ReferenceAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
