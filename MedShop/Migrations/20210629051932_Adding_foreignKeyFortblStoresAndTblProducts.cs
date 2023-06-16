using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class Adding_foreignKeyFortblStoresAndTblProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreType",
                table: "tblProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_StoreType",
                table: "tblProducts",
                column: "StoreType");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblStores_StoreType",
                table: "tblProducts",
                column: "StoreType",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblStores_StoreType",
                table: "tblProducts");

            migrationBuilder.DropIndex(
                name: "IX_tblProducts_StoreType",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "StoreType",
                table: "tblProducts");
        }
    }
}
