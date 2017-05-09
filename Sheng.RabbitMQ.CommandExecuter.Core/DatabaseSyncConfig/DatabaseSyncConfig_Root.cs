using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    [XmlRoot("databaseSync")]
    public class DatabaseSyncConfig_Root
    {
        [XmlElement("connectionList")]
        public DatabaseSyncConfig_Root_ConnectionList ConnectionList
        {
            get; set;
        }

        [XmlElement("consumerList")]
        public DatabaseSyncConfig_Root_ConsumerList ConfigList
        {
            get; set;
        }
    }

    public class DatabaseSyncConfig_Root_ConnectionList
    {
        [XmlElement("connection")]
        public List<DatabaseSyncConfig_Connection> Connection
        {
            get; set;
        }
    }

    public class DatabaseSyncConfig_Root_ConsumerList
    {
        [XmlElement("consumer")]
        public List<DatabaseSyncConfig_Consumer> ConsumerList
        {
            get; set;
        }
    }
}
