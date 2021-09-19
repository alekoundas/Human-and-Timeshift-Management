using DataAccess.Models.Security;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.ViewModels
{
    public class LogTypeEdit : LogTypeCreate
    {
        public int Id { get; set; }

        //Audit
        [Display(Name = "Δημηουργήθηκε απο")]
        public string CreatedBy_Id { get; set; }

        [Display(Name = "Δημηουργήθηκε απο")]
        public string CreatedBy_FullName { get; set; }

        [Display(Name = "Δημηουργήθηκε στις")]
        public DateTime CreatedOn { get; set; }


        public static LogTypeEdit CreateFrom(LogType model)
        {
            return new LogTypeEdit()
            {
                Title = model.Title,
                Title_GR = model.Title_GR,
                IsActive = true,
                CreatedBy_FullName = model.CreatedBy_FullName,
                CreatedBy_Id = model.CreatedBy_Id,
                CreatedOn = model.CreatedOn
            };
        }

        public static LogType CreateFrom(LogTypeEdit model)
        {
            return new LogType()
            {
                Title = model.Title,
                Title_GR = model.Title_GR,
                IsActive = true,
                CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                CreatedOn = DateTime.Now
            };
        }
    }
}
