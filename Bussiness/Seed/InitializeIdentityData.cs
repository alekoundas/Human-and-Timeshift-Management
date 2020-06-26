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
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Admin@Admin";
                user.Email = "Admin@Admin";

                IdentityResult result = userManager.CreateAsync(user, "P@ssw0rd").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Employee_View").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Create").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Delete").Wait();
                    
                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
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
                    userManager.AddToRoleAsync(user, "Employee_Create").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Edit").Wait();
                    userManager.AddToRoleAsync(user, "Employee_Delete").Wait();     


                    //User
                    userManager.AddToRoleAsync(user, "User_View").Wait();
                    userManager.AddToRoleAsync(user, "User_Create").Wait();
                    userManager.AddToRoleAsync(user, "User_Edit").Wait();
                    userManager.AddToRoleAsync(user, "User_Delete").Wait();
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
        }
    }
}
