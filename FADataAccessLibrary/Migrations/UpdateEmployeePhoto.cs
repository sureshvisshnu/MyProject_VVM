using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateEmployeePhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "EmployeePhoto",
                table: "Employees",
                type: "longblob", 
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeePhoto",
                table: "Employees");
        }
    }
}

