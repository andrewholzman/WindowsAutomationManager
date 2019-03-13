using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutomationManager.Data;
using AM_CustomJobs;
using AM_CustomJobs.Models;
using System.IO;

namespace AutomationManager.Models
{
    public class CustomJobsController : Controller
    {
        private readonly DatabaseContext _context;
        private CustomJobManager _cjm;
        private string localIP = Utils.GetAllLocalIPv4(System.Net.NetworkInformation.NetworkInterfaceType.Ethernet).FirstOrDefault();
        private string _folderPath = "\\WAM\\Uploads\\";

        public CustomJobsController(DatabaseContext context)
        {
            _context = context;
            _cjm = new CustomJobManager();
        }

        // GET: CustomJobs
        public async Task<IActionResult> Index()
        {
            List<CustomJobModel> jobs = _cjm.GetJobs();
            List<WAMCustomJob> jobsToReturn = new List<WAMCustomJob>();
            foreach (CustomJobModel job in jobs)
            {
                WAMCustomJob wcj = new WAMCustomJob(job.JobName, job.ScriptType, job.TriggerString, job.LastResult, job.ActionFilePath);
                jobsToReturn.Add(wcj);
            }
            return View(jobsToReturn);
            //return View(await _context.WAMCustomJob.ToListAsync());
        }

        // GET: CustomJobs/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //todo: make this async
            var job = _cjm.GetJob(id);

            if (job == null)
            {
                return NotFound();
            }
            WAMCustomJob wcj = new WAMCustomJob(job.JobName, job.ScriptType, job.TriggerString, job.LastResult, job.ActionFilePath);

            return View(wcj);
        }

        // GET: CustomJobs/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WAMCustomJob job)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (job.ActionFile.Length > 0)
                    {
                        // string filePath = $@"\\{localIP}\c$" + _folderPath;
                        string filePath = "C:\\WAM\\Uploads\\" + job.ActionFile.FileName;
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            job.ActionFile.CopyToAsync(stream);
                            job.ActionFilePath = $@"\\{localIP}" + _folderPath + job.ActionFile.FileName;


                        }
                    }
                    //todo: make this async
                    _cjm.CreateOrUpdateJob(job.JobName, job.ScriptType, job.ActionFilePath, job.TriggerString);

                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to create custom job {job.JobName}. Error: {ex.Message}");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(WAMCustomJob job, string oldJobName)
        {
            try
            {
                if (job.ActionFile.Length > 0)
                {
                    // string filePath = $@"\\{localIP}\c$" + _folderPath;
                    string filePath = "C:\\WAM\\Uploads\\" + job.ActionFile.FileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        job.ActionFile.CopyToAsync(stream);
                        job.ActionFilePath = $@"\\{localIP}" + _folderPath + job.ActionFile.FileName;


                    }
                }
                //todo: make this async
                if (job.JobName != oldJobName)
                {
                    _cjm.CreateOrUpdateJob(job.JobName, job.ScriptType, job.ActionFilePath, job.TriggerString, oldJobName);
                } else
                {
                    _cjm.CreateOrUpdateJob(job.JobName, job.ScriptType, job.ActionFilePath, job.TriggerString);
                }
                   

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create custom job {job.JobName}. Error: {ex.Message}");
            }
            
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                //todo: make this async
                var job = _cjm.GetJob(id);

                if (job == null)
                {
                    return NotFound();
                }
                WAMCustomJob wcj = new WAMCustomJob(job.JobName, job.ScriptType, job.TriggerString, job.LastResult, job.ActionFilePath);
                return View(wcj);

            } catch (Exception ex)
            {
                throw new Exception($"Failed to update job: {id}. Error: {ex.Message}");
            }
        }

        //[HttpPost]
        //public IActionResult Edit(WAMCustomJob job)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _cjm.CreateOrUpdateJob(job.JobName, job.ScriptType, job.ActionFilePath, job.TriggerString);

        //        }
        //        catch (Exception ex)
        //        {
        //            throw new Exception($"Failed to update job: {job.JobName}. Error: {ex.Message}");
        //        }

        //    }
        //    return RedirectToAction("Index");
        //}


        // GET: CustomJobs/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                //todo: make async
                _cjm.RemoveJob(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to remove job: {id}. Error: {ex.Message}");
            }

            return RedirectToAction("Index");
        }

        
        public IActionResult TriggerJob(string id)
        {
            if (id==null)
            {
                return NotFound();
            }

            try
            {
                _cjm.TriggerJob(id);
            }
            catch (Exception eX)
            {
                throw new Exception($"Failed to trigger job {id}. Error: {eX.Message}");
            }
            return RedirectToAction("Details", "CustomJobs", new { id = id });
        }
    }
}
