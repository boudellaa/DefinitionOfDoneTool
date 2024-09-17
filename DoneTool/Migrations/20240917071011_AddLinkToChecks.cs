using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoneTool.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkToChecks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Checks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Link",
                table: "Checks");
        }
    }
}
