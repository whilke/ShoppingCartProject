using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingCart.Service.Data.Migrations
{
    public partial class ItemData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Name", "Price", "Url" },
                values: new object[] { 1, "Google", 1.0, "https://www.google.com" });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Name", "Price", "Url" },
                values: new object[] { 2, "Microsoft", 2.0, "https://www.microsoft.com" });

            migrationBuilder.InsertData(
                table: "Items",
                columns: new[] { "Id", "Name", "Price", "Url" },
                values: new object[] { 3, "Yahoo", 3.0, "https://www.yahoo.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Items",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
