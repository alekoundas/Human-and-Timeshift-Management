using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.Models.Entity
{
    public class Customer: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AFM { get; set; }
        public string  Description { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<Contact> Contacts{ get; set; }

        public ICollection<WorkPlace> WorkPlaces { get; set; }
    }
}
