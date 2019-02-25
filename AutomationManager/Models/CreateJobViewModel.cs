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
        public string ScheduleName { get; set; }
        [NotMapped]
        public List<String> SelectedDays { get; set; }

        [NotMapped]
        public string RunTime { get; set; }
        [NotMapped]
        public string StepName { get; set; }
        [NotMapped]
        public string Command { get; set; }

    }
}
