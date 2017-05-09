using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    public class DatabaseSyncConfig_Consumer
    {
        [XmlAttribute("name")]
        public string Name
        {
            get; set;
        }
       
        [XmlAttribute("connection")]
        public string Connection
        {
            get; set;
        }

        [XmlElement("producerList")]
        public DatabaseSyncConfig_Consumer_ProducerList ProducerList
        {
            get;set;
        }
    }

    public class DatabaseSyncConfig_Consumer_ProducerList
    {
        [XmlElement("producer")]
        public List<DatabaseSyncConfig_Producer> Producer
        {
            get; set;
        }
    }
}
