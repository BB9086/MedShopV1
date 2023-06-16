using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddImagePicture_tblStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageSource",
                table: "tblStores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageSource",
                table: "tblStores");
        }
    }
}
