using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity.PhoneBookContacts;

namespace DataAccess.Models.Entity
{
    public class Customer: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string AFM { get; set; }
        public string  Description { get; set; }

        public int PhoneBookId { get; set; }
        public PhoneBook PhoneBook{ get; set; }

        public ICollection<WorkPlace> WorkPlaces { get; set; }
    }
}
