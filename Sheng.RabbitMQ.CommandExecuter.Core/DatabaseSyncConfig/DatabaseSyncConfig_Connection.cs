using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    public class DatabaseSyncConfig_Connection
    {
        [XmlAttribute("name")]
        public string Name
        {
            get; set;
        }

        [XmlAttribute("connectionString")]
        public string ConnectionString
        {
            get; set;
        }
    }
}
