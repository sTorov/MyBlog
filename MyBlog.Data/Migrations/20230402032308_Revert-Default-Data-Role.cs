using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class RevertDefaultDataRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "3794c9b0-4a17-4206-b12c-cee7f23c7d01", "User", "USER" },
                    { 2, "3e3c21a4-8f19-4359-b7ef-90553de1fd9b", "Moderator", "MODERATOR" },
                    { 3, "708222ab-15dd-4b0d-9e07-9ae45d68300f", "Administrator", "ADMINISTRATOR" }
                });
        }
    }
}
