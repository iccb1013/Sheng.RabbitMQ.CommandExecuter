using Linkup.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.WindowsService
{
    public partial class DataSyncService : ServiceBase
    {
        static LogService _logService = LogService.Instance;

        public DataSyncService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _logService.Write("OnStart");

            Sheng.RabbitMQ.CommandExecuter.Core.CommandExecuterService.Instance.Start();
        }

        protected override void OnStop()
        {
            _logService.Write("OnStop");
        }
    }
}
