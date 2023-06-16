using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MedShop.Migrations
{
    public partial class AddingNewColumnsInStoreTab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "tblStores",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StoreGuid",
                table: "tblStores",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "tblStores");

            migrationBuilder.DropColumn(
                name: "StoreGuid",
                table: "tblStores");
        }
    }
}
