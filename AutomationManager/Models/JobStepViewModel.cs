using System;

namespace AutomationManager.Models
{
    public class JobStepViewModel
    {
        public JobStepViewModel(int iD, string command, string database, string name, DateTime lastRunDate, string parentJobName)
        {
            ID = iD;
            Command = command;
            Database = database;
            Name = name;
            LastRunDate = lastRunDate;
            ParentJobName = parentJobName;
        }

        public int ID { get; set; }
        public string Command { get; set; }
        public string Database { get; set; }
        public string Name { get; set; }
        public DateTime LastRunDate { get; set; }
        public string ParentJobName { get; set; }
        
    }
}