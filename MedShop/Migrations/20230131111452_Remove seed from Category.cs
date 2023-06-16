using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class RemoveseedfromCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tblCategories",
                keyColumn: "CategoryId",
                keyValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "tblCategories",
                columns: new[] { "CategoryId", "CategoryName", "StoreType" },
                values: new object[] { 1, "Laptops", 0 });
        }
    }
}
