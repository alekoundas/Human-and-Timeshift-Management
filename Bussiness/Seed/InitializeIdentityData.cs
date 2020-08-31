using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Authorization;
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
                    UserName = "Admin@Admin",
                    Email = "Admin@Admin"
                };

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    //Employee
                    userManager.AddToRoleAsync(user, "Employee_View").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Create").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Delete").Wait();
                    
                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
                    userManager.AddToRoleAsync(user, "User_Create").Wait();
                    userManager.AddToRoleAsync(user, "User_Edit").Wait();
                    userManager.AddToRoleAsync(user, "User_Delete").Wait();

                    //Specialization
                    userManager.AddToRoleAsync(user, "Specialization_View").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Create").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Delete").Wait();

                    //Company
                    userManager.AddToRoleAsync(user, "Company_View").Wait();
                    userManager.AddToRoleAsync(user, "Company_Create").Wait();
                    userManager.AddToRoleAsync(user, "Company_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Company_Delete").Wait();

                    //Customer
                    userManager.AddToRoleAsync(user, "Customer_View").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Create").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Delete").Wait();

                    //WorkPlace
                    userManager.AddToRoleAsync(user, "WorkPlace_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Edit").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Delete").Wait();

                    //TimeShift
                    userManager.AddToRoleAsync(user, "TimeShift_View").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Create").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Edit").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Delete").Wait();

                    //Leave
                    userManager.AddToRoleAsync(user, "Leave_View").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Create").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Delete").Wait();

                    //RealWorkhour
                    userManager.AddToRoleAsync(user, "RealWorkhour_View").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkhour_Create").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkhour_Edit").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkhour_Delete").Wait();

                    //Projection
                    userManager.AddToRoleAsync(user, "ProjectionDifference_View").Wait();

                }
            }


            if (userManager.FindByNameAsync("SuperAdmin").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Super@Admin";
                user.Email = "Super@Admin";

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    //Employee
                    userManager.AddToRoleAsync(user, "Employee_View").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Create").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Delete").Wait();

                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
                    userManager.AddToRoleAsync(user, "User_Create").Wait();
                    userManager.AddToRoleAsync(user, "User_Edit").Wait();
                    userManager.AddToRoleAsync(user, "User_Delete").Wait();

                    //Specialization
                    userManager.AddToRoleAsync(user, "Specialization_View").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Create").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Specialization_Delete").Wait();

                    //Company
                    userManager.AddToRoleAsync(user, "Company_View").Wait();
                    userManager.AddToRoleAsync(user, "Company_Create").Wait();
                    userManager.AddToRoleAsync(user, "Company_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Company_Delete").Wait();

                    //Customer
                    userManager.AddToRoleAsync(user, "Customer_View").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Create").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Customer_Delete").Wait();

                    //WorkPlace
                    userManager.AddToRoleAsync(user, "WorkPlace_View").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Create").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Edit").Wait();
                    userManager.AddToRoleAsync(user, "WorkPlace_Delete").Wait();

                    //TimeShift
                    userManager.AddToRoleAsync(user, "TimeShift_View").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Create").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Edit").Wait();
                    userManager.AddToRoleAsync(user, "TimeShift_Delete").Wait();

                    //Leave
                    userManager.AddToRoleAsync(user, "Leave_View").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Create").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Leave_Delete").Wait();

                    //RealWorkhour
                    userManager.AddToRoleAsync(user, "RealWorkhour_View").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkhour_Create").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkhour_Edit").Wait();
                    userManager.AddToRoleAsync(user, "RealWorkhour_Delete").Wait();

                    //Projection
                    userManager.AddToRoleAsync(user, "ProjectionDifference_View").Wait();
                }
            }
        }

        protected static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            //Employee
            if (!roleManager.RoleExistsAsync("Employee_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Employee_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition= role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee_Create").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Employee_Create";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee_Edit").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Employee_Edit";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee_Delete").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Employee_Delete";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            //User
            if (!roleManager.RoleExistsAsync("User_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "User_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User_Create").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "User_Create";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User_Edit").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "User_Edit";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User_Delete").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "User_Delete";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            //Specialization
            if (!roleManager.RoleExistsAsync("Specialization_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Specialization_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Specialization_Create").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Specialization_Create";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Specialization_Edit").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Specialization_Edit";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Specialization_Delete").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Specialization_Delete";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            //Company
            if (!roleManager.RoleExistsAsync("Company_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Company_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Company_Create").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Company_Create";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Company_Edit").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Company_Edit";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Company_Delete").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Company_Delete";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            //Customer
            if (!roleManager.RoleExistsAsync("Customer_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Customer_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Customer_Create").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Customer_Create";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Customer_Edit").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Customer_Edit";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Customer_Delete").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Customer_Delete";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            //WorkPlace
            if (!roleManager.RoleExistsAsync("WorkPlace_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "WorkPlace_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("WorkPlace_Create").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "WorkPlace_Create";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("WorkPlace_Edit").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "WorkPlace_Edit";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("WorkPlace_Delete").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "WorkPlace_Delete";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            //TimeShift
            if (!roleManager.RoleExistsAsync("TimeShift_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "TimeShift_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("TimeShift_Create").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "TimeShift_Create";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("TimeShift_Edit").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "TimeShift_Edit";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("TimeShift_Delete").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "TimeShift_Delete";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            //Leave
            if (!roleManager.RoleExistsAsync("Leave_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Leave_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Leave_Create").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Leave_Create";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Leave_Edit").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Leave_Edit";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Leave_Delete").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "Leave_Delete";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            //RealWorkhour
            if (!roleManager.RoleExistsAsync("RealWorkhour_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "RealWorkhour_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("RealWorkhour_Create").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "RealWorkhour_Create";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("RealWorkhour_Edit").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "RealWorkhour_Edit";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("RealWorkhour_Delete").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "RealWorkhour_Delete";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            //Projection
            if (!roleManager.RoleExistsAsync("ProjectionDifference_View").Result)
            {
                ApplicationRole role = new ApplicationRole();
                role.Name = "ProjectionDifference_View";
                role.Controller = role.Name.Split('_')[0];
                role.Permition = role.Name.Split('_')[1];
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            
            }
        }
    }
}
