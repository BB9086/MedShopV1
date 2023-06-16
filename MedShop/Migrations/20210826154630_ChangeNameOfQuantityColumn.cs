using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class ChangeNameOfQuantityColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "tblProducts");

            migrationBuilder.AddColumn<string>(
                name: "QuantityOfProduct",
                table: "tblProducts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityOfProduct",
                table: "tblProducts");

            migrationBuilder.AddColumn<string>(
                name: "Quantity",
                table: "tblProducts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
