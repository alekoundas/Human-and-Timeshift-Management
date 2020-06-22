using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Contact: Entity
    {
        public int PhoneId { get; set; }
        public Phone Phone { get; set; }
    }
}
