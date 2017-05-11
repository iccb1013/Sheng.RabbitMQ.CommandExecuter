using Linkup.Common;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.RabbitMQ
{
    public class RabbitMQService
    {
        private static readonly RabbitMQService _instance = new RabbitMQService();
        public static RabbitMQService Instance
        {
            get { return _instance; }
        }

        private bool _started = false;

        private RabbitMQConfig_Root _rabbitMQConfig = null;
        private LogService _logService = LogService.Instance;

        private IModel _sendChannel;
        private IModel _receiveChannel;
        private IBasicProperties _sendProperties;

        private Dictionary<string, List<RabbitMQCallback>> _rottingKeyCallbackList = new Dictionary<string, List<RabbitMQCallback>>();
        private List<RabbitMQCallback> _allMessageCallbackList = new List<RabbitMQCallback>();

        private RabbitMQService()
        {
            try
            {
                _rabbitMQConfig = RabbitMQConfig.GetRabbitMQConfig();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                _logService.Write("加载 RabbitMQ 配置失败", ex.Message, TraceEventType.Error);
            }
        }

        public void Start()
        {
            if (_started)
                return;

            _started = true;

            if (_rabbitMQConfig == null)
            {
                _logService.Write("RabbitMQService.Start 失败，没有加载 RabbitMQ 配置", TraceEventType.Error);
                return;
            }

            _logService.Write("RabbitMQService.Start", TraceEventType.Verbose);

            try
            {

                ConnectionFactory factory = new ConnectionFactory();
                factory.HostName = _rabbitMQConfig.ConnectionFactory.HostName;
                factory.UserName = _rabbitMQConfig.ConnectionFactory.UserName;
                factory.Password = _rabbitMQConfig.ConnectionFactory.Password;

                IConnection connection = factory.CreateConnection();
                connection.CallbackException += Connection_CallbackException;
                connection.ConnectionBlocked += Connection_ConnectionBlocked;
                connection.ConnectionUnblocked += Connection_ConnectionUnblocked;
                connection.ConnectionShutdown += Connection_ConnectionShutdown;

                _sendChannel = connection.CreateModel();
                _sendProperties = _sendChannel.CreateBasicProperties();
                _sendProperties.Persistent = true;

                _receiveChannel = connection.CreateModel();

                foreach (RabbitMQConfig_Exchange exchange in _rabbitMQConfig.ExchangeList.Exchange)
                {
                    _sendChannel.ExchangeDeclare(exchange.Name, exchange.Type);
                    _receiveChannel.ExchangeDeclare(exchange.Name, exchange.Type);

                    foreach (RabbitMQConfig_Queue queue in exchange.QueueList.Queue)
                    {
                        if (queue.Type == "send")
                        {
                            _sendChannel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete, null);
                            _sendChannel.QueueBind(queue.Name, exchange.Name, queue.RoutingKey);
                        }
                        else
                        {
                            _receiveChannel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete, null);
                            _receiveChannel.QueueBind(queue.Name, exchange.Name, queue.RoutingKey);
                        }
                    }
                }

                _receiveChannel.BasicQos(0, 1, false);

                var consumer = new EventingBasicConsumer(_receiveChannel);
                consumer.Received += Consumer_Received;

                foreach (RabbitMQConfig_Exchange exchange in _rabbitMQConfig.ExchangeList.Exchange)
                {
                    foreach (RabbitMQConfig_Queue queue in exchange.QueueList.Queue)
                    {
                        if (queue.Type == "receive")
                        {
                            _receiveChannel.BasicConsume(queue.Name, false, consumer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                _logService.Write("Rabbit MQ 启动失败。", ex.Message, TraceEventType.Error);
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string strBody = Encoding.UTF8.GetString(e.Body);

            _logService.Write("收到MQ消息命令", strBody, TraceEventType.Verbose);

            if (_rottingKeyCallbackList.Count > 0 && _rottingKeyCallbackList.ContainsKey(e.RoutingKey))
            {
                List<RabbitMQCallback> actionList = _rottingKeyCallbackList[e.RoutingKey];

                if (actionList.Count == 0)
                    return;

                foreach (var action in actionList)
                {
                    try
                    {
                        action(e.DeliveryTag, e.RoutingKey, strBody);
                    }
                    catch (Exception ex)
                    {
                        _logService.Write("RabbitMQService 在调用回调方法时异常。", ex.Message, TraceEventType.Error);
                    }
                }
            }

            if (_allMessageCallbackList.Count > 0)
            {
                foreach (var action in _allMessageCallbackList)
                {
                    try
                    {
                        action(e.DeliveryTag, e.RoutingKey, strBody);
                    }
                    catch (Exception ex)
                    {
                        _logService.Write("RabbitMQService 在调用回调方法时异常。", ex.Message, TraceEventType.Error);
                    }
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
            try
            {
                var bytes = Encoding.UTF8.GetBytes(body);
                _sendChannel.BasicPublish(exchangeName, routingKey, _sendProperties, bytes);
            }catch(Exception ex)
            {
                _logService.Write("RabbitMQService.Send 失败", ex.Message, TraceEventType.Error);
            }
        }

        public void Subscribe(RabbitMQCallback callback)
        {
            if (callback == null)
            {
                _logService.Write("RabbitMQService.Subscribe", "callback 无效", TraceEventType.Error);
                throw new ArgumentNullException("callback");
            }

            if (_allMessageCallbackList.Contains(callback) == false)
            {
                lock (_allMessageCallbackList)
                {
                    if (_allMessageCallbackList.Contains(callback) == false)
                    {
                        _allMessageCallbackList.Add(callback);
                    }
                }
            }
        }

        public void Subscribe(string routingKey, RabbitMQCallback callback)
        {
            if (String.IsNullOrEmpty(routingKey))
            {
                _logService.Write("RabbitMQService.Subscribe", "routingKey 无效", TraceEventType.Error);
                throw new ArgumentNullException("routingKey");
            }

            if (callback == null)
            {
                _logService.Write("RabbitMQService.Subscribe", "callback 无效", TraceEventType.Error);
                throw new ArgumentNullException("callback");
            }

            if (_rottingKeyCallbackList.ContainsKey(routingKey) == false)
            {
                lock (_rottingKeyCallbackList)
                {
                    if (_rottingKeyCallbackList.ContainsKey(routingKey) == false)
                    {
                        _rottingKeyCallbackList.Add(routingKey, new List<RabbitMQCallback>());
                    }
                }
            }

            List<RabbitMQCallback> actionList = _rottingKeyCallbackList[routingKey];

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

        public void Ack(ulong deliveryTag, bool multiple)
        {
            try
            {
                _receiveChannel.BasicAck(deliveryTag, multiple);
            }catch(Exception ex)
            {
                _logService.Write("RabbitMQService.Ack 失败", ex.Message, TraceEventType.Error);
            }
        }

        public delegate void RabbitMQCallback(ulong deliveryTag,string routingKey, string body);
    }
}
