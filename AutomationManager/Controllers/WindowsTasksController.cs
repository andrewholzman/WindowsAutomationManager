using AutomationManager.Data;
using AutomationManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM_TaskScheduler;
using Task = Microsoft.Win32.TaskScheduler.Task;
using System.IO;

namespace AutomationManager.Controllers
{
    public class WindowsTasksController : Controller
    {
        private readonly DatabaseContext _context;

        public WindowsTasksController(DatabaseContext context)
        {
            _context = context;
        }

        private string localIP = Utils.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet).FirstOrDefault();
        private string _folderPath = "\\WAM\\Uploads\\";
        


        // GET: WindowsTasks from system task registration, not database
        public async Task<IActionResult> Index()
        {
            //todo: make this async
            AM_TaskScheduler.TaskSchedulerManager tm = new AM_TaskScheduler.TaskSchedulerManager();
            List<Task> tasks = tm.GetTasks();
            List<WindowsTasks> windowsTasks = new List<WindowsTasks>();
            foreach (var t in tasks)
            {
                WindowsTasks task = new WindowsTasks();
                task.Id = Guid.NewGuid(); //need to map this to something else from the task. look into docs
                task.Name = t.Name;
                task.Description = t.Definition.RegistrationInfo.Description;
                var triggerCount = t.Definition.Triggers.Count;
                if (triggerCount > 0)
                {
                    task.TriggerType = t.Definition.Triggers.First().TriggerType.ToString();
                    task.TriggerString = t.Definition.Triggers.First().Repetition.ToString(); //need to figure out what the trigger's schedule looks like and put it in here 
                    task.TriggerAction = t.Definition.Actions.First().ActionType.ToString();
                }
                
                task.LastRun = t.LastRunTime;
                task.CreatedByUser = t.Definition.RegistrationInfo.Author;
                windowsTasks.Add(task);
            }

            // return View(await _context.WindowsTasks.ToListAsync());
            return View(windowsTasks);

        }

