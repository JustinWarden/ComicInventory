using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicBookInventory.Migrations
{
    public partial class piechart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a24d9f4a-04ba-4802-a4cd-ce5cb8f21092", "AQAAAAEAACcQAAAAEHnyNQCrEHv5M7BkR8dfitRNguyaWARzi1N3lFyWziMKdceL7eJjMiJ2vBKPmNBv1A==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "1c5a4ce3-4621-4432-a533-5d0a7bde2c01", "AQAAAAEAACcQAAAAEC76mxtcmb+L3MgCM1XYJgilYOWvINl6hD76JlxIQ2fRbD/AcVsdzz0zLEF5Fj0RVw==" });
        }
    }
}
