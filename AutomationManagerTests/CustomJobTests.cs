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
            //_cjm.CreateOrUpdateJob();
            //var x = _cjm.GetJobs();
            //Assert.IsFalse(x == null);
        }

        [TestMethod]
        public void ShouldReturnJobs()
        {
            var jobs = _cjm.GetJobs();
            Assert.IsFalse(jobs.Count == 0);
        }
    }
}
