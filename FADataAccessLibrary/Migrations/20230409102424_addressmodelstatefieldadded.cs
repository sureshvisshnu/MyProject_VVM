using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class addressmodelstatefieldadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StatesId",
                table: "Addresses",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StatesId",
                table: "Addresses",
                column: "StatesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_States_StatesId",
                table: "Addresses",
                column: "StatesId",
                principalTable: "States",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_States_StatesId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_StatesId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "StatesId",
                table: "Addresses");
        }
    }
}
