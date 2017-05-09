using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShareJoy.SilkRoad.DataSync.Core
{
    [XmlRoot("rabbitMQ")]
    public class RabbitMQConfig_Root
    {
        [XmlElement("connectionFactory")]
        public RabbitMQConfig_ConnectionFactory ConnectionFactory
        {
            get; set;
        }

        [XmlElement("channel")]
        public RabbitMQConfig_Channel Channel
        {
            get;set;
        }
    }
}
