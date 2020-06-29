using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models.Entity
{
    public class Contact : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
