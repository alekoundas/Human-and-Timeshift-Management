using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels.LeaveTypes
{
   public class CreateLeaveType
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
        public static LeaveType CreateFrom(CreateLeaveType viewModel)
        {
            return new LeaveType()
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                CreatedOn = DateTime.Now

            };
        }
    }



}
