using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class MakeContractractHoursOerDayToDecimal_AndRemoveLogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "LogEntities");

            migrationBuilder.DropTable(
                name: "LogTypes");

            migrationBuilder.AlterColumn<decimal>(
                name: "HoursPerDay",
                table: "Contracts",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HoursPerDay",
                table: "Contracts",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateTable(
                name: "LogEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy_FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title_GR = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    CreatedBy_FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title_GR = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    ActingUserFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActingUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy_FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedJSON = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogEntityId = table.Column<int>(type: "int", nullable: false),
                    LogTypeId = table.Column<int>(type: "int", nullable: false),
                    OriginalJSON = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
    }
}
