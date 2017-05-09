using Linkup.Common;
using Newtonsoft.Json;
using Sheng.RabbitMQ.CommandExecuter.Contract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    class DatabaseSyncForwardCommandReceiver : CommandReceiver
    {
        private HttpService _httpService = HttpService.Instance;
        private string _url;

        public DatabaseSyncForwardCommandReceiver()
        {
            base.CommandType = DatabaseSyncForwardCommand.CommandTypeName;

            _url = ConfigurationManager.AppSettings["DatabaseSyncForwardUrl"];
        }

        public override void Handle(string routingKey, string strCommand)
        {
            DatabaseSyncForwardCommand command = 
                JsonConvert.DeserializeObject<DatabaseSyncForwardCommand>(strCommand);

            HttpRequestArgs args = new HttpRequestArgs();

            if (String.IsNullOrEmpty(command.Url) == false)
            {
                args.Url = command.Url;
            }
            else
            {
                args.Url = _url;
            }

            args.Content = command.CommandContent;

            _logService.Write("DatabaseSyncForwardCommandReceiver 发起HTTP请求",
                JsonHelper.Serializer(args), TraceEventType.Verbose);

            HttpRequestResult result = _httpService.Request(args);

            if (result.Success)
            {
                _logService.Write("DatabaseSyncForwardCommandReceiver HTTP请求完成",
                    JsonHelper.Serializer(result), TraceEventType.Verbose);
            }
            else
            {
                _logService.Write("DatabaseSyncForwardCommandReceiver HTTP请求失败",
                    JsonHelper.Serializer(result), TraceEventType.Warning);

            }
        }
    }
}
