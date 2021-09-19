using DataAccess;

namespace Bussiness.Seed
{
    public class InitializeBaseData
    {
        public static void SeedData(BaseDbContext context)
        {

            //LogEntity
            AddLogTypeIfNotExist(context, "Create", "Δημιουργία");
            AddLogTypeIfNotExist(context, "Delete", "Διαγραφή");
            AddLogTypeIfNotExist(context, "Edit", "Επεξεργασία");
            AddLogTypeIfNotExist(context, "Login", "Σύνδεση");
            AddLogTypeIfNotExist(context, "Logout", "Αποσύνδεση");

            //LogType
            AddLogEntityIfNotExist(context, "Leave", "Άδεια");
            AddLogEntityIfNotExist(context, "LogType", "Είδος Log");
            AddLogEntityIfNotExist(context, "Company", "Εταιρίες");
            AddLogEntityIfNotExist(context, "Customer", "Πελάτης");
            AddLogEntityIfNotExist(context, "Employee", "Υπάλληλος");
            AddLogEntityIfNotExist(context, "WorkHour", "Βάρδια");
            AddLogEntityIfNotExist(context, "Contract", "Σύμβαση");
            AddLogEntityIfNotExist(context, "WorkPlace", "Πόστο");
            AddLogEntityIfNotExist(context, "LeaveType", "Είδος άδειας");
            AddLogEntityIfNotExist(context, "TimeShift", "Χρονοδιάγραμμα");
            AddLogEntityIfNotExist(context, "LogEntity", "Log οντότητας");
            AddLogEntityIfNotExist(context, "ContractType", "Είδος Log");
            AddLogEntityIfNotExist(context, "RealWorkHour", "Π.Βάρδια");
            AddLogEntityIfNotExist(context, "Specialization", "Ειδικότητα");
            AddLogEntityIfNotExist(context, "HourRestriction", "Περιορισμοί Π.Βάρδιας");
            AddLogEntityIfNotExist(context, "EmployeeWorkPlace", "Υπάλληλος ανα Πόστο");
            AddLogEntityIfNotExist(context, "ContractMembership", "Ιδιότητα σύμβασης");
            context.SaveChanges();
        }

        private static void AddLogTypeIfNotExist(BaseDbContext context, string title, string title_GR)
        {
            //if (!context.LogTypes.Any(x => x.Title == title))
            //    context.Add(new LogType
            //    {
            //        Title = title,
            //        Title_GR = title_GR,
            //        CreatedBy_FullName = "Database Seeder",
            //        CreatedBy_Id = "",
            //        CreatedOn = DateTime.Now,
            //        IsActive = true
            //    });
        }

        private static void AddLogEntityIfNotExist(BaseDbContext context, string title, string title_GR)
        {
            //if (!context.LogEntities.Any(x => x.Title == title))
            //    context.Add(new LogEntity
            //    {
            //        Title = title,
            //        Title_GR = title_GR,
            //        CreatedBy_FullName = "Database Seeder",
            //        CreatedBy_Id = "",
            //        CreatedOn = DateTime.Now,
            //        IsActive = true
            //    });
        }

    }
}
