using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess   
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Empoyees { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Phone> Phone { get; set; }
        public DbSet<EmployeeContact> EmployeeContact { get; set; }
        public DbSet<Specialization> Specialization { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}



