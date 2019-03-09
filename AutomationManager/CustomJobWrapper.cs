using AM_CustomJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomationManager
{
    public class CustomJobWrapper
    {
       public CustomJobManager CJManager;

        public void Start()
        {
            CJManager = new CustomJobManager();
            CJManager.Start();
        }

        public void Stop()
        {
            CJManager.Stop();
        }
    }
}
