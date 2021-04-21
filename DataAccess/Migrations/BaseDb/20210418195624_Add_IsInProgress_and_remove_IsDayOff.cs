using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class Add_IsInProgress_and_remove_IsDayOff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RealWorkHours_StartOn_EndOn_EmployeeId",
                table: "RealWorkHours");

            migrationBuilder.DropColumn(
                name: "IsDayOff",
                table: "WorkHours");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndOn",
                table: "RealWorkHours",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsInProgress",
                table: "RealWorkHours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_RealWorkHours_StartOn_EndOn_EmployeeId",
                table: "RealWorkHours",
                columns: new[] { "StartOn", "EndOn", "EmployeeId" },
                unique: true,
                filter: "[EndOn] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RealWorkHours_StartOn_EndOn_EmployeeId",
                table: "RealWorkHours");

            migrationBuilder.DropColumn(
                name: "IsInProgress",
                table: "RealWorkHours");

            migrationBuilder.AddColumn<bool>(
                name: "IsDayOff",
                table: "WorkHours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndOn",
                table: "RealWorkHours",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealWorkHours_StartOn_EndOn_EmployeeId",
                table: "RealWorkHours",
                columns: new[] { "StartOn", "EndOn", "EmployeeId" },
                unique: true);
        }
    }
}
