﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations.BaseDb
{
    [DbContext(typeof(BaseDbContext))]
    [Migration("20210418195624_Add_IsInProgress_and_remove_IsDayOff")]
    partial class Add_IsInProgress_and_remove_IsDayOff
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataAccess.Models.Audit.AuditAutoHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Changed")
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("Kind")
                        .HasColumnType("int");

                    b.Property<string>("ModifiedBy_FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ModifiedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("RowId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.HasKey("Id");

                    b.ToTable("AuditAutoHistory");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("VatNumber")
                        .IsUnique();

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ContractMembershipId")
                        .HasColumnType("int");

                    b.Property<int>("ContractTypeId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DayOfDaysPerWeek")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("GrossSalaryPerHour")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("HoursPerDay")
                        .HasColumnType("int");

                    b.Property<int>("HoursPerWeek")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<decimal>("NetSalaryPerHour")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("WorkingDaysPerWeek")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContractMembershipId");

                    b.HasIndex("ContractTypeId");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.ContractMembership", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ContractMemberships");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.ContractType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("ContractTypes");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("DOY")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdentifyingName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Profession")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("VatNumber")
                        .IsUnique();

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ContractEndOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ContractId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ContractStartOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("ErpCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("HireDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SocialSecurityNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SpecializationId")
                        .HasColumnType("int");

                    b.Property<string>("VatNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ContractId");

                    b.HasIndex("SpecializationId");

                    b.HasIndex("VatNumber")
                        .IsUnique();

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.EmployeeWorkPlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("WorkPlaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("WorkPlaceId", "EmployeeId")
                        .IsUnique();

                    b.ToTable("EmployeeWorkPlaces");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.HourRestriction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<double>("MaxTicks")
                        .HasColumnType("float");

                    b.Property<int>("WorkPlaceHourRestrictionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("WorkPlaceHourRestrictionId");

                    b.ToTable("HourRestrictions");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Leave", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ApprovedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("LeaveTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("LeaveTypeId");

                    b.HasIndex("StartOn", "EndOn", "EmployeeId")
                        .IsUnique();

                    b.ToTable("Leaves");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.LeaveType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("LeaveTypes");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.RealWorkHour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsInProgress")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("TimeShiftId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("TimeShiftId");

                    b.HasIndex("StartOn", "EndOn", "EmployeeId")
                        .IsUnique()
                        .HasFilter("[EndOn] IS NOT NULL");

                    b.ToTable("RealWorkHours");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Specialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("PayPerHour")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Specializations");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.TimeShift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkPlaceId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkPlaceId", "Month", "Year")
                        .IsUnique();

                    b.ToTable("TimeShifts");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.WorkHour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("TimeShiftId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("TimeShiftId");

                    b.HasIndex("StartOn", "EndOn", "EmployeeId")
                        .IsUnique();

                    b.ToTable("WorkHours");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.WorkPlace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("Title", "CustomerId")
                        .IsUnique()
                        .HasFilter("[CustomerId] IS NOT NULL");

                    b.ToTable("WorkPlaces");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.WorkPlaceHourRestriction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy_FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<int>("WorkPlaceId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkPlaceId", "Month", "Year")
                        .IsUnique();

                    b.ToTable("WorkPlaceHourRestrictions");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Contact", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.Customer", "Customer")
                        .WithMany("Contacts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DataAccess.Models.Entity.Employee", "Employee")
                        .WithMany("Contacts")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Customer");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Contract", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.ContractMembership", "ContractMembership")
                        .WithMany("Contracts")
                        .HasForeignKey("ContractMembershipId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.Entity.ContractType", "ContractType")
                        .WithMany("Contracts")
                        .HasForeignKey("ContractTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ContractMembership");

                    b.Navigation("ContractType");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Customer", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.Company", "Company")
                        .WithMany("Customers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Company");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Employee", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.Company", "Company")
                        .WithMany("Employees")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DataAccess.Models.Entity.Contract", "Contract")
                        .WithMany("Employees")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DataAccess.Models.Entity.Specialization", "Specialization")
                        .WithMany("Employees")
                        .HasForeignKey("SpecializationId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Company");

                    b.Navigation("Contract");

                    b.Navigation("Specialization");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.EmployeeWorkPlace", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.Employee", "Employee")
                        .WithMany("EmployeeWorkPlaces")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.Entity.WorkPlace", "WorkPlace")
                        .WithMany("EmployeeWorkPlaces")
                        .HasForeignKey("WorkPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("WorkPlace");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.HourRestriction", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.WorkPlaceHourRestriction", "WorkPlaceHourRestriction")
                        .WithMany("HourRestrictions")
                        .HasForeignKey("WorkPlaceHourRestrictionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkPlaceHourRestriction");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Leave", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.Employee", "Employee")
                        .WithMany("Leaves")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.Entity.LeaveType", "LeaveType")
                        .WithMany("Leaves")
                        .HasForeignKey("LeaveTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("LeaveType");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.RealWorkHour", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.Employee", "Employee")
                        .WithMany("RealWorkHours")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.Entity.TimeShift", "TimeShift")
                        .WithMany("RealWorkHours")
                        .HasForeignKey("TimeShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("TimeShift");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.TimeShift", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.WorkPlace", "WorkPlace")
                        .WithMany("TimeShifts")
                        .HasForeignKey("WorkPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkPlace");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.WorkHour", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.Employee", "Employee")
                        .WithMany("WorkHours")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Models.Entity.TimeShift", "TimeShift")
                        .WithMany("WorkHours")
                        .HasForeignKey("TimeShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("TimeShift");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.WorkPlace", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.Customer", "Customer")
                        .WithMany("WorkPlaces")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.WorkPlaceHourRestriction", b =>
                {
                    b.HasOne("DataAccess.Models.Entity.WorkPlace", "WorkPlace")
                        .WithMany("WorkPlaceHourRestrictions")
                        .HasForeignKey("WorkPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WorkPlace");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Company", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Contract", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.ContractMembership", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.ContractType", b =>
                {
                    b.Navigation("Contracts");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Customer", b =>
                {
                    b.Navigation("Contacts");

                    b.Navigation("WorkPlaces");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Employee", b =>
                {
                    b.Navigation("Contacts");

                    b.Navigation("EmployeeWorkPlaces");

                    b.Navigation("Leaves");

                    b.Navigation("RealWorkHours");

                    b.Navigation("WorkHours");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.LeaveType", b =>
                {
                    b.Navigation("Leaves");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.Specialization", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.TimeShift", b =>
                {
                    b.Navigation("RealWorkHours");

                    b.Navigation("WorkHours");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.WorkPlace", b =>
                {
                    b.Navigation("EmployeeWorkPlaces");

                    b.Navigation("TimeShifts");

                    b.Navigation("WorkPlaceHourRestrictions");
                });

            modelBuilder.Entity("DataAccess.Models.Entity.WorkPlaceHourRestriction", b =>
                {
                    b.Navigation("HourRestrictions");
                });
#pragma warning restore 612, 618
        }
    }
}
