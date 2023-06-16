using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class CascadeDeletingOpeningHoursWhenDeleteStore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOpeningHours_tblStores_StoreId",
                table: "tblOpeningHours");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOpeningHours_tblStores_StoreId",
                table: "tblOpeningHours",
                column: "StoreId",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOpeningHours_tblStores_StoreId",
                table: "tblOpeningHours");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOpeningHours_tblStores_StoreId",
                table: "tblOpeningHours",
                column: "StoreId",
                principalTable: "tblStores",
                principalColumn: "StoreId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
