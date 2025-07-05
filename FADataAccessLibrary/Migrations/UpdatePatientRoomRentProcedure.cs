using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdatePatientRoomRentProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateConfiguration UpdateConfiguration = new UpdateConfiguration();
            UpdateConfiguration.DailyRoomRentProcedure();

            migrationBuilder.Sql("SET GLOBAL event_scheduler = ON;");

            migrationBuilder.Sql("DROP EVENT IF EXISTS CreateDailyRoomRentSchedule;");
            migrationBuilder.Sql("DROP EVENT IF EXISTS CreateHourlyRoomRentSchedule;");

            migrationBuilder.Sql(@"
                CREATE EVENT IF NOT EXISTS HourlyPatientRoomRentSchedule
                ON SCHEDULE EVERY 1 HOUR
                STARTS NOW()
                DO
                CALL DailyRoomRentProcedure();
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
