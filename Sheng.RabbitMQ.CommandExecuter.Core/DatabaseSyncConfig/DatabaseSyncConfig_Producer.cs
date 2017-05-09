using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    public class DatabaseSyncConfig_Producer
    {
        [XmlAttribute("name")]
        public string Name
        {
            get; set;
        }

        [XmlAttribute("routingKey")]
        public string RoutingKey
        {
            get; set;
        }

        [XmlAttribute("queueName")]
        public string QueueName
        {
            get; set;
        }

        [XmlAttribute("connection")]
        public string Connection
        {
            get; set;
        }

        [XmlElement("tableDefinition")]
        public DatabaseSyncConfig_TableDefinition TableDefinition
        {
            get; set;
        }
    }

    public class DatabaseSyncConfig_TableDefinition
    {
        [XmlElement("table")]
        public List<DatabaseSyncConfig_Table> TableList
        {
            get; set;
        }
    }
}
