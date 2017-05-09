using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ShareJoy.SilkRoad.DataSync.Core
{
    public class RabbitMQConfig
    {
        static RabbitMQConfig()
        {
        }

        public static RabbitMQConfig_Root GetRabbitMQConfig()
        {
            RabbitMQConfig_Root config = null;

            try
            {
                XmlSerializer _xmlSerializer = new XmlSerializer(typeof(RabbitMQConfig_Root));
                string strXmlFile = Path.Combine(Application.StartupPath, "RabbitMQConfig.xml");
                FileStream stream = new FileStream(strXmlFile, FileMode.Open);
                config = _xmlSerializer.Deserialize(stream) as RabbitMQConfig_Root;
            }catch(Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            return config;
        }
    }
}
