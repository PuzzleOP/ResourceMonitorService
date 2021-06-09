using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace ResourceMonitorService
{
    class Program
    {
        public static int memorySize;
        public static object tvms;

        static void Main(string[] args)
        {
            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                tvms = result["TotalVisibleMemorySize"];
                memorySize = Convert.ToInt32(tvms) / 1024;

            }

            var runValue = HostFactory.Run(x => 
            {
                x.Service<ResourceMonitor>(y =>
                {
                    y.ConstructUsing(rm => new ResourceMonitor());
                    y.WhenStarted(rm => rm.Start());
                    y.WhenStopped(rm => rm.Stop());
                });
                
                x.RunAsLocalSystem();

                x.SetServiceName("ResourceMonitorService");
                x.SetDisplayName("Resource Monitor Service Demo");
                x.SetDescription("A Resource Monitor Service done as a demo");

            });

            int runValueInteger = (int) Convert.ChangeType(runValue, runValue.GetTypeCode());
            Environment.ExitCode = runValueInteger;

        }
    }
}
