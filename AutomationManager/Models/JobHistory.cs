using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AutomationManager.Models
{
    public class JobHistory
    {
        public JobHistory(Guid id, string jobType, string jobName, string dateRan, string status, string error)
        {
            Id = id;
            JobType = jobType;
            JobName = jobName;
            DateRan = dateRan;
            Status = status;
            Error = error;
        }

        
        public Guid Id { get; set; }
        [DisplayName("Job Type")]
        public string JobType { get; set; }
        [DisplayName("Job Name")]
        public string JobName { get; set; }
        [DisplayName("Date Ran")]
        public string DateRan { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
    }
}
