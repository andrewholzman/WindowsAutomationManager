using System;
using System.Collections.Generic;
using System.Text;

namespace AM_History
{
    public class HistoryModel
    {
        public HistoryModel(Guid id, string jobType, string jobName, string dateRan, string status)
        {
            Id = id;
            JobType = jobType;
            JobName = jobName;
            DateRan = dateRan;
            Status = status;
        }
        public Guid Id { get; set; }
        public string JobType { get; set; }
        public string JobName { get; set; }
        public string DateRan { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
    }
}
