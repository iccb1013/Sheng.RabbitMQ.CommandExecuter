using Linkup.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.RabbitMQ
{
    public class RabbitMQConfig
    {
        private static LogService _logService = LogService.Instance;

        static RabbitMQConfig()
        {
        }

        public static RabbitMQConfig_Root GetRabbitMQConfig()
        {
            RabbitMQConfig_Root config = null;
            FileStream stream = null;
            try
            {
                string strXmlFile = Path.Combine(Application.StartupPath, "RabbitMQConfig.xml");
                if (File.Exists(strXmlFile) == false)
                {
                    strXmlFile = Path.Combine(ConfigurationManager.AppSettings["RootPath"], "RabbitMQConfig.xml");
                }

                _logService.Write("RabbitMQConfig.xml", strXmlFile, TraceEventType.Verbose);

                XmlSerializer _xmlSerializer = new XmlSerializer(typeof(RabbitMQConfig_Root));
                stream = new FileStream(strXmlFile, FileMode.Open);
                config = _xmlSerializer.Deserialize(stream) as RabbitMQConfig_Root;
            }
            catch (Exception ex)
            {
                _logService.Write("RabbitMQConfig.xml 加载失败", ex.Message, TraceEventType.Error);
                Debug.Assert(false, ex.Message);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }

            return config;
        }
    }
}
