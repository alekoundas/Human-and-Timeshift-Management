using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class LogTablesAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title_GR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy_FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title_GR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy_FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EditedJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActingUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActingUserFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogEntityId = table.Column<int>(type: "int", nullable: false),
                    LogTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy_FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logs_LogEntities_LogEntityId",
                        column: x => x.LogEntityId,
                        principalTable: "LogEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Logs_LogTypes_LogTypeId",
                        column: x => x.LogTypeId,
                        principalTable: "LogTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogEntities_Title",
                table: "LogEntities",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Id",
                table: "Logs",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Logs_LogEntityId",
                table: "Logs",
                column: "LogEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_LogTypeId",
                table: "Logs",
                column: "LogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LogTypes_Title",
                table: "LogTypes",
                column: "Title",
                unique: true,
                filter: "[Title] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "LogEntities");

            migrationBuilder.DropTable(
                name: "LogTypes");
        }
    }
}
