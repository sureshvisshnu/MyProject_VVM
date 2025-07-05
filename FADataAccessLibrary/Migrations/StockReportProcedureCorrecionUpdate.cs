using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class StockReportProcedureCorrecionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateConfiguration UpdateConfiguration = new UpdateConfiguration();
            UpdateConfiguration.UpdateCorrectionForStockReport();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
