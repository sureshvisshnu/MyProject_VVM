using System;
using fa.model.Catalog;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class AccountIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Accounts_CompanyId_Name",
                table: "Accounts",
                columns: new[] { "CompanyId","Name"});
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_Accounts_CompanyId_Name", table: "Accounts");
        }
    }
}
