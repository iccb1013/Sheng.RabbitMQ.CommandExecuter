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
    public class DatabaseSyncCommand: Command
    {
        public const string CommandTypeName = "DatabaseSyncCommand";

        private List<DatabaseSyncItem> _syncItemList = new List<DatabaseSyncItem>();
        /// <summary>
        /// 要同步的项目列表，列表中的项目将封装成事务按集合顺序执行
        /// </summary>
        public List<DatabaseSyncItem> SyncItemList
        {
            get { return _syncItemList; }
            set { _syncItemList = value; }
        }

        public DatabaseSyncCommand()
        {
            base.CommandType = CommandTypeName;
        }
    }

    /// <summary>
    /// 数据库同步项目
    /// </summary>
    public class DatabaseSyncItem
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Table
        {
            get;set;
        }

        /// <summary>
        /// 主键的值
        /// </summary>
        public string PrimaryKeyValue
        {
            get;set;
        }

        /// <summary>
        /// 动作
        /// </summary>
        public DatabaseSyncAction Action
        {
            get;set;
        }
    }
}
