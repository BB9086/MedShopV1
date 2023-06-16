using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddingCascadeDeleteOnShippingMethodWhenStoreDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblShippingMethod_tblStores_StoreType",
                table: "tblShippingMethod");

            migrationBuilder.AddForeignKey(
                name: "FK_tblShippingMethod_tblStores_StoreType",
                table: "tblShippingMethod",
                column: "StoreType",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblShippingMethod_tblStores_StoreType",
                table: "tblShippingMethod");

            migrationBuilder.AddForeignKey(
                name: "FK_tblShippingMethod_tblStores_StoreType",
                table: "tblShippingMethod",
                column: "StoreType",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
