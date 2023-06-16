using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddStoretbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreType",
                table: "tblCategories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblStores",
                columns: table => new
                {
                    StoreId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(nullable: true),
                    StoreDescription = table.Column<string>(nullable: true),
                    DeliveryOpen = table.Column<string>(nullable: true),
                    DeliveryClose = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblStores", x => x.StoreId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblStores");

            migrationBuilder.DropColumn(
                name: "StoreType",
                table: "tblCategories");
        }
    }
}
