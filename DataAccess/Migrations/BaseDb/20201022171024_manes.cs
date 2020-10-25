using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class manes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxTime",
                table: "HourRestrictions");

            migrationBuilder.AddColumn<double>(
                name: "MaxTicks",
                table: "HourRestrictions",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxTicks",
                table: "HourRestrictions");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "MaxTime",
                table: "HourRestrictions",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
