using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.Contract
{
    public abstract class Command
    {
        public string CommandType
        {
            get;
            protected set;
        }
    }
}
