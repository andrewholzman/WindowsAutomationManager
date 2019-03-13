using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomationManager.Models
{
    public class WAMCustomJob
    {
        public WAMCustomJob()
        {

        }

        public WAMCustomJob(string name, string type, string trigger, string lastRet, string filePath)
        {
            JobName = name;
            ScriptType = type;
            TriggerString = trigger;
            LastResult = lastRet;
            ActionFilePath = filePath;
        }
        public WAMCustomJob(string name, string type, string trigger, string filePath)
        {
            JobName = name;
            ScriptType = type;
            TriggerString = trigger;
            ActionFilePath = filePath;
        }
        [Key]
        [Required]
        public Guid Id { get; set; }
        [DisplayName("Job Name")]
        public string JobName { get; set; }
        [DisplayName("Script Type")]
        public string ScriptType { get; set; }
        [DisplayName("Trigger String")]
        public string TriggerString { get; set; }

        [NotMapped]
        [DisplayName("Action File")]
        public IFormFile ActionFile { get; set; }
        [DisplayName("Action File")]
        public string ActionFilePath { get; set; }
        [DisplayName("Last Result")]
        public string LastResult { get; set; }
    }
}
