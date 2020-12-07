using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.SecurityDb
{
    public partial class Recreate_table_ApplicationUserTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    ApplicationTagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTags_ApplicationTags_ApplicationTagId",
                        column: x => x.ApplicationTagId,
                        principalTable: "ApplicationTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTags_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTags_ApplicationTagId",
                table: "ApplicationUserTags",
                column: "ApplicationTagId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTags_ApplicationUserId",
                table: "ApplicationUserTags",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserTags");
        }
    }
}
