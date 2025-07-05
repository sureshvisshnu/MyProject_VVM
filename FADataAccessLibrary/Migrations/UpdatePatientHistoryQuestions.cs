using System;
using FADataAccessLibrary.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdatePatientHistoryQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            UpdateConfiguration UpdateConfiguration = new UpdateConfiguration();
            UpdateConfiguration.UpdatePatientHistoryQuestionsWithAllergies();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
