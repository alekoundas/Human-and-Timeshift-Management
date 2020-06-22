using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class Phone :Entity
    {
        public string Number { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
