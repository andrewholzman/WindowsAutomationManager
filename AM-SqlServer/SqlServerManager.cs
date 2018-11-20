using Microsoft.SqlServer.Management.Common;
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;


namespace AM_SqlServer
{
    /// <summary>
    /// Wrapper around SQL Sever Agent / SSMS / SSRS / SSIS jobs
    /// </summary>
    public class SqlServerManager
    {
        public Server ConnectToRemoteDatabase(string server, string username, string password)
        {
            ServerConnection srvConn = new ServerConnection(server);
            srvConn.LoginSecure = false;
            srvConn.Login = username;
            srvConn.Password = password;
            Server srv = new Server(srvConn);
            srv.ConnectionContext.Connect();
            if (srv.ConnectionContext.IsOpen)
            {
                return srv;
            }
            else
            {
                throw new Exception("Failed to connect to local SQL server");
            }
        }

       

        public Server ConnectToLocalDatabase(string username, string password)
        {
            Server srv = new Server();
            srv.ConnectionContext.LoginSecure = false; //set true for windows auth?
            srv.ConnectionContext.Login = username;
            srv.ConnectionContext.Password = password;
            srv.ConnectionContext.Connect();
            if (srv.ConnectionContext.IsOpen)
            {
                return srv;
            }
            else
            {
                throw new Exception("Failed to connect to local SQL server");
            }
        }

        public bool DisconnectFromRemoteDatabase(Server srv)
        {
            if (srv.ConnectionContext.IsOpen)
            {
                srv.ConnectionContext.Disconnect();
            }

            if (srv.ConnectionContext.IsOpen)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Job> GetAllSqlAgentJobs(string username, string password, string server = "localhost")
        {
            Server srv = null;
            if (server == "localhost")
            {
                srv = ConnectToLocalDatabase(username, password);
            }
            else
            {
                srv = ConnectToRemoteDatabase(server, username, password);
            }

            if (srv != null)
            {
                List<Job> jobs = new List<Job>();
                foreach (Job job in srv.JobServer.Jobs)
                {
                    jobs.Add(job);
                }

                return jobs;
            }
            else
            {
                throw new Exception("Srv is null make me a good exception do something else");
            }
        }


        public void CreateJob(Job job)
        {
            throw new NotImplementedException();
        }

        public Job FindJobByID(Guid jobId, string username, string password, string server = "localhost")
        {
        
            Server srv = null;
            if (server == "localhost")
            {
                srv = ConnectToLocalDatabase(username, password);
            }
            else
            {
                srv = ConnectToRemoteDatabase(server, username, password);
            }

            if (srv != null)
            {
                Job j = srv.JobServer.GetJobByID(jobId);
                return j;
            }
            else
            {
                throw new Exception("bad server");
            }
        }
    }
}
