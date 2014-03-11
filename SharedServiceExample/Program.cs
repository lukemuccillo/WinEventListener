using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;
using System.Text;

namespace SharedServiceExample
{
    static class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[] { new WtsService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
