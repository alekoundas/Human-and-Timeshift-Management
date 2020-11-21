using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Business.Seed
{
    public class InitializeIdentityData
    {
        public static void SeedData(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        protected static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "Admin@Admin.gr",
                    FirstName = "Admin",
                    LastName = "User",
                    HasToChangePassword = false
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    //Employee
                    userManager.AddToRoleAsync(user, "Employee_View").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Create").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Delete").Wait();

                    //Contract
                    userManager.AddToRoleAsync(user, "Contract_View").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Create").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Delete").Wait();

                    //ContractType
                    userManager.AddToRoleAsync(user, "ContractType_View").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Create").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Edit").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Delete").Wait();

                    //ContractMembership
                    userManager.AddToRoleAsync(user, "ContractMembership_View").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Create").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Edit").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Delete").Wait();

                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
                    userManager.AddToRoleAsync(user, "User_Create").Wait();
                    userManager.AddToRoleAsync(user, "User_Edit").Wait();
                    userManager.AddToRoleAsync(user, "User_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "User_Delete").Wait();

                    //Specialization
                    userManager.AddToRoleAsync(user, "Specialization_View").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Create").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Delete").Wait();

                    //Company
                    userManager.AddToRoleAsync(user, "Company_View").Wait();
                    userManager.AddToRoleAsync(user, "Company_Create").Wait();
                    userManager.AddToRoleAsync(user, "Company_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Company_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Company_Delete").Wait();

                    //Customer
                    userManager.AddToRoleAsync(user, "Customer_View").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Create").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Delete").Wait();

                    //WorkPlace
                    userManager.AddToRoleAsync(user, "WorkPlace_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Edit").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Delete").Wait();

                    //TimeShift
                    userManager.AddToRoleAsync(user, "TimeShift_View").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Create").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Edit").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Delete").Wait();

                    //WorkPlaceHourRestriction
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Delete").Wait();

                    //Leave
                    userManager.AddToRoleAsync(user, "Leave_View").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Create").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "Leave_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Delete").Wait();

                    //LeaveType
                    userManager.AddToRoleAsync(user, "LeaveType_View").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Create").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Edit").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Delete").Wait();

                    //RealWorkhour
                    userManager.AddToRoleAsync(user, "RealWorkHour_View").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Create").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "RealWorkHour_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Delete").Wait();

                    //Projection
                    userManager.AddToRoleAsync(user, "ProjectionDifference_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionConcentric_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursAnalytically_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursAnalyticallySum_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursSpecificDates_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionPresenceDaily_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionEmployeeRealHoursSum_View").Wait();
                }
            }


            if (userManager.FindByNameAsync("SuperAdmin").Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "SuperAdmin",
                    Email = "Super@Admin.gr",
                    FirstName = "Super",
                    LastName = "User",
                    HasToChangePassword = false

                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    //Employee
                    userManager.AddToRoleAsync(user, "Employee_View").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Create").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Delete").Wait();

                    //Contract
                    userManager.AddToRoleAsync(user, "Contract_View").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Create").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Delete").Wait();

                    //ContractType
                    userManager.AddToRoleAsync(user, "ContractType_View").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Create").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Edit").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Delete").Wait();

                    //ContractMembership
                    userManager.AddToRoleAsync(user, "ContractMembership_View").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Create").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Edit").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Delete").Wait();

                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
                    userManager.AddToRoleAsync(user, "User_Create").Wait();
                    userManager.AddToRoleAsync(user, "User_Edit").Wait();
                    userManager.AddToRoleAsync(user, "User_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "User_Delete").Wait();

                    //Specialization
                    userManager.AddToRoleAsync(user, "Specialization_View").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Create").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Delete").Wait();

                    //Company
                    userManager.AddToRoleAsync(user, "Company_View").Wait();
                    userManager.AddToRoleAsync(user, "Company_Create").Wait();
                    userManager.AddToRoleAsync(user, "Company_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Company_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Company_Delete").Wait();

                    //Customer
                    userManager.AddToRoleAsync(user, "Customer_View").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Create").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Delete").Wait();

                    //WorkPlace
                    userManager.AddToRoleAsync(user, "WorkPlace_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Edit").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Delete").Wait();

                    //TimeShift
                    userManager.AddToRoleAsync(user, "TimeShift_View").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Create").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Edit").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Delete").Wait();

                    //WorkPlaceHourRestriction
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Delete").Wait();

                    //Leave
                    userManager.AddToRoleAsync(user, "Leave_View").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Create").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "Leave_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Delete").Wait();

                    //LeaveType
                    userManager.AddToRoleAsync(user, "LeaveType_View").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Create").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Edit").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Delete").Wait();

                    //RealWorkhour
                    userManager.AddToRoleAsync(user, "RealWorkHour_View").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Create").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "RealWorkHour_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Delete").Wait();

                    //Projection
                    userManager.AddToRoleAsync(user, "ProjectionDifference_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionConcentric_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursAnalytically_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursAnalyticallySum_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursSpecificDates_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionPresenceDaily_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionEmployeeRealHoursSum_View").Wait();
                }
            }
        }

        protected static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            //Employee
            CreateRole(roleManager, "Employee", "Υπάλλήλου", "View");
            CreateRole(roleManager, "Employee", "Υπάλλήλου", "Create");
            CreateRole(roleManager, "Employee", "Υπάλλήλου", "Edit");
            CreateRole(roleManager, "Employee", "Υπάλλήλου", "Deactivate");
            CreateRole(roleManager, "Employee", "Υπάλλήλου", "Delete");

            //Contract
            CreateRole(roleManager, "Contract", "Σύμβαση", "View");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Create");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Edit");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Deactivate");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Delete");

            //ContractType
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "View");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Create");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Edit");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Deactivate");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Delete");

            //ContractMembership
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "View");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Create");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Edit");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Deactivate");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Delete");

            //User
            CreateRole(roleManager, "User", "Χρήστη", "View");
            CreateRole(roleManager, "User", "Χρήστη", "Create");
            CreateRole(roleManager, "User", "Χρήστη", "Edit");
            CreateRole(roleManager, "User", "Χρήστη", "Deactivate");
            CreateRole(roleManager, "User", "Χρήστη", "Delete");

            //Specialization
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "View");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Create");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Edit");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Deactivate");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Delete");

            //Company
            CreateRole(roleManager, "Company", "Εταιρία", "View");
            CreateRole(roleManager, "Company", "Εταιρία", "Create");
            CreateRole(roleManager, "Company", "Εταιρία", "Edit");
            CreateRole(roleManager, "Company", "Εταιρία", "Deactivate");
            CreateRole(roleManager, "Company", "Εταιρία", "Delete");

            //Customer
            CreateRole(roleManager, "Customer", "Πελάτη", "View");
            CreateRole(roleManager, "Customer", "Πελάτη", "Create");
            CreateRole(roleManager, "Customer", "Πελάτη", "Edit");
            CreateRole(roleManager, "Customer", "Πελάτη", "Deactivate");
            CreateRole(roleManager, "Customer", "Πελάτη", "Delete");

            //WorkPlace
            CreateRole(roleManager, "WorkPlace", "Πόστο", "View");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Create");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Edit");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Deactivate");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Delete");

            //TimeShift
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "View");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Create");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Edit");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Deactivate");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Delete");

            //Leave
            CreateRole(roleManager, "Leave", "Άδεια", "View");
            CreateRole(roleManager, "Leave", "Άδεια", "Create");
            CreateRole(roleManager, "Leave", "Άδεια", "Edit");
            //CreateRole(roleManager, "Leave","Άδεια",  "Deactivate");
            CreateRole(roleManager, "Leave", "Άδεια", "Delete");

            //LeaveType
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "View");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Create");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Edit");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Deactivate");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Delete");

            //RealWorkhour
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "View");
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Create");
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Edit");
            //CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Deactivate");
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Delete");

            //WorkPlaceHourRestriction
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "View");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Create");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Edit");
            //CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Deactivate");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Delete");

            //Projection
            CreateRole(roleManager, "ProjectionDifference", "Προβολή_Διαφορές", "View");
            CreateRole(roleManager, "ProjectionConcentric", "Προβολή_Συγκεντρωτικό", "View");
            CreateRole(roleManager, "ProjectionRealWorkHoursAnalytically", "Προβολή_Π.ΒάρδιεςΑναλ", "View");
            CreateRole(roleManager, "ProjectionRealWorkHoursAnalyticallySum", "Προβολή_Π.ΒάρδιεςΑναλΩρες", "View");
            CreateRole(roleManager, "ProjectionRealWorkHoursSpecificDates", "Προβολή_ΕπιλεγμένεςΗμ", "View");
            CreateRole(roleManager, "ProjectionEmployeeRealHoursSum", "Προβολή_ώρεςΑναΕργαζ", "View");
            CreateRole(roleManager, "ProjectionPresenceDaily", "Προβολή_ΠαρουσίεςΗμ", "View");

        }

        private static void CreateRole(RoleManager<ApplicationRole> roleManager, string controllerName, string greekName, string permitionName)
        {
            var roleFullName = controllerName + "_" + permitionName;
            if (!roleManager.RoleExistsAsync(roleFullName).Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = roleFullName;
                role.Controller = controllerName;
                role.Permition = permitionName;
                role.GreekName = greekName + GetGreekPermition(permitionName);
                var roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        private static string GetGreekPermition(string permitionName)
        {
            switch (permitionName)
            {
                case "View":
                    return "_Προβολή";
                case "Create":
                    return "_Δημιουργία";
                case "Edit":
                    return "_Επεξεργασία";
                case "Deactivate":
                    return "_Απενεργοποίηση";
                case "Delete":
                    return "_Διαγραφή";
                default:
                    return "";
            }
        }

    }
}
