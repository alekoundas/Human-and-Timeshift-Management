using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class Add_Index_To_Every_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaces_Id",
                table: "WorkPlaces",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkHours_EndOn",
                table: "WorkHours",
                column: "EndOn");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHours_StartOn",
                table: "WorkHours",
                column: "StartOn");

            migrationBuilder.CreateIndex(
                name: "IX_TimeShifts_Id",
                table: "TimeShifts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Id",
                table: "Specializations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealWorkHours_EndOn",
                table: "RealWorkHours",
                column: "EndOn");

            migrationBuilder.CreateIndex(
                name: "IX_RealWorkHours_StartOn",
                table: "RealWorkHours",
                column: "StartOn");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_Id",
                table: "LeaveTypes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_EndOn",
                table: "Leaves",
                column: "EndOn");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_StartOn",
                table: "Leaves",
                column: "StartOn");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkPlaces_Id",
                table: "EmployeeWorkPlaces",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Id",
                table: "Employees",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Id",
                table: "Customers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Id",
                table: "Contacts",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkPlaces_Id",
                table: "WorkPlaces");

            migrationBuilder.DropIndex(
                name: "IX_WorkHours_EndOn",
                table: "WorkHours");

            migrationBuilder.DropIndex(
                name: "IX_WorkHours_StartOn",
                table: "WorkHours");

            migrationBuilder.DropIndex(
                name: "IX_TimeShifts_Id",
                table: "TimeShifts");

            migrationBuilder.DropIndex(
                name: "IX_Specializations_Id",
                table: "Specializations");

            migrationBuilder.DropIndex(
                name: "IX_RealWorkHours_EndOn",
                table: "RealWorkHours");

            migrationBuilder.DropIndex(
                name: "IX_RealWorkHours_StartOn",
                table: "RealWorkHours");

            migrationBuilder.DropIndex(
                name: "IX_LeaveTypes_Id",
                table: "LeaveTypes");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_EndOn",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_Leaves_StartOn",
                table: "Leaves");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeWorkPlaces_Id",
                table: "EmployeeWorkPlaces");

            migrationBuilder.DropIndex(
                name: "IX_Employees_Id",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Customers_Id",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_Id",
                table: "Contacts");
        }
    }
}
