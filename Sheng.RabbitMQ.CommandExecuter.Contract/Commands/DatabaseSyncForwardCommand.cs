using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.Contract
{
    /// <summary>
    /// 数据库同步命令
    /// </summary>
    public class DatabaseSyncForwardCommand: Command
    {
        public const string CommandTypeName = "DatabaseSyncForwardCommand";

        public DatabaseSyncForwardCommand()
        {
            base.CommandType = CommandTypeName;
        }

        public string Url
        {
            get;set;
        }

        public string CommandContent
        {
            get;set;
        }
    }
}
