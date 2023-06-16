using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class ChangeTypeOfCategoryTypeAndStoreTypeInProductstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryType",
                table: "tblProducts");

            migrationBuilder.AlterColumn<int>(
                name: "StoreType",
                table: "tblProducts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryType",
                table: "tblProducts",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryType",
                table: "tblProducts",
                column: "CategoryType",
                principalTable: "tblCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryType",
                table: "tblProducts");

            migrationBuilder.AlterColumn<int>(
                name: "StoreType",
                table: "tblProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryType",
                table: "tblProducts",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tblProducts_tblCategories_CategoryType",
                table: "tblProducts",
                column: "CategoryType",
                principalTable: "tblCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
