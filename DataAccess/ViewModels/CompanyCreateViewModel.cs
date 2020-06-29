﻿using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Entity;

namespace DataAccess.ViewModels
{
    public class CompanyCreateViewModel
    {
        public string AFM { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }


        public static Company CreateFrom(CompanyCreateViewModel viewModel)
        {
            return new Company()
            {
                Title = viewModel.Title,
                AFM = viewModel.AFM,
                Description = viewModel.Description,
                CreatedOn = DateTime.Now
            };
        }
    }
}
