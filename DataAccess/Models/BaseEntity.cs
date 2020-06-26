using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
