using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddingCascadeDeletingOnProductWhenCategoryDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryType",
                table: "tblProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryType",
                table: "tblProducts",
                column: "CategoryType",
                principalTable: "tblCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryType",
                table: "tblProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryType",
                table: "tblProducts",
                column: "CategoryType",
                principalTable: "tblCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
