using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopManagement.Infrastructure.EFCore.Migrations
{
    public partial class SlideTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Slides",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Picture = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PictureAlt = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    PictureTitle = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    Heading = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Text = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: true),
                    BtnText = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BtnColor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slides", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slides");
        }
    }
}
