using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class SetNullOnDeletingStoreForCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCategories_tblStores_StoreType",
                table: "tblCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCategories_tblStores_StoreType",
                table: "tblCategories",
                column: "StoreType",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblCategories_tblStores_StoreType",
                table: "tblCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_tblCategories_tblStores_StoreType",
                table: "tblCategories",
                column: "StoreType",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
