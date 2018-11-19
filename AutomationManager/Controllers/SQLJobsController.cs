using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public SQLJobsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: SQLJobs
        public async Task<IActionResult> Index()
        {
            AM_SqlServer.SqlServerManager ssm = new AM_SqlServer.SqlServerManager();
            //TODO: use AppSettings to pull this, set value through a settings/config menu
            List<Job> localJobs = ssm.GetAllSqlAgentJobs("sa","password");
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

            List<Job> remoteJobs = ssm.GetAllSqlAgentJobs("sa", "password", "SERVER02");
            foreach (Job j in remoteJobs)
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
            return View(sqlJobs);
           // return View(await _context.SQLJobs.ToListAsync());
        }

        // GET: SQLJobs/Details/5
        public async Task<IActionResult> Details(Guid? id)
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

        // GET: SQLJobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST : SQLJobs/Create but no to entity db, create to SQL server
        public IActionResult CreateJob(Job job)
        {
            if (ModelState.IsValid)
            {
                AM_SqlServer.SqlServerManager ssm = new AM_SqlServer.SqlServerManager();
                ssm.CreateJob(job);
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
