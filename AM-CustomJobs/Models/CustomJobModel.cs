using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AM_CustomJobs.Models
{
    public class CustomJobModel
    {
        public CustomJobModel(string name, string type, string trigger, string lastResult,string filePath)
        {
            JobName = name;
            ScriptType = type;
            TriggerString = trigger;
            LastResult = lastResult;
            ActionFilePath = filePath;
        }
        public string JobName { get; set; }
        public string ScriptType { get; set; }
        public string TriggerString { get; set; }
        public string LastResult { get; set; }
        public string ActionFilePath { get; set; }
    }
}
