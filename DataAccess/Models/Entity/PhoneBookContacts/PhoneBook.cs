using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models.Entity.PhoneBookContacts
{
    public class PhoneBook : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public ICollection<Contact> Contacts { get; set; }
    }
}
