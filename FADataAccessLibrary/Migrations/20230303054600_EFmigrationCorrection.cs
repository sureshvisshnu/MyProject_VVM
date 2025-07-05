using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class EFmigrationCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO `vv-matrix`.__efmigrationshistory (MigrationId, ProductVersion) SELECT '20231114082246_AccountIndex', '7.0.2' WHERE EXISTS (SELECT MigrationId FROM `vv-matrix`.__efmigrationshistory WHERE MigrationId = 'AccountIndex');");
            migrationBuilder.Sql("INSERT INTO `vv-matrix`.__efmigrationshistory (MigrationId, ProductVersion) SELECT '20241125061904_AddRoomRent', '7.0.2' WHERE EXISTS (SELECT MigrationId FROM `vv-matrix`.__efmigrationshistory WHERE MigrationId = 'AddRoomRent');");
            migrationBuilder.Sql("INSERT INTO `vv-matrix`.__efmigrationshistory (MigrationId, ProductVersion) SELECT '20240612113302_AllergieUpdate', '7.0.2' WHERE EXISTS (SELECT MigrationId FROM `vv-matrix`.__efmigrationshistory WHERE MigrationId = 'AllergieUpdate');");

            migrationBuilder.Sql("DELETE target FROM `vv-matrix`.__efmigrationshistory AS target JOIN (SELECT MigrationId FROM `vv-matrix`.__efmigrationshistory WHERE MigrationId = 'AccountIndex') AS source ON target.MigrationId = source.MigrationId;");
            migrationBuilder.Sql("DELETE target FROM `vv-matrix`.__efmigrationshistory AS target JOIN (SELECT MigrationId FROM `vv-matrix`.__efmigrationshistory WHERE MigrationId = 'AddRoomRent') AS source ON target.MigrationId = source.MigrationId;");
            migrationBuilder.Sql("DELETE target FROM `vv-matrix`.__efmigrationshistory AS target JOIN (SELECT MigrationId FROM `vv-matrix`.__efmigrationshistory WHERE MigrationId = 'AllergieUpdate') AS source ON target.MigrationId = source.MigrationId;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
