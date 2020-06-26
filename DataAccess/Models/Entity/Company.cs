using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models.Entity
{
    public class Company : BaseEntity
    {
        public string AFM { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public ICollection<Employee> Employees { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
