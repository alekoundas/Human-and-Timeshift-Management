using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Models.Entity
{
    public class Leave : BaseEntity
    {
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Έναρξη άδειας")]
        public DateTime StartOn { get; set; }

        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Λήξη άδειας")]
        public DateTime EndOn { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }

        [Display(Name = "Εγκρίθηκε από")]
        public string ApprovedBy { get; set; }


        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Υπάλληλος")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }


        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        [Display(Name = "Τύπος αδείας")]
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }

      
    }
}
