using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutomationManager.Models
{
    public class WindowsTasks
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string TriggerType { get; set; }
        public string TriggerString { get; set; }
        public string TriggerAction { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastRun { get; set; }
        public string CreatedByUser { get; set; }
    }
}
