using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.RabbitMQ
{
    public class RabbitMQConfig_Exchange
    {
        [XmlAttribute("name")]
        public string Name
        {
            get;set;
        }

        [XmlAttribute("type")]
        public string Type
        {
            get; set;
        }

        [XmlElement("queueList")]
        public RabbitMQConfig_Exchange_QueueList QueueList
        {
            get;set;
        }
    }

    public class RabbitMQConfig_Exchange_QueueList
    {
        [XmlElement("queue")]
        public List<RabbitMQConfig_Queue> Queue
        {
            get; set;
        }
    }
}
