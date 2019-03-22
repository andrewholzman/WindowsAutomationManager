using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Trigger Type")]
        public string TriggerType { get; set; }
        [DisplayName("Trigger String")]
        public string TriggerString { get; set; }
        [DisplayName("Trigger Action")]
        public string TriggerAction { get; set; }
        [NotMapped]
        [DisplayName("Action File")]
        public IFormFile ActionFile { get; set; }
        [DisplayName("Action File Path")]
        public string ActionFilePath { get; set; }
        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }
        [DisplayName("Last Run")]
        public DateTime LastRun { get; set; }
        [DisplayName("Created By")]
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
