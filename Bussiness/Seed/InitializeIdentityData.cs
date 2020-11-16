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
            CreateRole(roleManager, "Employee", "View");
            CreateRole(roleManager, "Employee", "Create");
            CreateRole(roleManager, "Employee", "Edit");
            CreateRole(roleManager, "Employee", "Deactivate");
            CreateRole(roleManager, "Employee", "Delete");

            //User
            CreateRole(roleManager, "User", "View");
            CreateRole(roleManager, "User", "Create");
            CreateRole(roleManager, "User", "Edit");
            CreateRole(roleManager, "User", "Deactivate");
            CreateRole(roleManager, "User", "Delete");

            //Specialization
            CreateRole(roleManager, "Specialization", "View");
            CreateRole(roleManager, "Specialization", "Create");
            CreateRole(roleManager, "Specialization", "Edit");
            CreateRole(roleManager, "Specialization", "Deactivate");
            CreateRole(roleManager, "Specialization", "Delete");

            //Company
            CreateRole(roleManager, "Company", "View");
            CreateRole(roleManager, "Company", "Create");
            CreateRole(roleManager, "Company", "Edit");
            CreateRole(roleManager, "Company", "Deactivate");
            CreateRole(roleManager, "Company", "Delete");

            //Customer
            CreateRole(roleManager, "Customer", "View");
            CreateRole(roleManager, "Customer", "Create");
            CreateRole(roleManager, "Customer", "Edit");
            CreateRole(roleManager, "Customer", "Deactivate");
            CreateRole(roleManager, "Customer", "Delete");

            //WorkPlace
            CreateRole(roleManager, "WorkPlace", "View");
            CreateRole(roleManager, "WorkPlace", "Create");
            CreateRole(roleManager, "WorkPlace", "Edit");
            CreateRole(roleManager, "WorkPlace", "Deactivate");
            CreateRole(roleManager, "WorkPlace", "Delete");

            //TimeShift
            CreateRole(roleManager, "TimeShift", "View");
            CreateRole(roleManager, "TimeShift", "Create");
            CreateRole(roleManager, "TimeShift", "Edit");
            CreateRole(roleManager, "TimeShift", "Deactivate");
            CreateRole(roleManager, "TimeShift", "Delete");

            //Leave
            CreateRole(roleManager, "Leave", "View");
            CreateRole(roleManager, "Leave", "Create");
            CreateRole(roleManager, "Leave", "Edit");
            //CreateRole(roleManager, "Leave", "Deactivate");
            CreateRole(roleManager, "Leave", "Delete");

            //LeaveType
            CreateRole(roleManager, "LeaveType", "View");
            CreateRole(roleManager, "LeaveType", "Create");
            CreateRole(roleManager, "LeaveType", "Edit");
            CreateRole(roleManager, "LeaveType", "Deactivate");
            CreateRole(roleManager, "LeaveType", "Delete");

            //RealWorkhour
            CreateRole(roleManager, "RealWorkHour", "View");
            CreateRole(roleManager, "RealWorkHour", "Create");
            CreateRole(roleManager, "RealWorkHour", "Edit");
            //CreateRole(roleManager, "RealWorkHour", "Deactivate");
            CreateRole(roleManager, "RealWorkHour", "Delete");

            //WorkPlaceHourRestriction
            CreateRole(roleManager, "WorkPlaceHourRestriction", "View");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "Create");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "Edit");
            //CreateRole(roleManager, "WorkPlaceHourRestriction", "Deactivate");
            CreateRole(roleManager, "WorkPlaceHourRestriction", "Delete");

            //Projection
            CreateRole(roleManager, "ProjectionRealWorkHoursSpecificDates", "View");
            CreateRole(roleManager, "ProjectionPresenceDaily", "View");
            CreateRole(roleManager, "ProjectionEmployeeRealHoursSum", "View");
            CreateRole(roleManager, "ProjectionRealWorkHoursAnalyticallySum", "View");
            CreateRole(roleManager, "ProjectionRealWorkHoursAnalytically", "View");
            CreateRole(roleManager, "ProjectionConcentric", "View");
            CreateRole(roleManager, "ProjectionDifference", "View");

        }

        private static void CreateRole(RoleManager<ApplicationRole> roleManager, string controllerName, string permitionName)
        {
            var roleFullName = controllerName + "_" + permitionName;
            if (!roleManager.RoleExistsAsync(roleFullName).Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = roleFullName;
                role.Controller = controllerName;
                role.Permition = permitionName;
                var roleResult = roleManager.CreateAsync(role).Result;
            }
        }

        //protected static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        //{
        //    //Employee
        //    if (!roleManager.RoleExistsAsync("Employee_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Employee_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Employee_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Employee_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Employee_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Employee_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Employee_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Employee_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }


        //    //User
        //    if (!roleManager.RoleExistsAsync("User_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "User_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("User_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "User_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("User_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "User_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("User_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "User_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //Specialization
        //    if (!roleManager.RoleExistsAsync("Specialization_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Specialization_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Specialization_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Specialization_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Specialization_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Specialization_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Specialization_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Specialization_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //Company
        //    if (!roleManager.RoleExistsAsync("Company_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Company_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Company_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Company_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Company_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Company_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Company_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Company_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //Customer
        //    if (!roleManager.RoleExistsAsync("Customer_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Customer_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Customer_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Customer_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Customer_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Customer_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Customer_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Customer_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //WorkPlace
        //    if (!roleManager.RoleExistsAsync("WorkPlace_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "WorkPlace_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("WorkPlace_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "WorkPlace_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("WorkPlace_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "WorkPlace_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("WorkPlace_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "WorkPlace_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //TimeShift
        //    if (!roleManager.RoleExistsAsync("TimeShift_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "TimeShift_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("TimeShift_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "TimeShift_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("TimeShift_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "TimeShift_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("TimeShift_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "TimeShift_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //Leave
        //    if (!roleManager.RoleExistsAsync("Leave_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Leave_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Leave_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Leave_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Leave_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Leave_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("Leave_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "Leave_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //LeaveType
        //    if (!roleManager.RoleExistsAsync("LeaveType_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "LeaveType_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("LeaveType_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "LeaveType_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("LeaveType_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "LeaveType_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("LeaveType_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "LeaveType_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //RealWorkhour
        //    if (!roleManager.RoleExistsAsync("RealWorkHour_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "RealWorkHour_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("RealWorkHour_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "RealWorkHour_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("RealWorkHour_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "RealWorkHour_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("RealWorkHour_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "RealWorkHour_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //WorkPlaceHourRestriction
        //    if (!roleManager.RoleExistsAsync("WorkPlaceHourRestriction_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "WorkPlaceHourRestriction_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("WorkPlaceHourRestriction_Create").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "WorkPlaceHourRestriction_Create";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("WorkPlaceHourRestriction_Edit").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "WorkPlaceHourRestriction_Edit";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }
        //    if (!roleManager.RoleExistsAsync("WorkPlaceHourRestriction_Delete").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "WorkPlaceHourRestriction_Delete";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;
        //    }

        //    //Projection
        //    if (!roleManager.RoleExistsAsync("ProjectionDifference_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "ProjectionDifference_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;

        //    }
        //    if (!roleManager.RoleExistsAsync("ProjectionConcentric_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "ProjectionConcentric_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;

        //    }
        //    if (!roleManager.RoleExistsAsync("ProjectionRealWorkHoursAnalytically_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "ProjectionRealWorkHoursAnalytically_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;

        //    }
        //    if (!roleManager.RoleExistsAsync("ProjectionRealWorkHoursAnalyticallySum_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "ProjectionRealWorkHoursAnalyticallySum_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;

        //    }
        //    if (!roleManager.RoleExistsAsync("ProjectionRealWorkHoursSpecificDates_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "ProjectionRealWorkHoursSpecificDates_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;

        //    }
        //    if (!roleManager.RoleExistsAsync("ProjectionPresenceDaily_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "ProjectionPresenceDaily_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;

        //    }
        //    if (!roleManager.RoleExistsAsync("ProjectionEmployeeRealHoursSum_View").Result)
        //    {
        //        ApplicationRole role = new ApplicationRole();
        //        role.Name = "ProjectionEmployeeRealHoursSum_View";
        //        role.Controller = role.Name.Split('_')[0];
        //        role.Permition = role.Name.Split('_')[1];
        //        IdentityResult roleResult = roleManager.CreateAsync(role).Result;

        //    }

        //}

    }
}
