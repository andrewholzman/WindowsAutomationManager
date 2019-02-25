using System.Collections.Generic;
using AM_SqlServer;
using AM_SqlServer.Models;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomationManagerTests
{
    [TestClass]
    public class SqlServerIntegrationTests
    {
        SqlServerManager _ssm = new SqlServerManager();
        [TestMethod]
        public void LocalSqlServerConnectionSqlAuthShouldSucceed()
        {
            Server srv = _ssm.ConnectToLocalDatabase("sa", "password");
            Assert.IsNotNull(srv);
        }

        [TestMethod]
        public void LocalSqlServerConnectionShouldReturnJobs()
        {
            List<Job> jobs = _ssm.GetAllSqlAgentJobs("sa", "password");
            Assert.IsTrue(jobs.Count>0);
        }

        [TestMethod]
        public void RemoteSqlServerConnectionSqlAuthShouldSucceed()
        {
            Server srv = _ssm.ConnectToRemoteDatabase("SERVER02", "sa", "password");
            Assert.IsNotNull(srv);
        }

        [TestMethod]
        public void RemoteSqlServerConnectionShouldReturnJobs()
        {
            List<Job> jobs = _ssm.GetAllSqlAgentJobs("sa", "password", "SERVER02");
            Assert.IsTrue(jobs.Count>0);
        }

        [TestMethod]
        public void JobShouldBeCreated()
        {
            WAMSQLJob job = new WAMSQLJob();
            job.JobID = System.Guid.NewGuid();
            job.Name = "test job";
            job.Description = "Unit Test Job";
            job.OriginatingServer = "SERVER02";
            job.Schedule = new JobSchedule();
            job.Schedule.Name = "Unit Test Job Schedule";
            
            
        }
    }
}
