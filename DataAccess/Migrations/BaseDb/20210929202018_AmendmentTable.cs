using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class AmendmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AmendmentId",
                table: "RealWorkHours",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Amendments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewStartOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewEndOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RealWorkHourId = table.Column<int>(type: "int", nullable: true),
                    TimeShiftId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy_FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amendments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Amendments_RealWorkHours_RealWorkHourId",
                        column: x => x.RealWorkHourId,
                        principalTable: "RealWorkHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Amendments_TimeShifts_TimeShiftId",
                        column: x => x.TimeShiftId,
                        principalTable: "TimeShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RealWorkHours_AmendmentId",
                table: "RealWorkHours",
                column: "AmendmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_Id",
                table: "Amendments",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_RealWorkHourId",
                table: "Amendments",
                column: "RealWorkHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_TimeShiftId",
                table: "Amendments",
                column: "TimeShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_RealWorkHours_Amendments_AmendmentId",
                table: "RealWorkHours",
                column: "AmendmentId",
                principalTable: "Amendments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RealWorkHours_Amendments_AmendmentId",
                table: "RealWorkHours");

            migrationBuilder.DropTable(
                name: "Amendments");

            migrationBuilder.DropIndex(
                name: "IX_RealWorkHours_AmendmentId",
                table: "RealWorkHours");

            migrationBuilder.DropColumn(
                name: "AmendmentId",
                table: "RealWorkHours");
        }
    }
}
