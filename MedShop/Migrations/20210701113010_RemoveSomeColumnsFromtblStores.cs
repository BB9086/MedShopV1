using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class RemoveSomeColumnsFromtblStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryClose",
                table: "tblStores");

            migrationBuilder.DropColumn(
                name: "DeliveryOpen",
                table: "tblStores");

            migrationBuilder.AddColumn<string>(
                name: "StoreAddress",
                table: "tblStores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoreAddress",
                table: "tblStores");

            migrationBuilder.AddColumn<string>(
                name: "DeliveryClose",
                table: "tblStores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryOpen",
                table: "tblStores",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
