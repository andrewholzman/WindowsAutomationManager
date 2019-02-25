using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomationManager.Models
{
    public class JobStepViewModel
    {
        public JobStepViewModel() { }
        public JobStepViewModel(int iD, string command, string database, string name, DateTime lastRunDate, string parentJobName)
        {
            ID = iD;
            Command = command;
            Database = database;
            Name = name;
            LastRunDate = lastRunDate;
            ParentJobName = parentJobName;
        }
        [NotMapped]
        public int ID { get; set; }
        [NotMapped]
        public string Command { get; set; }
        [NotMapped]
        public string Database { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public DateTime LastRunDate { get; set; }
        [NotMapped]
        public string ParentJobName { get; set; }
        
    }
}