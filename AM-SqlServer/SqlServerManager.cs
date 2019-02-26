using Microsoft.SqlServer.Management.Common;
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Agent;
using System.Reflection;
using AM_SqlServer.Models;

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
        private List<WAMSqlConnection> _connections;

        public SqlServerManager(string server, string username, string password, List<WAMSqlConnection> conns)
        {
            _server = server;
            _username = username;
            _password = password;
            _connections = conns;
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

        public void CreateJob(WAMSQLJob wamJob, string server = "localhost")
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
                    Operator op = new Operator(srv.JobServer, "WAM_Operator");
                    op.NetSendAddress = "WAM_Server";
                    op.Create();
                    Job job = new Job(srv.JobServer, wamJob.Name);
                    job.Description = wamJob.Description;
                    job.Create();
                    
                    JobStep jobStep = new JobStep(job, wamJob.JobStepName);
                    jobStep.Command = wamJob.JobStepCommand;
                    jobStep.OnSuccessAction = StepCompletionAction.QuitWithSuccess;
                    jobStep.OnFailAction = StepCompletionAction.QuitWithFailure;
                    jobStep.Create();
                    

                    JobSchedule jobSched = new JobSchedule(job, wamJob.ScheduleName);
                    jobSched.FrequencyTypes = wamJob.ScheduleFrequencyType;
                    jobSched.FrequencyRecurrenceFactor = wamJob.ScheduleFrequencyRecurrenceFactor;
                    jobSched.FrequencyInterval = wamJob.ScheduleFrequencyInterval;
                    jobSched.ActiveStartTimeOfDay = wamJob.ScheduleActiveStartTimeOfDay;
                    jobSched.Create();


                } catch (Exception ex)
                {
                    throw new Exception("Failed To Create Job: " + wamJob.Name + " - " + ex.Message);
                }
                
            } else
            {
                throw new Exception("Failed To Connect to SQL Job Server");
            }
        }
        public void CreateJob(WAMSQLJob wamJob, string username, string password, string server = "localhost")
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
                    Operator op = new Operator(srv.JobServer, "WAM_Operator");
                    op.NetSendAddress = "WAM_Server";
                    op.Create();

                    Job job = new Job(srv.JobServer, wamJob.Name);
                    job.Description = wamJob.Description;
                    job.Create();
                    JobStep jobStep = new JobStep(job, wamJob.JobStepName);
                    jobStep.Command = wamJob.JobStepCommand;
                    jobStep.OnFailAction = StepCompletionAction.QuitWithFailure;
                    jobStep.Create();
                    

                    JobSchedule jobSched = new JobSchedule(job, wamJob.ScheduleName);
                    jobSched.FrequencyTypes = wamJob.ScheduleFrequencyType;
                    jobSched.FrequencyRecurrenceFactor = wamJob.ScheduleFrequencyRecurrenceFactor;
                    jobSched.FrequencyInterval = wamJob.ScheduleFrequencyInterval;
                    jobSched.ActiveStartTimeOfDay = wamJob.ScheduleActiveStartTimeOfDay;
                    jobSched.Create();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed To Create Job: " + wamJob.Name + " - " + ex.Message);
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
                        cleanJob.Alter(); //update the instance of the job on the jobServer
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
                        cleanJob.Alter(); //update the instance of the job on the jobServer
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

        /// <summary>
        /// Loop through the properties of the clean job (pulled from the JobServer)
        /// and if they differ from the dirty job, update the cleanjob with the dirty job's values
        /// </summary>
        /// <param name="cleanJob">clean job pulled from JobServer</param>
        /// <param name="dirtyJob">Edited job sent from web site</param>
        private void UpdateJob(Job cleanJob, Job dirtyJob)
        {
            foreach (PropertyInfo propInfo in cleanJob.GetType().GetProperties())
            {
                if (propInfo.CanRead)
                {
                    object cleanValue = propInfo.GetValue(cleanJob, null);
                    object dirtyValue = propInfo.GetValue(dirtyJob, null);
                    if (!Equals(cleanValue,dirtyValue))
                    {
                        propInfo.SetValue(cleanJob, dirtyValue);
                    }
                }
            }
        }
    }
}
