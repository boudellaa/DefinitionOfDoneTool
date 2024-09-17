// <copyright file="20240911081151_InitialCreate.cs" company="Skyline Communications">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace DoneTool.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checks",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Item = table.Column<string>(type: "nvarchar(max)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TaskChecklist",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskID = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Guard = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskChecksID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskChecklist", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TaskChecks",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Step = table.Column<int>(type: "int", nullable: false),
                    TaskType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskChecks", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checks");

            migrationBuilder.DropTable(
                name: "TaskChecklist");

            migrationBuilder.DropTable(
                name: "TaskChecks");
        }
    }
}
