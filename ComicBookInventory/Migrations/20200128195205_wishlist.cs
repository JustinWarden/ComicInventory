using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicBookInventory.Migrations
{
    public partial class wishlist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IssueNumber",
                table: "Comics",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    IssueNumber = table.Column<int>(nullable: false),
                    Publisher = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    VolumeNumber = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    ComicImage = table.Column<byte[]>(maxLength: 55, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishlist_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4da3311d-1b26-4de4-82f4-a248f8252cf5", "AQAAAAEAACcQAAAAEBmjF8IU9md6RIoGiRLAZwP8UCSbuFbpzqsmAwjhR78dIdB1feXmYSvBDvafGZMaaQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_UserId",
                table: "Wishlist",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wishlist");

            migrationBuilder.DropColumn(
                name: "IssueNumber",
                table: "Comics");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c306effb-35a8-4117-9c87-acf2952d7b00", "AQAAAAEAACcQAAAAECidOzv8L5wTyCAkaspJRAfthTHdSk1MNOCHPvu/QlxfEnOJygIeiaYzfO9hA5Cm6Q==" });
        }
    }
}
