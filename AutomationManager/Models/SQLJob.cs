using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutomationManager.Models
{
    public class SQLJob
    {
        //for ref: https://docs.microsoft.com/en-us/dotnet/api/microsoft.sqlserver.management.smo.agent.job?view=sqlserver-2016
        [Key]
        [Required]
        public Guid JobID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string OriginatingServer { get; set; }

        [NotMapped]
        public object Schedule { get; set; }
        [NotMapped]
        public object Steps { get; set; }
        [NotMapped]
        public object CurrentRunStatus { get; set; }
        [NotMapped]
        public object CurrentRunStep { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime LastRunDate { get; set; }
        public DateTime NextRunDate { get; set; }


         
    }
}
