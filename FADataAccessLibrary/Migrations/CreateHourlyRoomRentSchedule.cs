using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class CreateHourlyRoomRentSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET GLOBAL event_scheduler = ON;");

            migrationBuilder.Sql("DROP EVENT IF EXISTS CreateDailyRoomRentSchedule;");

            migrationBuilder.Sql(@"
                CREATE EVENT IF NOT EXISTS CreateHourlyRoomRentSchedule
                ON SCHEDULE EVERY 1 HOUR
                STARTS NOW()
                DO
                CALL CreateDailyRoomRentProcedure();
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
