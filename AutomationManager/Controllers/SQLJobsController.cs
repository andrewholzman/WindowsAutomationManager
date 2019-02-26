using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM_SqlServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SqlServer.Management.Smo.Agent;
using Microsoft.EntityFrameworkCore;
using AutomationManager.Data;
using AutomationManager.Models;

namespace AutomationManager.Controllers
{
    public class SQLJobsController : Controller
    {
        private readonly DatabaseContext _context;
        private AM_SqlServer.SqlServerManager _ssm;

        public SQLJobsController(DatabaseContext context)
        {
            _context = context;
            //TODO - pull from AppSettings. Construct a WAMSqlConnection object that contains the server username and password
            // and alter SSM constructor to take in list of WAMSqlConnection objects
            _ssm = new SqlServerManager("localhost","sa","password",null);
        }

        // GET: SQLJobs
        public async Task<IActionResult> Index()
        {
            //AM_SqlServer.SqlServerManager ssm = new AM_SqlServer.SqlServerManager();
            //TODO: use AppSettings to pull this, set value through a settings/config menu
            List<Job> localJobs = _ssm.GetAllSqlAgentJobs();
            List<SQLJob> sqlJobs = new List<SQLJob>();

            foreach (Job j in localJobs)
            {
                SQLJob job = new SQLJob();
                job.JobID = j.JobID;
                job.Name = j.Name;
                job.Description = j.Description;
                job.OriginatingServer = j.OriginatingServer;
                job.Schedule = j.JobSchedules;
                job.Steps = j.JobSteps;
                job.CurrentRunStatus = j.CurrentRunStatus;
                job.CurrentRunStep = j.CurrentRunStep;
                job.DateCreated = j.DateCreated;
                job.DateModified = j.DateLastModified;
                job.LastRunDate = j.LastRunDate;
                job.NextRunDate = j.NextRunDate;
                sqlJobs.Add(job);
            }

            //TODO remove this and have code loop through the list of WAMSqlConnectionObjects and pull jobs for each
            List<Job> remoteJobs = _ssm.GetAllSqlAgentJobs("sa", "password", "SERVER02");
            foreach (Job j in remoteJobs)
            {
                SQLJob job = new SQLJob();
                //TODO move this to a constructor
                job.JobID = j.JobID;
                job.Name = j.Name;
                job.Description = j.Description;
                job.OriginatingServer = j.OriginatingServer;
                job.Schedule = j.JobSchedules;
                job.Steps = j.JobSteps;
                job.CurrentRunStatus = j.CurrentRunStatus;
                job.CurrentRunStep = j.CurrentRunStep;
                job.DateCreated = j.DateCreated;
                job.DateModified = j.DateLastModified;
                job.LastRunDate = j.LastRunDate;
                job.NextRunDate = j.NextRunDate;
                sqlJobs.Add(job);
            }
            return View(sqlJobs);
           // return View(await _context.SQLJobs.ToListAsync());
        }

        // GET: SQLJobs/Details/5
        public async Task<IActionResult> Details(Guid id, [FromQuery]string server)
        {
            //SqlServerManager ssm = new SqlServerManager();
            var j = _ssm.FindJobByID(id,server);
            SQLJob job = new SQLJob();
            //TODO move this to a constructor
            job.JobID = j.JobID;
            job.Name = j.Name;
            job.Description = j.Description;
            job.OriginatingServer = j.OriginatingServer;
            job.Schedule = j.JobSchedules;
            job.Steps = j.JobSteps;
            job.CurrentRunStatus = j.CurrentRunStatus;
            job.CurrentRunStep = j.CurrentRunStep;
            job.DateCreated = j.DateCreated;
            job.DateModified = j.DateLastModified;
            job.LastRunDate = j.LastRunDate;
            job.NextRunDate = j.NextRunDate;
//            var sQLJob = await _context.SQLJobs
//                .FirstOrDefaultAsync(m => m.JobID == id);
//            if (sQLJob == null)
//            {
//                return NotFound();
//            }

            return View(job);
        }

        // GET: SQLJobs/Create
        public IActionResult Create()
        {
            CreateJobViewModel cjvm = new CreateJobViewModel();
            return View(cjvm);
        }

        // POST : SQLJobs/Create but no to entity db, create to SQL server
        public IActionResult CreateJob(CreateJobViewModel job)
        {
            if (ModelState.IsValid)
            {
                int frequency = 0; //days of week in sql agent jobs are handled by a single integer, need to parse selected days and generate that number
                foreach(string s in job.SelectedDays)
                {
                    frequency += int.Parse(s);
                }
                AM_SqlServer.Models.WAMSQLJob wsj = new AM_SqlServer.Models.WAMSQLJob();
                wsj.Name = job.Name;
                wsj.Description = job.Description;
                wsj.OriginatingServer = job.Server;
                wsj.ScheduleName = job.ScheduleName;
                wsj.ScheduleFrequencyInterval = frequency;
                wsj.ScheduleFrequencyRecurrenceFactor = 1;
                wsj.ScheduleFrequencyType = FrequencyTypes.Weekly;
                wsj.ScheduleActiveStartTimeOfDay = TimeSpan.Parse(job.RunTime);
                wsj.JobStepName = job.StepName;
                wsj.JobStepCommand = job.Command;

                //AM_SqlServer.SqlServerManager ssm = new AM_SqlServer.SqlServerManager();
                _ssm.CreateJob(wsj);
            }
            return RedirectToAction("Index");
        }

        // POST: SQLJobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobID,Name,Description,OriginatingServer,DateCreated,DateModified,LastRunDate,NextRunDate")] SQLJob sQLJob)
        {
            if (ModelState.IsValid)
            {
                sQLJob.JobID = Guid.NewGuid();
                _context.Add(sQLJob);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sQLJob);
        }

        // GET: SQLJobs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sQLJob = await _context.SQLJobs.FindAsync(id);
            if (sQLJob == null)
            {
                return NotFound();
            }
            return View(sQLJob);
        }

        // POST: SQLJobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("JobID,Name,Description,OriginatingServer,DateCreated,DateModified,LastRunDate,NextRunDate")] SQLJob sQLJob)
        {
            if (id != sQLJob.JobID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sQLJob);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SQLJobExists(sQLJob.JobID))
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
            return View(sQLJob);
        }

        // GET: SQLJobs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sQLJob = await _context.SQLJobs
                .FirstOrDefaultAsync(m => m.JobID == id);
            if (sQLJob == null)
            {
                return NotFound();
            }

            return View(sQLJob);
        }

        // POST: SQLJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sQLJob = await _context.SQLJobs.FindAsync(id);
            _context.SQLJobs.Remove(sQLJob);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SQLJobExists(Guid id)
        {
            return _context.SQLJobs.Any(e => e.JobID == id);
        }
    }
}
