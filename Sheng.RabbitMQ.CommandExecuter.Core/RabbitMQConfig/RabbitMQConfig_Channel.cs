using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShareJoy.SilkRoad.DataSync.Core
{
    public class RabbitMQConfig_Channel
    {
        [XmlElement("queueList")]
        public RabbitMQConfig_QueueList QueueList
        {
            get;set;
        }
    }
}
