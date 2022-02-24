using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopManagement.Infrastructure.EFCore.Migrations
{
    public partial class addMaterialInProductTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "Materials",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ProductId",
                table: "Materials",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Products_ProductId",
                table: "Materials",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Products_ProductId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ProductId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Materials");
        }
    }
}
