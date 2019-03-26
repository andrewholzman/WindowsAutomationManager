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
        string _id = "unit-test-job";
        string _scriptType = "VBS";
        string _actionFilePath = "C:\temp\temp.vbs";
        string _triggerString = "0 * * * *";
        [TestMethod]
        public void JobShouldCreateSuccessfully()
        {
            _cjm.CreateOrUpdateJob(_id, _scriptType, _actionFilePath, _triggerString);
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
            var job = _cjm.GetJob(_id);
            Assert.IsNotNull(job);
        }

        [TestMethod]
        public void JobShouldDeleteSuccessfully()
        {
            JobShouldCreateSuccessfully();
            _cjm.RemoveJob(_id);
            var job = _cjm.GetJob(_id);
            Assert.IsNull(job); //job was not returned, therefore it does not exist
        }

        [TestMethod]
        public void JobShouldTriggerSuccessfully()
        {
            JobShouldCreateSuccessfully();
            _cjm.TriggerJob(_id);
            var job = _cjm.GetJob(_id);
            Assert.IsNotNull(job.LastResult); //job has a last result, which means its ran
        }
    }
}
