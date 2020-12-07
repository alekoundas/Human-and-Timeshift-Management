using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.SecurityDb
{
    public partial class Drop_table_ApplicationUserTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserTag");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserTag",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationTagId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTag_ApplicationTags_ApplicationTagId",
                        column: x => x.ApplicationTagId,
                        principalTable: "ApplicationTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTag_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTag_ApplicationTagId",
                table: "ApplicationUserTag",
                column: "ApplicationTagId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTag_ApplicationUserId",
                table: "ApplicationUserTag",
                column: "ApplicationUserId");
        }
    }
}
