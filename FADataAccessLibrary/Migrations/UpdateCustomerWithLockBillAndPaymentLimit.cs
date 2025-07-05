using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FADataAccessLibrary.Migrations
{
    public partial class UpdateCustomerWithLockBillAndPaymentLimit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PaymentLimit",
                table: "customers",
                type: "double",
                nullable: true);
            
            migrationBuilder.AddColumn<bool>(
                name: "LockBill",
                table: "customers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentLimit",
                table: "customers");
            
            migrationBuilder.DropColumn(
                name: "LockBill",
                table: "customers");
        }
    }
}
