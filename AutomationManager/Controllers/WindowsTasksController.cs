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

namespace AutomationManager
{
    public class WindowsTasksController : Controller
    {
        private readonly DatabaseContext _context;

        public WindowsTasksController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: WindowsTasks from system task registration, not database
        public async Task<IActionResult> Index()
        {
            //todo: make this async
            AM_TaskScheduler.TaskScheduler tm = new AM_TaskScheduler.TaskScheduler();
            List<Task> tasks = tm.GetTasks();
            List<WindowsTasks> windowsTasks = new List<WindowsTasks>();
            foreach (var t in tasks)
            {
                WindowsTasks task = new WindowsTasks();
                task.Id = Guid.NewGuid(); //need to map this to something else from the task. look into docs
                task.Description = t.Definition.RegistrationInfo.Description;
                var asdf = t.Definition.Triggers.Count;
                if (asdf > 0)
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
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var windowsTasks = await _context.WindowsTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (windowsTasks == null)
            {
                return NotFound();
            }

            return View(windowsTasks);
        }

        // GET: WindowsTasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WindowsTasks/Create but not to database
        public IActionResult CreateTask(WindowsTasks task)
        {
            if (ModelState.IsValid)
            {
                //string server, string username, string domain, string password, string folder, string description, string cronString
                AM_TaskScheduler.TaskScheduler tm = new AM_TaskScheduler.TaskScheduler();
                tm.CreateTask("",task.Description,task.TriggerString);

            }
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
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var windowsTasks = await _context.WindowsTasks.FindAsync(id);
            if (windowsTasks == null)
            {
                return NotFound();
            }
            return View(windowsTasks);
        }

        // POST: WindowsTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Description,TriggerType,TriggerString,TriggerAction,DateCreated,LastRun,CreatedByUser")] WindowsTasks windowsTasks)
        {
            if (id != windowsTasks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(windowsTasks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WindowsTasksExists(windowsTasks.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(windowsTasks);
        }

        // GET: WindowsTasks/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var windowsTasks = await _context.WindowsTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (windowsTasks == null)
            {
                return NotFound();
            }

            return View(windowsTasks);
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

        private bool WindowsTasksExists(Guid id)
        {
            return _context.WindowsTasks.Any(e => e.Id == id);
        }
    }
}
