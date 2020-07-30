using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class AddCommentsAndIsDayOffColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "WorkHours",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDayOff",
                table: "WorkHours",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "WorkHours");

            migrationBuilder.DropColumn(
                name: "IsDayOff",
                table: "WorkHours");
        }
    }
}
