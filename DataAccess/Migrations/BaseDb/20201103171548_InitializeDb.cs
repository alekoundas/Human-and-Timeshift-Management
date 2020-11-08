﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations.BaseDb
{
    public partial class InitializeDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Afm = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PayPerHour = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ΙdentifyingΝame = table.Column<string>(nullable: false),
                    AFM = table.Column<string>(nullable: false),
                    Profession = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    DOY = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Afm = table.Column<string>(nullable: true),
                    SocialSecurityNumber = table.Column<string>(nullable: true),
                    ErpCode = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    SpecializationId = table.Column<int>(nullable: true),
                    CompanyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Specializations_SpecializationId",
                        column: x => x.SpecializationId,
                        principalTable: "Specializations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPlaces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkPlaces_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contacts_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Leaves",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    StartOn = table.Column<DateTime>(nullable: false),
                    EndOn = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<int>(nullable: false),
                    LeaveTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Leaves_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Leaves_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeWorkPlaces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    WorkPlaceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeWorkPlaces", x => new { x.Id, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeWorkPlaces_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeWorkPlaces_WorkPlaces_WorkPlaceId",
                        column: x => x.WorkPlaceId,
                        principalTable: "WorkPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeShifts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    WorkPlaceId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeShifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeShifts_WorkPlaces_WorkPlaceId",
                        column: x => x.WorkPlaceId,
                        principalTable: "WorkPlaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "RealWorkHours",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    StartOn = table.Column<DateTime>(nullable: false),
                    EndOn = table.Column<DateTime>(nullable: false),
                    TimeShiftId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RealWorkHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RealWorkHours_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RealWorkHours_TimeShifts_TimeShiftId",
                        column: x => x.TimeShiftId,
                        principalTable: "TimeShifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkHours",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    StartOn = table.Column<DateTime>(nullable: false),
                    EndOn = table.Column<DateTime>(nullable: false),
                    IsDayOff = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    TimeShiftId = table.Column<int>(nullable: false),
                    EmployeeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkHours_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkHours_TimeShifts_TimeShiftId",
                        column: x => x.TimeShiftId,
                        principalTable: "TimeShifts",
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
                    MaxTicks = table.Column<double>(nullable: false),
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
                name: "IX_Contacts_CustomerId",
                table: "Contacts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_EmployeeId",
                table: "Contacts",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Id",
                table: "Contacts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CompanyId",
                table: "Customers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Id",
                table: "Customers",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CompanyId",
                table: "Employees",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Id",
                table: "Employees",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_SpecializationId",
                table: "Employees",
                column: "SpecializationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkPlaces_EmployeeId",
                table: "EmployeeWorkPlaces",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkPlaces_Id",
                table: "EmployeeWorkPlaces",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeWorkPlaces_WorkPlaceId",
                table: "EmployeeWorkPlaces",
                column: "WorkPlaceId");

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
                name: "IX_Leaves_EmployeeId",
                table: "Leaves",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_EndOn",
                table: "Leaves",
                column: "EndOn");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_LeaveTypeId",
                table: "Leaves",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Leaves_StartOn",
                table: "Leaves",
                column: "StartOn");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveTypes_Id",
                table: "LeaveTypes",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RealWorkHours_EmployeeId",
                table: "RealWorkHours",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RealWorkHours_EndOn",
                table: "RealWorkHours",
                column: "EndOn");

            migrationBuilder.CreateIndex(
                name: "IX_RealWorkHours_StartOn",
                table: "RealWorkHours",
                column: "StartOn");

            migrationBuilder.CreateIndex(
                name: "IX_RealWorkHours_TimeShiftId",
                table: "RealWorkHours",
                column: "TimeShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_Id",
                table: "Specializations",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeShifts_Id",
                table: "TimeShifts",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TimeShifts_WorkPlaceId",
                table: "TimeShifts",
                column: "WorkPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHours_EmployeeId",
                table: "WorkHours",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHours_EndOn",
                table: "WorkHours",
                column: "EndOn");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHours_StartOn",
                table: "WorkHours",
                column: "StartOn");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHours_TimeShiftId",
                table: "WorkHours",
                column: "TimeShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaceHourRestrictions_Id",
                table: "WorkPlaceHourRestrictions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaceHourRestrictions_WorkPlaceId",
                table: "WorkPlaceHourRestrictions",
                column: "WorkPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaces_CustomerId",
                table: "WorkPlaces",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkPlaces_Id",
                table: "WorkPlaces",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "EmployeeWorkPlaces");

            migrationBuilder.DropTable(
                name: "HourRestrictions");

            migrationBuilder.DropTable(
                name: "Leaves");

            migrationBuilder.DropTable(
                name: "RealWorkHours");

            migrationBuilder.DropTable(
                name: "WorkHours");

            migrationBuilder.DropTable(
                name: "WorkPlaceHourRestrictions");

            migrationBuilder.DropTable(
                name: "LeaveTypes");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "TimeShifts");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropTable(
                name: "WorkPlaces");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}