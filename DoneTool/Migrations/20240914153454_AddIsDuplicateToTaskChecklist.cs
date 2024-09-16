using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoneTool.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDuplicateToTaskChecklist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDuplicate",
                table: "TaskChecklist",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDuplicate",
                table: "TaskChecklist");
        }
    }
}
