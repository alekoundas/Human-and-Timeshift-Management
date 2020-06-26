using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models.Entity.PhoneBookContacts
{
    public class Contact: BaseEntity
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
