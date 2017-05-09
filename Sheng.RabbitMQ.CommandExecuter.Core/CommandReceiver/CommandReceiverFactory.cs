using Linkup.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    class CommandReceiverFactory
    {
        private static readonly CommandReceiverFactory _instance = new CommandReceiverFactory();
        public static CommandReceiverFactory Instance
        {
            get { return _instance; }
        }

        protected static LogService _logService = LogService.Instance;

        private Dictionary<string, CommandReceiver> _receiverList = new Dictionary<string, CommandReceiver>();

        private CommandReceiverFactory()
        {
            List<Type> receiverTypeList = ReflectionHelper.GetTypeListBaseOn<CommandReceiver>();
            foreach (var receiverType in receiverTypeList)
            {
                CommandReceiver receiver = (CommandReceiver)Activator.CreateInstance(receiverType);
                _receiverList.Add(receiver.CommandType, receiver);
            }
        }

        public void Handle(string routingKey, string strCommandType, string strCommand)
        {
            _logService.Write("处理命令：" + strCommandType , strCommand, TraceEventType.Verbose);

            if (String.IsNullOrEmpty(strCommandType) || String.IsNullOrEmpty(strCommand))
            {
                _logService.Write("strCommandType 或  strCommand 为空" ,
                    strCommandType + Environment.NewLine + strCommand, TraceEventType.Verbose);

                Debug.Assert(false, "strCommandType 或  strCommand 为空");
                return;
            }

            if(_receiverList.ContainsKey(strCommandType) == false)
            {
                _logService.Write("strCommandType 不支持：" + strCommandType, strCommand, TraceEventType.Error);

                Debug.Assert(false, "strCommandType 不支持");
                return;
            }

            CommandReceiver receiver = _receiverList[strCommandType];

            try
            {
                receiver.Handle(routingKey, strCommand);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);

                string msg = strCommand;

                msg += Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                {
                    msg += Environment.NewLine + ex.InnerException.Message;
                }

                _logService.Write("CommandReceiverFactory.Handle 异常", msg, TraceEventType.Error);
            }

        }
    }
}
