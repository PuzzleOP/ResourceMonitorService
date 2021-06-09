using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ResourceMonitorService
{
    class ResourceMonitor
    {
        private readonly Timer t;

        static int scanFrequency = 2000;
        string filePath = @"D:\Ivan\Repos\ResourceMonitorLog.txt";

        PerformanceCounter pCPUCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        PerformanceCounter pMemoryAvailable = new PerformanceCounter("Memory", "Available MBytes");
        PerformanceCounter pSystemUpTime = new PerformanceCounter("System", "System Up Time");
        PerformanceCounter pMemoryBytesCached = new PerformanceCounter("Memory","Cache Bytes");


        public ResourceMonitor()
        {
            t = new Timer(scanFrequency) { AutoReset = true };
            t.Elapsed += timerElapsed;
            
        }

        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            string[] lines = new string[]
            {
                DateTime.Now.ToString() + " ==>  \nCPU: " +(int)pCPUCounter.NextValue() + 
                "%\nMemory: " + (int)pMemoryAvailable.NextValue() +
                " MB /" + Program.memorySize + " MB\nMemory Cached Bytes: " +
                ((int)pMemoryBytesCached.NextValue() / 1024) / 1024 +
                " MB\nSystem Up Time: " + (int)pSystemUpTime.NextValue() / 60 +
                " minutes " + (int)pSystemUpTime.NextValue() % 60 +
                " seconds\n------------------------"
            };

            File.AppendAllLines(filePath, lines);
        }

        public void Start()
        {
            t.Start();
        }

        public void Stop()
        {
            t.Stop();
        }

    }
}
