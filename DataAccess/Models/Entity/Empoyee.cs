using System;
using System.Collections.Generic;
using DataAccess.Models.Entity.PhoneBookContacts;
using DataAccess.Models.Entity.WorkTimeShift;

namespace DataAccess.Models.Entity
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string ErpCode { get; set; }
        public string Afm { get; set; }
        public string  SocialSecurityNumber { get; set; }


        public int MyProperty { get; set; }

        public int ScpecializationId { get; set; }
        public Specialization Specialization { get; set; }

        public int PhoneBookId { get; set; }
        public PhoneBook PhoneBook { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<WorkHour> WorkHours{ get; set; }
    }
}
