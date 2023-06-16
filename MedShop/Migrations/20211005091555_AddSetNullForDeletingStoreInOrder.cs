using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddSetNullForDeletingStoreInOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOrders_tblStores_StoreId",
                table: "tblOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrders_tblStores_StoreId",
                table: "tblOrders",
                column: "StoreId",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOrders_tblStores_StoreId",
                table: "tblOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrders_tblStores_StoreId",
                table: "tblOrders",
                column: "StoreId",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
