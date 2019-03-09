using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [NotMapped]
        public IFormFile ActionFile { get; set; }
        public string ActionFilePath { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastRun { get; set; }
        public string CreatedByUser { get; set; }

        [NotMapped]
        public List<SelectListItem> TriggerTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "Custom", Text = "Custom" },
             new SelectListItem { Value = "Logon", Text = "Logon" },
              new SelectListItem { Value = "Startup", Text = "Startup" }
        };
    }
}
