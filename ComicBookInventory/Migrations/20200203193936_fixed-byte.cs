using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicBookInventory.Migrations
{
    public partial class fixedbyte : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Comics",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ComicImage",
                table: "Comics",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(55)",
                oldMaxLength: 55,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1c5a4ce3-4621-4432-a533-5d0a7bde2c01", "AQAAAAEAACcQAAAAEC76mxtcmb+L3MgCM1XYJgilYOWvINl6hD76JlxIQ2fRbD/AcVsdzz0zLEF5Fj0RVw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Comics",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 55,
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "ComicImage",
                table: "Comics",
                type: "varbinary(55)",
                maxLength: 55,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "98e9929d-b254-4969-8557-a79f71685fe0", "AQAAAAEAACcQAAAAEFY5LegWLGfB23iqS0ov9a03DSyd1U1J31PB9g5h0XbT0PSUpKf1Zr7JeQV4S1lLSQ==" });
        }
    }
}
