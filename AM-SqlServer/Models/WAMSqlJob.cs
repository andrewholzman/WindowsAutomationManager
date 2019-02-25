using Microsoft.SqlServer.Management.Smo.Agent;
using System;
using System.Collections.Generic;
using System.Text;

namespace AM_SqlServer.Models
{
    public class WAMSQLJob
    {
        //for ref: https://docs.microsoft.com/en-us/dotnet/api/microsoft.sqlserver.management.smo.agent.job?view=sqlserver-2016
        public Guid JobID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OriginatingServer { get; set; }
        // object set to weekly, recurrence factor evrey 1 week, and the interval set to integer as follows:
        //1 = Sunday, 2 = Monday, 4 = Tuesady, 8 = Wednesday, 16 = Thursday, 32 = friday, 64 = Saturday
        public JobSchedule Schedule { get; set; }
        
        //objects created to execute whatever query they type in
        public JobStepCollection Steps { get; set; }
        public object CurrentRunStatus { get; set; }
        public object CurrentRunStep { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime LastRunDate { get; set; }
        public DateTime NextRunDate { get; set; }



    }
}
