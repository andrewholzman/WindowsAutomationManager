using AM_CustomJobs.Models;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace AM_CustomJobs
{
    /// <summary>
    /// Wrapper for logic to implement custom jobs - use HangFire to persist jobs and allow for different job types
    /// Implement script editor in GUI and pass here, save it as a vbs/bat and then run through HangFire
    /// </summary>
    public class CustomJobManager
    {
        private readonly string _connStr = "Server=localhost;database=WAM; Integrated Security=SSPI";
        public BackgroundJobServer _jobServer;
        public RecurringJobManager _manager;
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
            _manager = new RecurringJobManager();
        }

        public void Stop()
        {
            _jobServer.Dispose();
        }

        public List<CustomJobModel> GetJobs()
        {
            try
            {
                var conn = Hangfire.JobStorage.Current.GetConnection();
                var jobs = StorageConnectionExtensions.GetRecurringJobs(conn);
                //cast jobs to WAMCustomJob type
                List<CustomJobModel> retJobs = new List<CustomJobModel>();
                foreach(var job in jobs)
                {
                    string x = job.Id;
                    
                }
            } catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve jobs. Error: {ex.Message}");
            }
            throw new NotImplementedException();
        }

        public CustomJobModel GetJob(Guid id)
        {
            throw new NotImplementedException();
        }

        public void CreateOrUpdateJob(string id, string scriptType, string actionFilePath, string triggerString)
        {
            try
            {
                switch (scriptType)
                {
                    case "BAT":
                        RecurringJob.AddOrUpdate(id, () => PerformBatchScript(id, actionFilePath), triggerString);
                        break;
                    case "VBS":
                        RecurringJob.AddOrUpdate(id, () => PerformVBScript(id, actionFilePath), triggerString);
                        break;
                    case "PS":
                        RecurringJob.AddOrUpdate(id, () => PerformPowershellScript(id, actionFilePath), triggerString);
                        break;
                }
            } catch (Exception ex)
            {
                throw new Exception($"Failed to create {scriptType} job. Error: {ex.Message}");
            }

        }


        public void RemoveJob(string id)
        {
            try
            {
                RecurringJob.RemoveIfExists(id);
            } catch (Exception ex)
            {
                throw new Exception($"Failed to remove Custom Job {id}. Error: {ex.Message}");
            }

        }

        private void PerformBatchScript(string id, string actionFilePath)
        {
            try
            {
                ExecuteBatchCommand(actionFilePath);
                LogHistory(Guid.NewGuid(),id, "BAT", DateTime.Now, "Success");
            } catch (Exception ex)
            {
                LogHistory(Guid.NewGuid(), id, "BAT", DateTime.Now, "Failure",ex.Message);
                throw new Exception($"Failed to execute BAT script for job: ${id}. Error: {ex.Message}");
            }
        }


        private void PerformVBScript(string id, string actionFilePath)
        {
            try
            {
                ExecuteVBSCommand(actionFilePath);
                LogHistory(Guid.NewGuid(), id, "VBS", DateTime.Now, "Success");
            }
            catch (Exception ex)
            {
                LogHistory(Guid.NewGuid(), id, "VBS", DateTime.Now, "Failure", ex.Message);
                throw new Exception($"Failed to execute VB script for job: ${id}. Error: {ex.Message}");
            }
        }

        private void PerformPowershellScript(string id, string actionFilePath)
        {
            try
            {
                ExecutePowerhsellScript(actionFilePath);
                LogHistory(Guid.NewGuid(), id, "PS", DateTime.Now, "Success");
            }
            catch (Exception ex)
            {
                LogHistory(Guid.NewGuid(), id, "PS", DateTime.Now, "Failure", ex.Message);
                throw new Exception($"Failed to execute PowerShell script for job: ${id}. Error: {ex.Message}");
            }
        }


        private static void ExecuteBatchCommand(string filePath)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + filePath);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            Process process = Process.Start(processInfo);
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            process.Close();
        }

        private static void ExecuteVBSCommand(string filePath)
        {

            Process scriptProc = new Process();
            scriptProc.StartInfo.FileName = @"cscript";
            scriptProc.StartInfo.WorkingDirectory = Directory.GetParent(filePath).ToString();
            scriptProc.StartInfo.Arguments = "//B //Nologo " + filePath;
            scriptProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden; 
            scriptProc.Start();
            scriptProc.WaitForExit(); // maybe remove this?
            scriptProc.Close();
        }

        private static void ExecutePowerhsellScript(string filePath)
        {
            string scriptText = LoadScript(filePath);

            // create Powershell runspace
            Runspace runspace = RunspaceFactory.CreateRunspace();
            runspace.Open();
            Pipeline pipeline = runspace.CreatePipeline();
            pipeline.Commands.AddScript(scriptText);

            // execute the script
            Collection<PSObject> results = pipeline.Invoke();

            // close the runspace
            runspace.Close();   
        }

        private static string LoadScript(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    StringBuilder fileContents = new StringBuilder();
                    string curLine;
                    while ((curLine = sr.ReadLine()) != null)
                    {
                        fileContents.Append(curLine + "\n");
                    }
                    return fileContents.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse PowerShell Script: {fileName}. Error: {ex.Message}");
            }

        }

        private void LogHistory(Guid newGuid, string id, string jobType, DateTime timeStamp, string result, string error=null)
        {
            SqlCommand cmd = GetDbcommand();
            cmd.CommandText = @"INSERT INTO JobHistory VALUES (@Id, @JobName, @JobType, @TimeStamp, @Result, @Error)";
            cmd.Parameters.AddWithValue("@Id", newGuid);
            cmd.Parameters.AddWithValue("@JobName", id);
            cmd.Parameters.AddWithValue("@JobType", jobType);
            cmd.Parameters.AddWithValue("@TimeStamp", timeStamp);
            cmd.Parameters.AddWithValue("@Result", result);
            if (string.IsNullOrWhiteSpace(error))
            {
                cmd.Parameters.AddWithValue("@Error", DBNull.Value);
            } else
            {
                cmd.Parameters.AddWithValue("@Error", error);
            }

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            } catch (Exception ex)
            {
                throw new Exception($"Failed to write to JobHistory for Job: {id}. Error: {ex.Message}");
            }
        }


        private SqlCommand GetDbcommand()
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = _connStr;
            return conn.CreateCommand();
        }
    }
}
