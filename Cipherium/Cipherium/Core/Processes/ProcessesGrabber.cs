using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cipherium.Core.Processes
{
    internal class ProcessesGrabber
    {
        public static string GetProcesses()
        {
            string actualString = @"";
            Process[] processes = Process.GetProcesses();

            // Print information about each process
            foreach (Process process in processes)
            {
                actualString += $@" 
---------------------------------
Name: {process.ProcessName}
PID: {process.Id}
Memory Usage (MB): {process.PrivateMemorySize64 / 1024 / 1024}
---------------------------------
";
                
            }

            return actualString;
        }
    }
}
