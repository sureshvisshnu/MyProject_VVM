using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateDailyRoomRentProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateConfiguration UpdateConfiguration = new UpdateConfiguration();
            UpdateConfiguration.UpdateDailyRoomRentProcedure();

            migrationBuilder.Sql("SET GLOBAL event_scheduler = ON;");

            migrationBuilder.Sql(@"
                CREATE EVENT IF NOT EXISTS CreateHourlyRoomRentSchedule
                ON SCHEDULE EVERY 1 HOUR
                STARTS NOW()
                DO
                CALL UpdateDailyRoomRentProcedure();
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
