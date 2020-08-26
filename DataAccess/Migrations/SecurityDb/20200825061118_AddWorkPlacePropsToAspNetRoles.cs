using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.SecurityDb
{
    public partial class AddWorkPlacePropsToAspNetRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WorkPlaceId",
                table: "AspNetRoles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkPlaceName",
                table: "AspNetRoles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkPlaceId",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "WorkPlaceName",
                table: "AspNetRoles");
        }
    }
}
