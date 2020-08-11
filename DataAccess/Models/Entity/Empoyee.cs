using System;
using System.Collections.Generic;
using DataAccess.Models.Entity;

namespace DataAccess.Models.Entity
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string ErpCode { get; set; }
        public string Afm { get; set; }
        public string  SocialSecurityNumber { get; set; }


        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }


        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<Contact> Contacts{ get; set; }
        public ICollection<WorkHour> WorkHours { get; set; }
        public ICollection<RealWorkHour> RealWorkHours { get; set; }
        public ICollection<EmployeeWorkPlace> EmployeeWorkPlaces { get; set; }
        public ICollection<Leave> Leaves { get; set; }


    }
}
