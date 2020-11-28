using DataAccess.Configurations;
using DataAccess.Models.Audit;
using DataAccess.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            : base(options)
        {
        }

        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<WorkHour> WorkHours { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<WorkPlace> WorkPlaces { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<TimeShift> TimeShifts { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<RealWorkHour> RealWorkHours { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<HourRestriction> HourRestrictions { get; set; }
        public DbSet<EmployeeWorkPlace> EmployeeWorkPlaces { get; set; }
        public DbSet<ContractMembership> ContractMemberships { get; set; }
        public DbSet<WorkPlaceHourRestriction> WorkPlaceHourRestrictions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.EnableAutoHistory<AuditAutoHistory>(x => { });


            builder.ApplyConfiguration(new LeaveConfiguration());
            builder.ApplyConfiguration(new CompanyConfiguration());
            builder.ApplyConfiguration(new ContactConfiguration());
            builder.ApplyConfiguration(new ContractConfiguration());
            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new WorkHourConfiguration());
            builder.ApplyConfiguration(new EmployeeConfiguration());
            builder.ApplyConfiguration(new LeaveTypeConfiguration());
            builder.ApplyConfiguration(new TimeShiftConfiguration());
            builder.ApplyConfiguration(new WorkPlaceConfiguration());
            builder.ApplyConfiguration(new ContractTypeConfiguration());
            builder.ApplyConfiguration(new RealWorkHourConfiguration());
            builder.ApplyConfiguration(new SpecializationConfiguration());
            builder.ApplyConfiguration(new HourRestrictionConfiguration());
            builder.ApplyConfiguration(new EmployeeWorkPlaceConfiguration());
            builder.ApplyConfiguration(new ContractMembershipConfiguration());
            builder.ApplyConfiguration(new WorkPlaceHourRestrictionConfiguration());
        }

    }
    
}
