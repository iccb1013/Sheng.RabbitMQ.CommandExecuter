using Linkup.Common;
using Sheng.RabbitMQ.CommandExecuter.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    public class CommandExecuterService
    {
        private static readonly CommandExecuterService _instance = new CommandExecuterService();
        public static CommandExecuterService Instance
        {
            get { return _instance; }
        }

        private RabbitMQService _rabbitMQService = RabbitMQService.Instance;
        private CommandReceiverFactory _commandReceiverFactory = CommandReceiverFactory.Instance;
        private LogService _logService = LogService.Instance;

        private CommandExecuterService()
        {
            _rabbitMQService.Subscribe(MessageHandle);
        }

        private void MessageHandle(ulong deliveryTag, string routingKey, string body)
        {
            if (String.IsNullOrEmpty(routingKey) || String.IsNullOrEmpty(body))
            {
                _logService.Write("routingKey 或 body 为空。", TraceEventType.Warning);
                Debug.Assert(false, "routingKey 或 body 为空。");
                return;
            }

            string strCommandType = JsonHelper.GetProperty(body, "CommandType");

            if (String.IsNullOrEmpty(strCommandType))
            {
                _logService.Write("strCommandType 为空。", body, TraceEventType.Warning);
                Debug.Assert(false, "strCommandType 为空。");
                return;
            }

            _commandReceiverFactory.Handle(routingKey, strCommandType, body);
        }

        public void Start()
        {
            _rabbitMQService.Start();
        }
    }
}
