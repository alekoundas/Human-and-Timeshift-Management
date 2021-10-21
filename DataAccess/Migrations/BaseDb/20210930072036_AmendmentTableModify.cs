using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class AmendmentTableModify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Amendments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Amendments_EmployeeId",
                table: "Amendments",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Amendments_Employees_EmployeeId",
                table: "Amendments",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Amendments_Employees_EmployeeId",
                table: "Amendments");

            migrationBuilder.DropIndex(
                name: "IX_Amendments_EmployeeId",
                table: "Amendments");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Amendments");
        }
    }
}
