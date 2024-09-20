using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoneTool.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsDuplicateAddOriginalTaskChecklistID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDuplicate",
                table: "TaskChecklist");

            migrationBuilder.AddColumn<Guid>(
                name: "OriginalTaskChecklistID",
                table: "TaskChecklist",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalTaskChecklistID",
                table: "TaskChecklist");

            migrationBuilder.AddColumn<bool>(
                name: "IsDuplicate",
                table: "TaskChecklist",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
