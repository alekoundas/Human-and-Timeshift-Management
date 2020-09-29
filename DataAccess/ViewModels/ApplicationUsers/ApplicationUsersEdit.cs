using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Identity;

namespace DataAccess.ViewModels.ApplicationUsers
{
    public class ApplicationUsersEdit : ApplicationUser
    {
        public List<WorkPlaceRoleValues> WorkPlaceRoles { get; set; }

        public static ApplicationUsersEdit CreateFrom(ApplicationUser user) =>
            new ApplicationUsersEdit()
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            };
    }

    public class WorkPlaceRoleValues
    {
        public string WorkPlaceId { get; set; }
        public string Name { get; set; }
    }

}
