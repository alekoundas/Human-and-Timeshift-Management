using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess.Models.Audit
{
    public class AuditAutoHistory : AutoHistory
    {
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
