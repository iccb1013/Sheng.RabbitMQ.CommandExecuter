using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.RabbitMQ
{
    [XmlRoot("rabbitMQ")]
    public class RabbitMQConfig_Root
    {
        [XmlElement("connectionFactory")]
        public RabbitMQConfig_ConnectionFactory ConnectionFactory
        {
            get; set;
        }

        [XmlElement("exchangeList")]
        public RabbitMQConfig_Root_ExchangeList ExchangeList
        {
            get; set;
        }
    }

    public class RabbitMQConfig_Root_ExchangeList
    {
        [XmlElement("exchange")]
        public List<RabbitMQConfig_Exchange> Exchange
        {
            get; set;
        }
    }
}
