using AM_CustomJobs.Models;
using Hangfire;
using Hangfire.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AM_CustomJobs
{
    /// <summary>
    /// Wrapper for logic to implement custom jobs - use HangFire to persist jobs and allow for different job types
    /// Implement script editor in GUI and pass here, save it as a vbs/bat and then run through HangFire
    /// </summary>
    public class CustomJobManager
    {
        private readonly string _connStr = "@Server=localhost;database=WAM; Integrated Security=SSPI";
        public BackgroundJobServer _jobServer;

        public CustomJobManager()
        {
            var options = new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = true
            };

            GlobalConfiguration.Configuration.UseSqlServerStorage(_connStr, options);
        }

        public void Start()
        { 
            _jobServer = new BackgroundJobServer();
        }

        public void Stop()
        {
            _jobServer.Dispose();
        }

        private SqlCommand GetDbcommand()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _connStr;
            return conn.CreateCommand();
        }

        public void CreateJob()
        {

        }

        public List<WAMCustomJob> GetJobs()
        {
            throw new NotImplementedException();
        }

        public WAMCustomJob GetJob(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
