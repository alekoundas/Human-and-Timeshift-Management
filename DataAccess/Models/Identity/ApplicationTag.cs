using DataAccess.Models.Audit;
using System.Collections.Generic;

namespace DataAccess.Models.Identity
{
    public class ApplicationTag : BaseEntity
    {
        public string Title { get; set; }
        public ICollection<ApplicationUserTag> ApplicationUserTags { get; set; }
    }
}
