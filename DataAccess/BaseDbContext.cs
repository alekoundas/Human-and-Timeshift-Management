using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models;
using DataAccess.Models.Configurations;
using DataAccess.Models.Entity;
using DataAccess.Models.Entity.WorkTimeShift;
using Microsoft.EntityFrameworkCore;

namespace DataAccess   
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<TimeShift> TimeShifts{ get; set; }
        public DbSet<WorkHour> WorkHours { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<WorkPlace> WorkPlaces { get; set; }
        //public DbSet<EmployeeWorkPlace> EmployeeWorkPlaces { get; set; }
        //public DbSet<EmployeeWorkHour> EmployeeWorkHours { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new EmployeeWorkPlaceConfiguration());
            builder.ApplyConfiguration(new EmployeeWorkHourConfiguration());
        }
    }
}



