using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.Audit
{
    public class AuditAutoHistory : AutoHistory
    {
        [Required]
        public string ModifiedBy_FullName { get; set; }
        [Required]
        public string ModifiedBy_Id { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
