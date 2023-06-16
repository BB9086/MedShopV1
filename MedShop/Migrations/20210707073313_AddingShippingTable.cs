using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddingShippingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BufferTime",
                table: "tblStores");

            migrationBuilder.DropColumn(
                name: "ShippingMethod",
                table: "tblStores");

            migrationBuilder.CreateTable(
                name: "tblShippingMethod",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShippingMethod = table.Column<string>(nullable: true),
                    BufferTime = table.Column<int>(nullable: false),
                    StoreType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblShippingMethod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblShippingMethod_tblStores_StoreType",
                        column: x => x.StoreType,
                        principalTable: "tblStores",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblShippingMethod_StoreType",
                table: "tblShippingMethod",
                column: "StoreType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblShippingMethod");

            migrationBuilder.AddColumn<int>(
                name: "BufferTime",
                table: "tblStores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShippingMethod",
                table: "tblStores",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
