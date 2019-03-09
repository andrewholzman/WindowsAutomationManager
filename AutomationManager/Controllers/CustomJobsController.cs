using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutomationManager.Data;
using AM_CustomJobs;

namespace AutomationManager.Models
{
    public class CustomJobsController : Controller
    {
        private readonly DatabaseContext _context;
        private CustomJobManager _cjm;

        public CustomJobsController(DatabaseContext context)
        {
            _context = context;
            _cjm = new CustomJobManager();
        }

        // GET: CustomJobs
        public async Task<IActionResult> Index()
        {
            var x = _cjm.GetJobs();
            return View(x);
            //return View(await _context.WAMCustomJob.ToListAsync());
        }

        // GET: CustomJobs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wAMCustomJob = await _context.WAMCustomJob
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wAMCustomJob == null)
            {
                return NotFound();
            }

            return View(wAMCustomJob);
        }

        // GET: CustomJobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomJobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,JobType,JobName")] WAMCustomJob wAMCustomJob)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        wAMCustomJob.Id = Guid.NewGuid();
        //        _context.Add(wAMCustomJob);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(wAMCustomJob);
        //}

        // GET: CustomJobs/Edit/5
        //public async Task<IActionResult> Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var wAMCustomJob = await _context.WAMCustomJob.FindAsync(id);
        //    if (wAMCustomJob == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(wAMCustomJob);
        //}

        // POST: CustomJobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(Guid id, [Bind("Id,JobType,JobName")] WAMCustomJob wAMCustomJob)
        //{
        //    if (id != wAMCustomJob.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(wAMCustomJob);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!WAMCustomJobExists(wAMCustomJob.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(wAMCustomJob);
        //}

        // GET: CustomJobs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wAMCustomJob = await _context.WAMCustomJob
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wAMCustomJob == null)
            {
                return NotFound();
            }

            return View(wAMCustomJob);
        }

        // POST: CustomJobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var wAMCustomJob = await _context.WAMCustomJob.FindAsync(id);
            _context.WAMCustomJob.Remove(wAMCustomJob);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WAMCustomJobExists(Guid id)
        {
            return _context.WAMCustomJob.Any(e => e.Id == id);
        }
    }
}
