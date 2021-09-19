using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataAccess.Models.Security
{
    public class ApplicationRole : IdentityRole
    {
        public string Controller { get; set; }
        public string Permition { get; set; }
        public string WorkPlaceName { get; set; }
        public string WorkPlaceId { get; set; }
        public string GreekName { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}
