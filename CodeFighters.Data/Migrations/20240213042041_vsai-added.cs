using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeFighters.Data.Migrations
{
    /// <inheritdoc />
    public partial class vsaiadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVsAI",
                table: "Games",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVsAI",
                table: "Games");
        }
    }
}
