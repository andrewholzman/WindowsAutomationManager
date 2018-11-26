using Microsoft.SqlServer.Management.Common;
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Reflection;

namespace AM_SqlServer
{
    /// <summary>
    /// Wrapper around SQL Sever Agent / SSMS / SSRS / SSIS jobs
    /// </summary>
    public class SqlServerManager
    {
        private static string _server;
        private static string _username;
        private static string _password;

        public SqlServerManager(string server, string username, string password)
        {
            _server = server;
            _username = username;
            _password = password;
        }
        public SqlServerManager() { } //empty constructor to allow manual creation for now

        public Server ConnectToRemoteDatabase()
        {
            ServerConnection srvConn = new ServerConnection(_server);
            srvConn.LoginSecure = false;
            srvConn.Login = _username;
            srvConn.Password = _password;
            Server srv = new Server(srvConn);
            srv.ConnectionContext.Connect();
            if (srv.ConnectionContext.IsOpen)
            {
                return srv;
            }
            else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }
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
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }

        public Server ConnectToLocalDatabase()
        {
            Server srv = new Server();
            srv.ConnectionContext.LoginSecure = false; //set true for windows auth?
            srv.ConnectionContext.Login = _username;
            srv.ConnectionContext.Password = _password;
            srv.ConnectionContext.Connect();
            if (srv.ConnectionContext.IsOpen)
            {
                return srv;
            }
            else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
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
                throw new Exception("Failed To Connect to SQL Job Server");
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

        public List<Job> GetAllSqlAgentJobs(string server = "localhost")
        {
            Server srv = null;
            if (server == "localhost")
            {
                srv = ConnectToLocalDatabase(_username, _password);
            }
            else
            {
                srv = ConnectToRemoteDatabase(server, _username, _password);
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
                throw new Exception("Failed To Connect to SQL Job Server");
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
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }

        public Job FindJobByID(Guid jobId, string server = "localhost")
        {
            Server srv = null;
            if (server == "localhost")
            {
                srv = ConnectToLocalDatabase(_username, _password);
            }
            else
            {
                srv = ConnectToRemoteDatabase(server, _username, _password);
            }

            if (srv != null)
            {
                Job j = srv.JobServer.GetJobByID(jobId);
                return j;
            }
            else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
            }
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
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }

        public void CreateJob(Job job, string server = "localhost")
        {
            Server srv = null;
            if (server == "localhost")
            {
                srv = ConnectToLocalDatabase(_username, _password);
            }
            else
            {
                srv = ConnectToRemoteDatabase(server, _username, _password);
            }

            if (srv != null)
            {
                try
                {
                    srv.JobServer.Jobs.Add(job);
                } catch (Exception ex)
                {
                    throw new Exception("Failed To Create Job: " + job.Name + " - " + ex.Message);
                }
                
            } else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }
        public void CreateJob(Job job, string username, string password, string server = "localhost")
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
                try
                {
                    srv.JobServer.Jobs.Add(job);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed To Create Job: " + job.Name + " - " + ex.Message);
                }

            }
            else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }

        public void DeleteJob(Guid jobId, string server = "localhost")
        {
            Server srv = null;
            if (server == "localhost")
            {
                srv = ConnectToLocalDatabase(_username, _password);
            }
            else
            {
                srv = ConnectToRemoteDatabase(server, _username, _password);
            }

            if (srv != null)
            {
                try
                {
                    srv.JobServer.DropJobByID(jobId);
                } catch (Exception ex)
                {
                    throw new Exception("Failed To Drop Job: " + jobId.ToString() +" - " + ex.Message);
                }
                
            } else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }
        public void DeleteJob(Guid jobId, string username, string password, string server = "localhost")
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
                try
                {
                    srv.JobServer.DropJobByID(jobId);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed To Drop Job: " + jobId.ToString() + " - " + ex.Message);
                }

            }
            else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }

        public Job UpdateJobById(Job dirtyJob, string server = "localhost")
        {
            Server srv = null;
            if (server == "localhost")
            {
                srv = ConnectToLocalDatabase(_username, _password);
            }
            else
            {
                srv = ConnectToRemoteDatabase(server, _username, _password);
            }

            if (srv != null)
            {
                try
                {
                    Job cleanJob = srv.JobServer.GetJobByID(dirtyJob.JobID);
                    if (cleanJob != dirtyJob)
                    {
                        UpdateJob(cleanJob, dirtyJob);
                    }
                    return cleanJob;
                } catch (Exception ex)
                {
                    throw new Exception("Failed To Update Job - " + ex.Message);
                }
                
            } else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }
        public Job UpdateJobById(Job dirtyJob, string username, string password, string server = "localhost")
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
                try
                {
                    Job cleanJob = srv.JobServer.GetJobByID(dirtyJob.JobID);
                    if (cleanJob != dirtyJob)
                    {
                        UpdateJob(cleanJob, dirtyJob);
                    }
                    return cleanJob;
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed To Update Job - " + ex.Message);
                }

            }
            else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }

        private void UpdateJob(Job cleanJob, Job dirtyJob)
        {
            foreach (PropertyInfo prop in cleanJob.GetType().GetProperties())
            {
                PropertyInfo dirtyProp = dirtyJob.GetType().GetProperty(prop.Name);
                if (dirtyProp != prop)
                {
                    cleanJob.GetType().GetProperty(prop.Name).SetValue(cleanJob.GetType().GetProperty(prop.Name), dirtyProp);
                }
            }
        }
    }
}
