using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateAccountGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DebitMultiplier",
                table: "AccountGroups",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreditMultiplier",
                table: "AccountGroups",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DebitMultiplier",
                table: "AccountGroups");

            migrationBuilder.DropColumn(
                name: "CreditMultiplier",
                table: "AccountGroups");
        }
    }
}
