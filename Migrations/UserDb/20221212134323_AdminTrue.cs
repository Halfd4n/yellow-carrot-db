using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YellowCarrotDb.Migrations.UserDb
{
    /// <inheritdoc />
    public partial class AdminTrue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "IsAdmin",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "IsAdmin",
                value: false);
        }
    }
}
