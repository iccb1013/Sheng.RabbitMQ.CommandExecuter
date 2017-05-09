using Linkup.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.WindowsService
{
    static class Program
    {
        

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
           // Thread.Sleep(15000);

            LogService _logService = LogService.Instance;

            _logService.Write("Application_Start");

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new DataSyncService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
