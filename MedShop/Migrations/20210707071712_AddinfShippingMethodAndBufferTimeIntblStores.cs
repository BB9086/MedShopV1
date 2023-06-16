using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddinfShippingMethodAndBufferTimeIntblStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BufferTime",
                table: "tblStores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShippingMethod",
                table: "tblStores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BufferTime",
                table: "tblStores");

            migrationBuilder.DropColumn(
                name: "ShippingMethod",
                table: "tblStores");
        }
    }
}
