using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;

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
                    HasToChangePassword = false,
                    CreatedBy_Id = "000",
                    CreatedBy_FullName = "System",
                    CreatedOn = DateTime.Now
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
                    userManager.AddToRoleAsync(user, "Employee_Import").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Export").Wait();

                    //Contract
                    userManager.AddToRoleAsync(user, "Contract_View").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Create").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Import").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Export").Wait();

                    //ContractType
                    userManager.AddToRoleAsync(user, "ContractType_View").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Create").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Edit").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Delete").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Import").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Export").Wait();

                    //ContractMembership
                    userManager.AddToRoleAsync(user, "ContractMembership_View").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Create").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Edit").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Delete").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Import").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Export").Wait();

                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
                    userManager.AddToRoleAsync(user, "User_Create").Wait();
                    userManager.AddToRoleAsync(user, "User_Edit").Wait();
                    userManager.AddToRoleAsync(user, "User_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "User_Delete").Wait();
                    userManager.AddToRoleAsync(user, "User_Import").Wait();
                    userManager.AddToRoleAsync(user, "User_Export").Wait();

                    //Specialization
                    userManager.AddToRoleAsync(user, "Specialization_View").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Create").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Import").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Export").Wait();

                    //Company
                    userManager.AddToRoleAsync(user, "Company_View").Wait();
                    userManager.AddToRoleAsync(user, "Company_Create").Wait();
                    userManager.AddToRoleAsync(user, "Company_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Company_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Company_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Company_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Company_Export").Wait();

                    //Customer
                    userManager.AddToRoleAsync(user, "Customer_View").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Create").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Import").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Export").Wait();

                    //WorkPlace
                    userManager.AddToRoleAsync(user, "WorkPlace_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Edit").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Delete").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Import").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Export").Wait();

                    //TimeShift
                    userManager.AddToRoleAsync(user, "TimeShift_View").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Create").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Edit").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Delete").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Import").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Export").Wait();

                    //WorkPlaceHourRestriction
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Delete").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Import").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Export").Wait();

                    //Leave
                    userManager.AddToRoleAsync(user, "Leave_View").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Create").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "Leave_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Import").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Export").Wait();

                    //LeaveType
                    userManager.AddToRoleAsync(user, "LeaveType_View").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Create").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Edit").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Delete").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Import").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Export").Wait();

                    //RealWorkhour
                    userManager.AddToRoleAsync(user, "RealWorkHour_View").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Create").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "RealWorkHour_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Delete").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Import").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Export").Wait();

                    //Projection
                    userManager.AddToRoleAsync(user, "ProjectionDifference_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionConcentric_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursAnalytically_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursAnalyticallySum_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursSpecificDates_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionPresenceDaily_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionEmployeeRealHoursSum_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHourRestrictions_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionTimeShiftSuggestions_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionHoursWithComments_View").Wait();
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
                    HasToChangePassword = false,
                    CreatedBy_Id = "000",
                    CreatedBy_FullName = "System",
                    CreatedOn = DateTime.Now

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
                    userManager.AddToRoleAsync(user, "Employee_Import").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Export").Wait();

                    //Contract
                    userManager.AddToRoleAsync(user, "Contract_View").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Create").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Import").Wait();
                    userManager.AddToRoleAsync(user, "Contract_Export").Wait();

                    //ContractType
                    userManager.AddToRoleAsync(user, "ContractType_View").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Create").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Edit").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Delete").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Import").Wait();
                    userManager.AddToRoleAsync(user, "ContractType_Export").Wait();

                    //ContractMembership
                    userManager.AddToRoleAsync(user, "ContractMembership_View").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Create").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Edit").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Delete").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Import").Wait();
                    userManager.AddToRoleAsync(user, "ContractMembership_Export").Wait();

                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
                    userManager.AddToRoleAsync(user, "User_Create").Wait();
                    userManager.AddToRoleAsync(user, "User_Edit").Wait();
                    userManager.AddToRoleAsync(user, "User_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "User_Delete").Wait();
                    userManager.AddToRoleAsync(user, "User_Import").Wait();
                    userManager.AddToRoleAsync(user, "User_Export").Wait();

                    //Specialization
                    userManager.AddToRoleAsync(user, "Specialization_View").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Create").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Import").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Export").Wait();

                    //Company
                    userManager.AddToRoleAsync(user, "Company_View").Wait();
                    userManager.AddToRoleAsync(user, "Company_Create").Wait();
                    userManager.AddToRoleAsync(user, "Company_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Company_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Company_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Company_Import").Wait();
                    userManager.AddToRoleAsync(user, "Company_Export").Wait();

                    //Customer
                    userManager.AddToRoleAsync(user, "Customer_View").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Create").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Import").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Export").Wait();

                    //WorkPlace
                    userManager.AddToRoleAsync(user, "WorkPlace_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Edit").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Delete").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Import").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Export").Wait();

                    //TimeShift
                    userManager.AddToRoleAsync(user, "TimeShift_View").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Create").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Edit").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Delete").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Import").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Export").Wait();

                    //WorkPlaceHourRestriction
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Delete").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Import").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlaceHourRestriction_Export").Wait();

                    //Leave
                    userManager.AddToRoleAsync(user, "Leave_View").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Create").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "Leave_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Delete").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Import").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Export").Wait();

                    //LeaveType
                    userManager.AddToRoleAsync(user, "LeaveType_View").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Create").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Edit").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Delete").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Import").Wait();
                    userManager.AddToRoleAsync(user, "LeaveType_Export").Wait();

                    //RealWorkhour
                    userManager.AddToRoleAsync(user, "RealWorkHour_View").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Create").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Edit").Wait();
                    //userManager.AddToRoleAsync(user, "RealWorkHour_Deactivate").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Delete").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Import").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkHour_Export").Wait();

                    //Projection
                    userManager.AddToRoleAsync(user, "ProjectionDifference_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionConcentric_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursAnalytically_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursAnalyticallySum_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHoursSpecificDates_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionPresenceDaily_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionEmployeeRealHoursSum_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionRealWorkHourRestrictions_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionTimeShiftSuggestions_View").Wait();
                    userManager.AddToRoleAsync(user, "ProjectionHoursWithComments_View").Wait();
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
            CreateRole(roleManager, "Employee", "Υπάλλήλου", "Import");
            CreateRole(roleManager, "Employee", "Υπάλλήλου", "Export");

            //Contract
            CreateRole(roleManager, "Contract", "Σύμβαση", "View");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Create");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Edit");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Deactivate");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Delete");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Import");
            CreateRole(roleManager, "Contract", "Σύμβαση", "Export");

            //ContractType
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "View");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Create");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Edit");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Deactivate");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Delete");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Import");
            CreateRole(roleManager, "ContractType", "ΤύποςΣύμβασης", "Export");

            //ContractMembership
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "View");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Create");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Edit");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Deactivate");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Delete");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Import");
            CreateRole(roleManager, "ContractMembership", "ΙδιότηταΣύμβασης", "Export");

            //User
            CreateRole(roleManager, "User", "Χρήστη", "View");
            CreateRole(roleManager, "User", "Χρήστη", "Create");
            CreateRole(roleManager, "User", "Χρήστη", "Edit");
            CreateRole(roleManager, "User", "Χρήστη", "Deactivate");
            CreateRole(roleManager, "User", "Χρήστη", "Delete");
            CreateRole(roleManager, "User", "Χρήστη", "Import");
            CreateRole(roleManager, "User", "Χρήστη", "Export");

            //Specialization
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "View");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Create");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Edit");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Deactivate");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Delete");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Import");
            CreateRole(roleManager, "Specialization", "Ειδικότητα", "Export");

            //Company
            CreateRole(roleManager, "Company", "Εταιρία", "View");
            CreateRole(roleManager, "Company", "Εταιρία", "Create");
            CreateRole(roleManager, "Company", "Εταιρία", "Edit");
            CreateRole(roleManager, "Company", "Εταιρία", "Deactivate");
            CreateRole(roleManager, "Company", "Εταιρία", "Delete");
            CreateRole(roleManager, "Company", "Εταιρία", "Import");
            CreateRole(roleManager, "Company", "Εταιρία", "Export");

            //Customer
            CreateRole(roleManager, "Customer", "Πελάτη", "View");
            CreateRole(roleManager, "Customer", "Πελάτη", "Create");
            CreateRole(roleManager, "Customer", "Πελάτη", "Edit");
            CreateRole(roleManager, "Customer", "Πελάτη", "Deactivate");
            CreateRole(roleManager, "Customer", "Πελάτη", "Delete");
            CreateRole(roleManager, "Customer", "Πελάτη", "Import");
            CreateRole(roleManager, "Customer", "Πελάτη", "Export");

            //WorkPlace
            CreateRole(roleManager, "WorkPlace", "Πόστο", "View");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Create");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Edit");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Deactivate");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Delete");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Import");
            CreateRole(roleManager, "WorkPlace", "Πόστο", "Export");

            //TimeShift
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "View");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Create");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Edit");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Deactivate");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Delete");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Import");
            CreateRole(roleManager, "TimeShift", "Χρονοδίαγραμμα", "Export");

            //Leave
            CreateRole(roleManager, "Leave", "Άδεια", "View");
            CreateRole(roleManager, "Leave", "Άδεια", "Create");
            CreateRole(roleManager, "Leave", "Άδεια", "Edit");
            //CreateRole(roleManager, "Leave","Άδεια",  "Deactivate");
            CreateRole(roleManager, "Leave", "Άδεια", "Delete");
            CreateRole(roleManager, "Leave", "Άδεια", "Import");
            CreateRole(roleManager, "Leave", "Άδεια", "Export");

            //LeaveType
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "View");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Create");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Edit");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Deactivate");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Delete");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Import");
            CreateRole(roleManager, "LeaveType", "ΕίδοςΆδειας", "Export");

            //RealWorkhour
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "View");
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Create");
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Edit");
            //CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Deactivate");
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Delete");
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Import");
            CreateRole(roleManager, "RealWorkHour", "ΠραγματικήΒάρδια", "Export");

            //WorkPlaceHourRestriction
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "View");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Create");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Edit");
            //CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Deactivate");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Delete");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Import");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "ΠεριορισμόςΠόστου", "Export");

            //Projection
            CreateRole(roleManager, "ProjectionDifference", "Προβολή_Διαφορές", "View");
            CreateRole(roleManager, "ProjectionConcentric", "Προβολή_Συγκεντρωτικό", "View");
            CreateRole(roleManager, "ProjectionRealWorkHoursAnalytically", "Προβολή_Π.ΒάρδιεςΑναλ", "View");
            CreateRole(roleManager, "ProjectionRealWorkHoursAnalyticallySum", "Προβολή_Π.ΒάρδιεςΑναλΩρες", "View");
            CreateRole(roleManager, "ProjectionRealWorkHoursSpecificDates", "Προβολή_ΕπιλεγμένεςΗμ", "View");
            CreateRole(roleManager, "ProjectionEmployeeRealHoursSum", "Προβολή_ώρεςΑναΕργαζ", "View");
            CreateRole(roleManager, "ProjectionPresenceDaily", "Προβολή_ΠαρουσίεςΗμ", "View");
            CreateRole(roleManager, "ProjectionRealWorkHourRestrictions", "Προβολή_ΠαρουσίεςΗμ", "View");
            CreateRole(roleManager, "ProjectionTimeShiftSuggestions", "Προβολή_ΥποδείξειςΧρονοδιαγράμματος", "View");
            CreateRole(roleManager, "ProjectionHoursWithComments", "Προβολή_ΒάρδιεςΜεΣχόλια", "View");

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
                case "Import":
                    return "_Import";
                case "Export":
                    return "_Export";
                default:
                    return "";
            }
        }

    }
}
