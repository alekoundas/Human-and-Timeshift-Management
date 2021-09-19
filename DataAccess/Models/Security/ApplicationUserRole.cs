using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models.Security
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {


        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }
}