        // GET: WindowsTasks/Details/5
        public async Task<IActionResult> Details(string id, bool substring)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                TaskSchedulerManager tsm = new TaskSchedulerManager();
                var t = tsm.GetTask(id,substring);
                WindowsTasks task = new WindowsTasks();
                task.Id = Guid.NewGuid(); //need to map this to something else from the task. look into docs
                task.Name = t.Name;
                task.Description = t.Definition.RegistrationInfo.Description;
                var triggerCount = t.Definition.Triggers.Count;
                if (triggerCount > 0)
                {
                    task.TriggerType = t.Definition.Triggers.First().TriggerType.ToString();
                    //task.TriggerString = t.Definition.Triggers.First().Repetition.ToString(); //need to figure out what the trigger's schedule looks like and put it in here 
                    task.TriggerString = t.Definition.Triggers.First().ToString();
                    task.TriggerAction = t.Definition.Actions.First().ActionType.ToString();
                    task.ActionFilePath = t.Definition.Actions.First().ToString();
                }
                task.LastRun = t.LastRunTime;
                task.CreatedByUser = t.Definition.RegistrationInfo.Author;
                return View(task);
            } catch (Exception ex)
            {
                throw new Exception($"Failed to retreive task: {id}. Error: {ex.Message}");
            }

            
        }

        // GET: WindowsTasks/Create
        public IActionResult Create()
        {
            WindowsTasks task = new WindowsTasks();

            return View(task);
        }

        // POST: WindowsTasks/Create but not to database
        public IActionResult CreateTask(WindowsTasks task)
        {
            if (task.ActionFile.Length > 0)
            {
                // string filePath = $@"\\{localIP}\c$" + _folderPath;
                string filePath = "C:\\WAM\\Uploads\\"+task.ActionFile.FileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    task.ActionFile.CopyToAsync(stream);
                    task.ActionFilePath = $@"\\{localIP}"+_folderPath + task.ActionFile.FileName;
                       

                }
            }
            //string server, string username, string domain, string password, string folder, string description, string cronString
            AM_TaskScheduler.TaskSchedulerManager tm = new AM_TaskScheduler.TaskSchedulerManager();
            WindowsTask mTasks = new WindowsTask(task);

            tm.CreateTask(mTasks);

            
            return RedirectToAction("Index");
        }

        // POST: WindowsTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,TriggerType,TriggerString,TriggerAction,DateCreated,LastRun,CreatedByUser")] WindowsTasks windowsTasks)
        {
            if (ModelState.IsValid)
            {
                windowsTasks.Id = Guid.NewGuid();
                _context.Add(windowsTasks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(windowsTasks);
        }

        // GET: WindowsTasks/Edit/5
        public async Task<IActionResult> Edit(string id, bool substring)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                TaskSchedulerManager tsm = new TaskSchedulerManager();
                var t = tsm.GetTask(id, substring);
                WindowsTasks task = new WindowsTasks();
                task.Id = Guid.NewGuid(); //need to map this to something else from the task. look into docs
                task.Name = t.Name;
                task.Description = t.Definition.RegistrationInfo.Description;
                var triggerCount = t.Definition.Triggers.Count;
                if (triggerCount > 0)
                {
                    task.TriggerType = t.Definition.Triggers.First().TriggerType.ToString();
                    //task.TriggerString = t.Definition.Triggers.First().Repetition.ToString(); //need to figure out what the trigger's schedule looks like and put it in here 
                    task.TriggerString = t.Definition.Triggers.First().ToString();
                    task.TriggerAction = t.Definition.Actions.First().ActionType.ToString();
                    task.ActionFilePath = t.Definition.Actions.First().ToString();
                }
                task.LastRun = t.LastRunTime;
                task.CreatedByUser = t.Definition.RegistrationInfo.Author;
                return View(task);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retreive task: {id}. Error: {ex.Message}");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(WindowsTasks task)
        {
            if (task.ActionFile.Length > 0)
            {
                // string filePath = $@"\\{localIP}\c$" + _folderPath;
                string filePath = "C:\\WAM\\Uploads\\" + task.ActionFile.FileName;
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    task.ActionFile.CopyToAsync(stream);
                    task.ActionFilePath = $@"\\{localIP}" + _folderPath + task.ActionFile.FileName;


                }
            }
            //string server, string username, string domain, string password, string folder, string description, string cronString
            AM_TaskScheduler.TaskSchedulerManager tm = new AM_TaskScheduler.TaskSchedulerManager();
            WindowsTask mTasks = new WindowsTask(task);

            tm.UpdateTask(mTasks);         
            return RedirectToAction("Index");
        }
            // GET: WindowsTasks/Delete/5
            public async Task<IActionResult> Delete(string taskName)
        {
            if (taskName == null)
            {
                return NotFound();
            }

            //todo: make this async
            AM_TaskScheduler.TaskSchedulerManager tm = new AM_TaskScheduler.TaskSchedulerManager();
            tm.DeleteTask(taskName);
            return RedirectToAction(nameof(Index));
        }

        // POST: WindowsTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var windowsTasks = await _context.WindowsTasks.FindAsync(id);
            _context.WindowsTasks.Remove(windowsTasks);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult TriggerTask(string id, bool substring)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                //string server, string username, string domain, string password, string folder, string description, string cronString
                AM_TaskScheduler.TaskSchedulerManager tm = new AM_TaskScheduler.TaskSchedulerManager();
                tm.TriggerTask(id,substring);
            }
            catch (Exception eX)
            {
                throw new Exception($"Failed to trigger Task {id}. Error: {eX.Message}");
            }
            return RedirectToAction("Details", "WindowsTasks", new { id = id });
        }

        private bool WindowsTasksExists(Guid id)
        {
            return _context.WindowsTasks.Any(e => e.Id == id);
        }
    }
}
