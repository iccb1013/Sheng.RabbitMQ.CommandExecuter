using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    public class DatabaseSyncConfig_Table
    {
        [XmlAttribute("name")]
        public string Name
        {
            get; set;
        }

        [XmlAttribute("primaryKey")]
        public string PrimaryKey
        {
            get; set;
        }

        [XmlAttribute("consumerTable")]
        public string ConsumerTable
        {
            get; set;
        }

        [XmlAttribute("consumerTablePrimaryKey")]
        public string ConsumerTablePrimaryKey
        {
            get; set;
        }

        [XmlElement("Field")]
        public List<DatabaseSyncConfig_Field> Field
        {
            get;set;
        }
    }
}
