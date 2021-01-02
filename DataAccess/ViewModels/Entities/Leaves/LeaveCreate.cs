using DataAccess.Models.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class LeaveCreate
    {
        [Display(Name = "Έναρξη")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public DateTime StartOn { get; set; }

        [Display(Name = "Λήξη")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public DateTime EndOn { get; set; }

        [Display(Name = "Περιγραφή")]
        public string Description { get; set; }

        [Display(Name = "Εγκρίθηκε απο")]
        public string ApprovedBy { get; set; }

        [Display(Name = "Τύπος άδειας")]
        [Required(ErrorMessage = "Το παιδίο είναι υποχρεωτικό")]
        public int LeaveTypeId { get; set; }

        [Display(Name = "Υπάλληλοι")]
        public List<int> EmployeeIds { get; set; }

        public static List<Leave> CreateRangeFrom(LeaveCreate viewModel)
        {
            var results = new List<Leave>();
            foreach (var employeeId in viewModel.EmployeeIds)
            {
                results.Add(new Leave()
                {
                    StartOn = viewModel.StartOn,
                    EndOn = viewModel.EndOn,
                    Description = viewModel.Description,
                    ApprovedBy = viewModel.ApprovedBy,
                    LeaveTypeId = viewModel.LeaveTypeId,
                    EmployeeId = employeeId,
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                    CreatedOn = DateTime.Now
                });
            }
            return results;
        }
    }
}
