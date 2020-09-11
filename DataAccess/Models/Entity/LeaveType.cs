using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccess.Models.Entity
{
    public class LeaveType :BaseEntity
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Είδος άδειας")]
        public string Name { get; set; }
        //public string Name {
        //    get { return Name + " - Άδεια"; }
        //    set { Name= value; }
        //}

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }
    }
}
