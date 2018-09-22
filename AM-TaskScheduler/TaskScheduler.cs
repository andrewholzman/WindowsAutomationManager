using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32.TaskScheduler;

namespace AM_TaskScheduler
{
    public class TaskScheduler
    {
        public List<Task> GetTasks()
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    IEnumerable<Task> tasks = ts.RootFolder.AllTasks;
                    return tasks.ToList();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve tasks - " + ex.Message);
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

        public void CreateTask(string folder, string description, string cronString)
        {
            try
            {
                using (TaskService ts = new TaskService())
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
                throw new Exception("Failed to create task on local host" + ex.Message);
            }
        }
    }
}
