using Linkup.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShareJoy.SilkRoad.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareJoy.SilkRoad.RabbitMQ
{
    public class RabbitMQClient
    {
        private static readonly RabbitMQClient _instance = new RabbitMQClient();
        public static RabbitMQClient Instance
        {
            get { return _instance; }
        }

        private RabbitMQConfig_Root _rabbitMQConfig = null;
        private LogService _logService = LogService.Instance;

        private Dictionary<string, List<RabbitMQCallback>> _callbackList = new Dictionary<string, List<RabbitMQCallback>>();

        private RabbitMQClient()
        {
            try
            {
                _rabbitMQConfig = RabbitMQConfig.GetRabbitMQConfig();
            }
            catch (Exception ex)
            {
                _logService.Write("加载 RabbitMQ 配置失败", ex.Message, TraceEventType.Error);
            }
        }

        public void Start()
        {
            if (_rabbitMQConfig == null)
            {
                _logService.Write("RabbitMQClient.Start 失败，没有加载 RabbitMQ 配置", TraceEventType.Error);
                return;
            }

            _logService.Write("RabbitMQClient.Start", TraceEventType.Verbose);

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = _rabbitMQConfig.ConnectionFactory.HostName;
            factory.UserName = _rabbitMQConfig.ConnectionFactory.UserName;
            factory.Password = _rabbitMQConfig.ConnectionFactory.Password;

            IConnection connection = factory.CreateConnection();
            connection.CallbackException += Connection_CallbackException;
            connection.ConnectionBlocked += Connection_ConnectionBlocked;
            connection.ConnectionUnblocked += Connection_ConnectionUnblocked;
            connection.ConnectionShutdown += Connection_ConnectionShutdown;

            IModel channel = connection.CreateModel();

            foreach (RabbitMQConfig_Exchange exchange in _rabbitMQConfig.ExchangeList.Exchange)
            {
                foreach (RabbitMQConfig_Queue queue in exchange.QueueList.Queue)
                {
                    channel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete, null);
                }
            }

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            foreach (RabbitMQConfig_Exchange exchange in _rabbitMQConfig.ExchangeList.Exchange)
            {
                foreach (RabbitMQConfig_Queue queue in exchange.QueueList.Queue)
                {
                    channel.BasicConsume(queue.Name, true, consumer);
                }
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string strBody = Encoding.UTF8.GetString(e.Body);

            _logService.Write("收到MQ消息命令", strBody, TraceEventType.Verbose);

            if (_callbackList.Count == 0)
                return;

            if (_callbackList.ContainsKey(e.RoutingKey) == false)
                return;

            List<RabbitMQCallback> actionList = _callbackList[e.RoutingKey];

            if (actionList.Count == 0)
                return;

            foreach (var action in actionList)
            {
                action(e.RoutingKey, strBody);
            }
        }

        #region ConnectionEvent

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logService.Write("RabbitMQClient.Connection_ConnectionShutdown", TraceEventType.Verbose);
        }

        private void Connection_ConnectionUnblocked(object sender, EventArgs e)
        {
            _logService.Write("RabbitMQClient.Connection_ConnectionUnblocked", TraceEventType.Verbose);
        }

        private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            _logService.Write("RabbitMQClient.Connection_ConnectionBlocked", TraceEventType.Verbose);
        }

        private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            _logService.Write("RabbitMQClient.Connection_CallbackException", TraceEventType.Verbose);
        }

        #endregion

        public void Subscribe(string routingKey, RabbitMQCallback callback)
        {
            if (String.IsNullOrEmpty(routingKey))
            {
                throw new ArgumentNullException("routingKey");
            }

            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            if (_callbackList.ContainsKey(routingKey) == false)
            {
                lock (_callbackList)
                {
                    if (_callbackList.ContainsKey(routingKey) == false)
                    {
                        _callbackList.Add(routingKey, new List<RabbitMQCallback>());
                    }
                }
            }

            List<RabbitMQCallback> actionList = _callbackList[routingKey];

            if (actionList.Contains(callback) == false)
            {
                lock (actionList)
                {
                    if (actionList.Contains(callback) == false)
                    {
                        actionList.Add(callback);
                    }
                }
            }
        }


        public delegate void RabbitMQCallback(string routingKey, string body);
    }
}
