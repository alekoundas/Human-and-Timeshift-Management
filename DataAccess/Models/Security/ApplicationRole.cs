using Microsoft.AspNetCore.Identity;

namespace DataAccess.Models.Security
{
    public class ApplicationRole : IdentityRole
    {
        public string Controller { get; set; }
        public string Permition { get; set; }
        public string WorkPlaceName { get; set; }
        public string WorkPlaceId { get; set; }
        public string GreekName { get; set; }
    }
}
