using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class SetCascadeForProductWhenStoreDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblStores_StoreType",
                table: "tblProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblStores_StoreType",
                table: "tblProducts",
                column: "StoreType",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblStores_StoreType",
                table: "tblProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblStores_StoreType",
                table: "tblProducts",
                column: "StoreType",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
