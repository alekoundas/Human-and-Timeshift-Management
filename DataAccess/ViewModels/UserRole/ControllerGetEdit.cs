using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Identity;

namespace DataAccess.ViewModels.UserRole
{
    public class ControllerGetEdit : ApplicationUser
    {
        public List<WorkPlaceRoleValues> WorkPlaceRoles { get; set; }

        public static ControllerGetEdit CreateFrom(ApplicationUser user) =>
            new ControllerGetEdit()
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
