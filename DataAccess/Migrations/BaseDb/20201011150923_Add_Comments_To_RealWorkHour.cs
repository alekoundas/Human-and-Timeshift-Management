using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class Add_Comments_To_RealWorkHour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "RealWorkHours",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comments",
                table: "RealWorkHours");
        }
    }
}
