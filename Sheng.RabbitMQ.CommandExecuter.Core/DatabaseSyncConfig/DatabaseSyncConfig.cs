using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    public class DatabaseSyncConfig
    {
        static DatabaseSyncConfig()
        {
        }

        public static DatabaseSyncConfig_Root GetDatabaseSyncConfig()
        {
            DatabaseSyncConfig_Root config = null;
            FileStream stream = null;
            try
            {
                XmlSerializer _xmlSerializer = new XmlSerializer(typeof(DatabaseSyncConfig_Root));
                string strXmlFile = Path.Combine(Application.StartupPath, "DatabaseSyncConfig.xml");
                stream = new FileStream(strXmlFile, FileMode.Open);
                config = _xmlSerializer.Deserialize(stream) as DatabaseSyncConfig_Root;

            }
            catch (Exception ex)
            {
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
