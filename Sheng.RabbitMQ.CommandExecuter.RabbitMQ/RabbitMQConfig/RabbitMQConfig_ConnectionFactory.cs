using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.RabbitMQ
{
    public class RabbitMQConfig_ConnectionFactory
    {
        [XmlAttribute("hostName")]
        public string HostName
        {
            get; set;
        }

        [XmlAttribute("userName")]
        public string UserName
        {
            get; set;
        }

        [XmlAttribute("password")]
        public string Password
        {
            get; set;
        }
    }
}
