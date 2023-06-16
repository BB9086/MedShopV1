using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class setNullForDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems");

            migrationBuilder.AddForeignKey(
                name: "FK_tblOrderItems_tblProducts_ProductId",
                table: "tblOrderItems",
                column: "ProductId",
                principalTable: "tblProducts",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
