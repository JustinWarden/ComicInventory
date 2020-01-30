using Microsoft.EntityFrameworkCore.Migrations;

namespace ComicBookInventory.Migrations
{
    public partial class dbcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "98e9929d-b254-4969-8557-a79f71685fe0", "AQAAAAEAACcQAAAAEFY5LegWLGfB23iqS0ov9a03DSyd1U1J31PB9g5h0XbT0PSUpKf1Zr7JeQV4S1lLSQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "00000000-ffff-ffff-ffff-ffffffffffff",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "4da3311d-1b26-4de4-82f4-a248f8252cf5", "AQAAAAEAACcQAAAAEBmjF8IU9md6RIoGiRLAZwP8UCSbuFbpzqsmAwjhR78dIdB1feXmYSvBDvafGZMaaQ==" });
        }
    }
}
