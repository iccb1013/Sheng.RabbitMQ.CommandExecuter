using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShareJoy.SilkRoad.DataSync.Core
{
    public class RabbitMQConfig_QueueList
    {
        [XmlElement("queue")]
        public List<RabbitMQConfig_Queue> QueueList
        {
            get;set;
        }
    }
}
