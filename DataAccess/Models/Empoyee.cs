using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public class Employee : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string ErpCode { get; set; }
        public string Afm { get; set; }
        public string  SocialSecurityNumber { get; set; }

        public Specialization Specializations { get; set; }

        public int EmployeeContactId { get; set; }
        public ICollection<EmployeeContact> EmployeeContact { get; set; }
    }
}
