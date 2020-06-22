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
            RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }
        protected static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("Admin").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Admin@Admin";
                user.Email = "Admin@Admin";

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee_View").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Details").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Create").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Delete").Wait();
                    
                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
                    userManager.AddToRoleAsync(user, "User_Details").Wait();
                    userManager.AddToRoleAsync(user, "User_Create").Wait();
                    userManager.AddToRoleAsync(user, "User_Edit").Wait();
                    userManager.AddToRoleAsync(user, "User_Delete").Wait();


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
                    userManager.AddToRoleAsync(user, "Employee_Details").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Create").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Delete").Wait();     


                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
                    userManager.AddToRoleAsync(user, "User_Details").Wait();
                    userManager.AddToRoleAsync(user, "User_Create").Wait();
                    userManager.AddToRoleAsync(user, "User_Edit").Wait();
                    userManager.AddToRoleAsync(user, "User_Delete").Wait();
                }
            }
        }

        protected static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            //Employee
            if (!roleManager.RoleExistsAsync("Employee_View").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Employee_View";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee_Details").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Employee_Details";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee_Create").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Employee_Create";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee_Edit").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Employee_Edit";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Employee_Delete").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "Employee_Delete";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            //User
            if (!roleManager.RoleExistsAsync("User_View").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User_View";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User_Details").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User_Details";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User_Create").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User_Create";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User_Edit").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User_Edit";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("User_Delete").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "User_Delete";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
