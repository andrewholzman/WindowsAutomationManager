using AM_TaskScheduler;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomationManagerTests
{
    [TestClass]
    public class WindowsTaskIntegrationTests
    {
        [TestMethod]
        public void FindJobWithSubstring()
        {
            string id = "Keeps your Google software up to date".Substring(0, 20);
            bool substring = true;
            TaskSchedulerManager tsm = new TaskSchedulerManager();
            var t = tsm.GetTask(id, substring);
        }
    }
}

