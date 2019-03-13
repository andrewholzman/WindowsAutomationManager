using AM_CustomJobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationManagerTests
{
    [TestClass]
    public class CustomJobTests
    {
        CustomJobManager _cjm = new CustomJobManager();
        [TestMethod]
        public void JobShouldCreateSuccessfully()
        {
            string id = "tset-job-1";
            string scriptType = "VBS";
            string actionFilePath = "C:\temp\temp.vbs";
            string triggerString = "0 * * * *";
            _cjm.CreateOrUpdateJob(id, scriptType, actionFilePath, triggerString);
        }
        [TestMethod]
        public void ShouldReturnJobs()
        {
            var jobs = _cjm.GetJobs();
            Assert.IsFalse(jobs.Count == 0);
        }

        [TestMethod]
        public void GetJobById()
        {
            var job = _cjm.GetJob("tset-job-1");
            Assert.IsNotNull(job);
        }
    }
}
