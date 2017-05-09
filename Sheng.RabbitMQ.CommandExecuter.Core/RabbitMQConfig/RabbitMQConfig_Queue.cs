using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShareJoy.SilkRoad.DataSync.Core
{
    public class RabbitMQConfig_Queue
    {
        [XmlAttribute("name")]
        public string Name
        {
            get; set;
        }

        [XmlAttribute("durable")]
        public bool Durable
        {
            get; set;
        }

        [XmlAttribute("exclusive")]
        public bool Exclusive
        {
            get; set;
        }

        [XmlAttribute("autoDelete")]
        public bool AutoDelete
        {
            get; set;
        }
    }
}
