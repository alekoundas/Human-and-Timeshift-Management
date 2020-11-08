using System;

namespace DataAccess.Models
{
    public class BaseEntityIsActive
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
