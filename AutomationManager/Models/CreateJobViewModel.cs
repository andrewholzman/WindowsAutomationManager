using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutomationManager.Models
{
    public class CreateJobViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string Description { get; set; }
        [NotMapped]
        public string Server { get; set; }
        [NotMapped]
        public CreateJobStepViewModel Step { get; set; }
        [NotMapped]
        public JobScheduleViewModel Schedule { get; set; }


    }

    public class JobScheduleViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        private List<String> _selectedDays { get; set; }
        //[NotMapped]
        //public List<SelectListItem> DaysOfWeek { get; } = new List<SelectListItem>
        //{
        //    new SelectListItem { Value = "1", Text = "Sunday" },
        //    new SelectListItem { Value = "2", Text = "Monday" },
        //    new SelectListItem { Value = "4", Text = "Tuesday" },
        //    new SelectListItem { Value = "8", Text = "Wednesday" },
        //    new SelectListItem { Value = "16", Text = "Thursday" },
        //    new SelectListItem { Value = "32", Text = "Friday" },
        //    new SelectListItem { Value = "64", Text = "Saturday" },
        //} ;
        [NotMapped]
        private List<SelectListItem> _daysOfWeek;
        [NotMapped]
        public List<SelectListItem> DaysOfWeek
        {
            get
            {
                return new List<SelectListItem> {
            new SelectListItem { Value = "1", Text = "Sunday" },
            new SelectListItem { Value = "2", Text = "Monday" },
            new SelectListItem { Value = "4", Text = "Tuesday" },
            new SelectListItem { Value = "8", Text = "Wednesday" },
            new SelectListItem { Value = "16", Text = "Thursday" },
            new SelectListItem { Value = "32", Text = "Friday" },
            new SelectListItem { Value = "64", Text = "Saturday" }
                };
            }
            set
            {
                _daysOfWeek = value;
                foreach(SelectListItem item in _daysOfWeek)
                {
                    if (item.Selected)
                    {
                        _selectedDays.Add(item.Value);
                    }
                }
            }
        }

        [NotMapped]
        public DateTime RunTime { get; set; }

    }

    public class CreateJobStepViewModel
    {
        public string Name { get; set; }
        public string Command { get; set; }
    }
}
