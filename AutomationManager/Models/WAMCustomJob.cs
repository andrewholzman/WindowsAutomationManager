using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomationManager.Models
{
    public class WAMCustomJob
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string ScriptType { get; set; }
        public string TriggerString { get; set; }
        [NotMapped]
        public IFormFile ActionFile { get; set; }
        public string ActionFilePath { get; set; }
    }
}
