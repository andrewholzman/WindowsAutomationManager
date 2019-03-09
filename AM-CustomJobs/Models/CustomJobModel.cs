using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AM_CustomJobs.Models
{
    public class CustomJobModel
    {
        public CustomJobModel(string name, string type, string trigger, string filePath)
        {
            JobName = name;
            ScriptType = type;
            TriggerString = trigger;
            ActionFilePath = filePath;
        }

        public Guid Id { get; set; }
        public string JobName { get; set; }
        public string ScriptType { get; set; }
        public string TriggerString { get; set; }
        public string ActionFilePath { get; set; }
    }
}
