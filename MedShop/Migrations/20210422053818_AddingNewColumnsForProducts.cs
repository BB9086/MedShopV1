using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddingNewColumnsForProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryTypeCategoryId",
                table: "tblProducts");

            migrationBuilder.DropIndex(
                name: "IX_tblProducts_CategoryTypeCategoryId",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "CategoryTypeCategoryId",
                table: "tblProducts");

            migrationBuilder.AddColumn<int>(
                name: "CategoryType",
                table: "tblProducts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPrice",
                table: "tblProducts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "tblProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Producer",
                table: "tblProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductDescription",
                table: "tblProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductKey",
                table: "tblProducts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Quantity",
                table: "tblProducts",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalQuantity_Kg",
                table: "tblProducts",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_CategoryType",
                table: "tblProducts",
                column: "CategoryType");

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

            migrationBuilder.DropIndex(
                name: "IX_tblProducts_CategoryType",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "CategoryType",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "CurrentPrice",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "Producer",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "ProductDescription",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "ProductKey",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "tblProducts");

            migrationBuilder.DropColumn(
                name: "TotalQuantity_Kg",
                table: "tblProducts");

            migrationBuilder.AddColumn<int>(
                name: "CategoryTypeCategoryId",
                table: "tblProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblProducts_CategoryTypeCategoryId",
                table: "tblProducts",
                column: "CategoryTypeCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryTypeCategoryId",
                table: "tblProducts",
                column: "CategoryTypeCategoryId",
                principalTable: "tblCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
