using Linkup.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareJoy.SilkRoad.RabbitMQ
{
    public class RabbitMQService
    {
        private static readonly RabbitMQService _instance = new RabbitMQService();
        public static RabbitMQService Instance
        {
            get { return _instance; }
        }

        private RabbitMQConfig_Root _rabbitMQConfig = null;
        private LogService _logService = LogService.Instance;

        IModel _channel;
        IBasicProperties _properties;

        private RabbitMQService()
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
                _logService.Write("RabbitMQService.Start 失败，没有加载 RabbitMQ 配置", TraceEventType.Error);
                return;
            }

            _logService.Write("RabbitMQService.Start", TraceEventType.Verbose);

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = _rabbitMQConfig.ConnectionFactory.HostName;
            factory.UserName = _rabbitMQConfig.ConnectionFactory.UserName;
            factory.Password = _rabbitMQConfig.ConnectionFactory.Password;

            IConnection connection = factory.CreateConnection();
            connection.CallbackException += Connection_CallbackException;
            connection.ConnectionBlocked += Connection_ConnectionBlocked;
            connection.ConnectionUnblocked += Connection_ConnectionUnblocked;
            connection.ConnectionShutdown += Connection_ConnectionShutdown;

            _channel = connection.CreateModel();
            _properties = _channel.CreateBasicProperties();
            _properties.Persistent = true;

            foreach (RabbitMQConfig_Exchange exchange in _rabbitMQConfig.ExchangeList.Exchange)
            {
                _channel.ExchangeDeclare(exchange.Name, exchange.Type);

                foreach (RabbitMQConfig_Queue queue in exchange.QueueList.Queue)
                {
                    _channel.QueueDeclare(queue.Name, true, false, false, null);
                    _channel.QueueBind(queue.Name, exchange.Name, queue.RoutingKey);
                }
            }
        }

        #region ConnectionEvent

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            _logService.Write("RabbitMQService.Connection_ConnectionShutdown", TraceEventType.Verbose);
        }

        private void Connection_ConnectionUnblocked(object sender, EventArgs e)
        {
            _logService.Write("RabbitMQService.Connection_ConnectionUnblocked", TraceEventType.Verbose);
        }

        private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            _logService.Write("RabbitMQService.Connection_ConnectionBlocked", TraceEventType.Verbose);
        }

        private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            _logService.Write("RabbitMQService.Connection_CallbackException", TraceEventType.Verbose);
        }

        #endregion

        public void Send(string exchangeName, string routingKey, string body)
        {
            var bytes = Encoding.UTF8.GetBytes(body);
            _channel.BasicPublish(exchangeName, routingKey, _properties, bytes);
        }
    }
}
