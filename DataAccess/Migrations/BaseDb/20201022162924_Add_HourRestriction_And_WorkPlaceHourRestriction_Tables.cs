using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class Add_HourRestriction_And_WorkPlaceHourRestriction_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkPlaceHourRestrictions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    WorkPlaceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlaceHourRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPlaceHourRestrictions_WorkPlaces_WorkPlaceId",
                        column: x => x.WorkPlaceId,
                        principalTable: "WorkPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HourRestrictions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    MaxTime = table.Column<TimeSpan>(nullable: false),
                    WorkPlaceHourRestrictionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HourRestrictions_WorkPlaceHourRestrictions_WorkPlaceHourRestrictionId",
                        column: x => x.WorkPlaceHourRestrictionId,
                        principalTable: "WorkPlaceHourRestrictions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Id",
                table: "Companies",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HourRestrictions_Id",
                table: "HourRestrictions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HourRestrictions_WorkPlaceHourRestrictionId",
                table: "HourRestrictions",
                column: "WorkPlaceHourRestrictionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaceHourRestrictions_Id",
                table: "WorkPlaceHourRestrictions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaceHourRestrictions_WorkPlaceId",
                table: "WorkPlaceHourRestrictions",
                column: "WorkPlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HourRestrictions");

            migrationBuilder.DropTable(
                name: "WorkPlaceHourRestrictions");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Id",
                table: "Companies");
        }
    }
}
