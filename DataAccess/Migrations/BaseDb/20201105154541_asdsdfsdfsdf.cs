using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class asdsdfsdfsdf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkPlaceHourRestrictions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkHours");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RealWorkHours");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "EmployeeWorkPlaces");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkPlaceHourRestrictions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkHours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RealWorkHours",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Leaves",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "EmployeeWorkPlaces",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
