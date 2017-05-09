using Linkup.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    abstract class CommandReceiver
    {
        protected static LogService _logService = LogService.Instance;

        public string CommandType
        {
            get;
            protected set;
        }

        public abstract void Handle(string routingKey, string strCommand);
    }
}
