using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using Microsoft.Win32.TaskScheduler;

namespace AM_TaskScheduler
{
    public class TaskSchedulerManager
    {
        private static string TaskPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System),@"Tasks\AutomationManager\");

        public List<Task> GetTasks()
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    Predicate<Task> filter = FindNonSystemTasks;
                    IEnumerable<Task> tasks = ts.FindAllTasks(filter); //using the predicate FindNonSystemTasks to exclude any task that is under the Microsoft folder
                    return tasks.ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve tasks - " + ex.Message);
            }

        }

        public Task GetTask(string taskName, bool substring)
        {
            if (substring) { taskName = taskName + "*"; }
            try
            {
                using (TaskService ts = new TaskService())
                {
                    return ts.FindTask(taskName, true);


                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve task: " + taskName + " on Local Server - " + ex.Message);
            }
        }

        public Task GetTask(string taskName, bool substring, string server, string username, string domain, string password)
        {
            try
            {
                using (TaskService ts = new TaskService(server,username,domain,password))
                {
                    return ts.FindTask(taskName, true);

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve task: " + taskName + " on Server: " + server + " - " + ex.Message);
            }
        }

        public void CreateExecTaskRemote(string server, string username, string domain, string password, string folder, string description, string cronString)
        {
            try
            {
                using (TaskService ts = new TaskService(server, username, domain, password))
                {
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = description;

                    
                    Trigger[] trigger = Trigger.FromCronFormat(cronString);


                    td.Triggers.Add(trigger.First());
                    td.Actions.Add(new ExecAction("notepad.exe", "C:\\temp\\temp.log", null));
                    ts.RootFolder.RegisterTaskDefinition(folder, td);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create task on server: " + server + " - " + ex.Message);
            }
        }

        public void CreateTask(WindowsTask task)
        {
            try
            {
                var securityIdentifier = WindowsIdentity.GetCurrent().User;
                if (securityIdentifier != null)
                {
                    var sid = securityIdentifier.Value;
                }
                var userId = WindowsIdentity.GetCurrent().Name;

                //using (TaskService ts = new TaskService())
                //{
                //    TaskDefinition td = ts.NewTask();
                //    td.RegistrationInfo.Description = description;
                //    td.Principal.RunLevel = TaskRunLevel.Highest;
                //    td.Actions.Add(new ExecAction("notepad.exe", @"C:\temp\temp.log", null));
                //    td.Triggers.Add(new DailyTrigger() { StartBoundary = DateTime.Today.AddHours(19) });
                //    td.Triggers.Add(Trigger.FromCronFormat(cronString).First());
                //    ts.RootFolder.RegisterTaskDefinition(@"Test", td);
                //}
                using (TaskService ts = new TaskService())
                {
                    TaskDefinition td = ts.NewTask();
                    td.RegistrationInfo.Description = task.Description;

 
                    switch (task.TriggerType)
                    {
                        case "Custom":
                            Trigger[] trigger = Trigger.FromCronFormat(task.TriggerString);
                            td.Triggers.Add(trigger.First());
                            break;
                        case "Logon":
                            td.Triggers.Add(new LogonTrigger());
                            break;
                        case "Startup":
                            td.Triggers.Add(new BootTrigger());
                            break;
                    }

                    
                    td.Actions.Add(new ExecAction(task.ActionFilePath, null));
                    td.Principal.UserId = @"NT AUTHORITY\NETWORKSERVICE"; /*System.Security.Principal.WindowsIdentity.GetCurrent().Name;*/
                    td.Principal.LogonType = TaskLogonType.ServiceAccount;
                    string taskDef = task.Description;
                    TaskService.Instance.RootFolder.RegisterTaskDefinition(taskDef, td);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create task on local host" + ex.Message);
            }
        }

        public void DeleteTask(string taskName)
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    TaskFolder folder = ts.FindTask(taskName).Folder;
                    folder.DeleteTask(taskName);
                }
            } catch (Exception ex)
            {
                throw new Exception("Failed to remove task" + taskName + " On Local Server. " + ex.Message);
                     
            }

        }

        public void DeleteTask(string taskName, string server, string username, string domain, string password)
        {
            try
            {
                using (TaskService ts = new TaskService(server, username, domain, password))
                {
                    TaskFolder folder = ts.FindTask(taskName).Folder;
                    folder.DeleteTask(taskName);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to remove task: " + taskName + " on Server: " + server + " - " + ex.Message);
            }
        }

        public void TriggerTask(string taskName, bool substring)
        {
            if (substring) { taskName = "*" + taskName + "*"; }
            try
            {
                using (TaskService ts = new TaskService())
                {
                    Task task = ts.FindTask(taskName, true);
                    var runTask = task.Run();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to trigger task: " + taskName + " on Local Server - " + ex.Message);
            }
        }

        public void TriggerTask(string taskName, bool substring, string server, string username, string domain, string password)
        {
            try
            {
                using (TaskService ts = new TaskService(server, username, domain, password))
                {
                    Task task = ts.FindTask(taskName);
                    var runTask = task.Run();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to trigger task: " + taskName + " on Server: " + server + " - " + ex.Message);
            }
        }

        /**
         * Predicate used in the retrieval of all tasks to remove microsft System tasks windows has preloaded
         */
        private static bool FindNonSystemTasks(Task task)
        {
            if (task.Folder.ToString().Contains("Microsoft"))
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
