using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM_History;
using AutomationManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutomationManager.Controllers
{
    public class DashboardController : Controller
    {
        JobHistoryManager _jhm = new JobHistoryManager();
        int _limit = 1000;
        // GET: Dashboard
        public ActionResult Index()
        {
            List<HistoryModel> allHistFromDB = _jhm.GetAllJobHistory();
            List<JobHistory> historyToDisplay = new List<JobHistory>();
            int counter = 0;
            foreach(HistoryModel hm in allHistFromDB)
            {
                if (counter==_limit) { break;  }
                if (hm.Error != null)
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, hm.Error));
                } else
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, null));
                }
                counter++;
                
            }
            //do something to get jobHistory
            return View(historyToDisplay);
        }

        // GET: SqlJobsDash
        public ActionResult SqlJobHistory()
        {
            List<HistoryModel> allHistFromDB = _jhm.GetJobHistoryForJobType("SQL");
            List<JobHistory> historyToDisplay = new List<JobHistory>();
            int counter = 0;
            foreach (HistoryModel hm in allHistFromDB)
            {
                if (counter == _limit) { break; }
                if (hm.Error != null)
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, hm.Error));
                }
                else
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, null));
                }
                counter++;

            }
            //do something to get jobHistory
            return View(historyToDisplay);
        }

        public ActionResult SqlJobHistoryForType(string type)
        {
            List<HistoryModel> allHistFromDB = _jhm.GetJobHistoryForJobType("SQL", type);
            List<JobHistory> historyToDisplay = new List<JobHistory>();
            int counter = 0;
            foreach (HistoryModel hm in allHistFromDB)
            {
                if (counter == _limit) { break; }
                if (hm.Error != null)
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, hm.Error));
                }
                else
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, null));
                }
                counter++;

            }
            //do something to get jobHistory
            return View("SqlJobHistory",historyToDisplay);
        }

        // GET: SqlJobsDash
        public ActionResult CustomJobHistory()
        {
            List<HistoryModel> allHistFromDB = _jhm.GetJobHistoryForJobType("Custom");
            List<JobHistory> historyToDisplay = new List<JobHistory>();
            int counter = 0;
            foreach (HistoryModel hm in allHistFromDB)
            {
                if (counter == _limit) { break; }
                if (hm.Error != null)
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, hm.Error));
                }
                else
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, null));
                }
                counter++;

            }
            //do something to get jobHistory
            return View(historyToDisplay);
        }

        public ActionResult CustomJobHistoryForType(string type)
        {
            List<HistoryModel> allHistFromDB = _jhm.GetJobHistoryForJobType("Custom", type);
            List<JobHistory> historyToDisplay = new List<JobHistory>();
            int counter = 0;
            foreach (HistoryModel hm in allHistFromDB)
            {
                if (counter == _limit) { break; }
                if (hm.Error != null)
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, hm.Error));
                }
                else
                {
                    historyToDisplay.Add(new JobHistory(hm.Id, hm.JobType, hm.JobName, hm.DateRan, hm.Status, null));
                }
                counter++;

            }
            //do something to get jobHistory
            return View("CustomJobHistory", historyToDisplay);
        }
        // GET: Dashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Dashboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dashboard/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Dashboard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dashboard/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}