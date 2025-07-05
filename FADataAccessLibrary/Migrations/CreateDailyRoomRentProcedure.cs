using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class CreateDailyRoomRentProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateConfiguration UpdateConfiguration = new UpdateConfiguration();
            UpdateConfiguration.CreateDailyRoomRentProcedure();

            migrationBuilder.Sql("SET GLOBAL event_scheduler = ON;");

            migrationBuilder.Sql(@"
                CREATE EVENT IF NOT EXISTS CreateDailyRoomRentSchedule
                ON SCHEDULE EVERY 1 DAY
                STARTS CURDATE()
                DO
                CALL CreateDailyRoomRentProcedure();
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
