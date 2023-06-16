using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddingForeignkey_Stores_Categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_tblCategories_StoreType",
                table: "tblCategories",
                column: "StoreType");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCategories_tblStores_StoreType",
                table: "tblCategories",
                column: "StoreType",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCategories_tblStores_StoreType",
                table: "tblCategories");

            migrationBuilder.DropIndex(
                name: "IX_tblCategories_StoreType",
                table: "tblCategories");
        }
    }
}
