using Newtonsoft.Json;
using Sheng.RabbitMQ.CommandExecuter.Contract;
using Sheng.RabbitMQ.CommandExecuter.Core;
using Sheng.RabbitMQ.CommandExecuter.RabbitMQ;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Sheng.RabbitMQ.CommandExecuter.WindowsForm
{
    public partial class Form1 : Form
    {
        RabbitMQService _rabbitMQService = RabbitMQService.Instance;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStartClient_Click(object sender, EventArgs e)
        {
            CommandExecuterService.Instance.Start();
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            _rabbitMQService.Subscribe("routingKey_B", RabbitMQCallback);

            _rabbitMQService.Start();

            MessageBox.Show("RabbitMQService 已启动。");
        }

        private void RabbitMQCallback(ulong deliveryTag, string routingKey, string body)
        {
            _rabbitMQService.Ack(deliveryTag, false);

            Debug.WriteLine(routingKey + Environment.NewLine + body);

            MessageBox.Show(routingKey + Environment.NewLine + body);
        }

        private void btnSendDatabaseSyncCommand_Click(object sender, EventArgs e)
        {
            DatabaseSyncCommand cmd = new DatabaseSyncCommand();

            DatabaseSyncItem item1 = new DatabaseSyncItem()
            {
                Action = DatabaseSyncAction.Add,
                Table = "Customers",
                PrimaryKeyValue = "062B54F5-69AA-A108-09F8-39DB9C2F58C4"
            };

            DatabaseSyncItem item2 = new DatabaseSyncItem()
            {
                Action = DatabaseSyncAction.Update,
                Table = "Customers",
                PrimaryKeyValue = "062B54F5-69AA-A108-09F8-39DB9C2F58C4"
            };

            DatabaseSyncItem item3 = new DatabaseSyncItem()
            {
                Action = DatabaseSyncAction.Delete,
                Table = "Customers",
                PrimaryKeyValue = "062B54F5-69AA-A108-09F8-39DB9C2F58C4"
            };

            cmd.SyncItemList.Add(item1);
            cmd.SyncItemList.Add(item2);
            cmd.SyncItemList.Add(item3);

            string json = JsonConvert.SerializeObject(cmd);

            _rabbitMQService.Send("exchangeName_A", "routingKey_A", json);

            MessageBox.Show("DatabaseSyncCommand 命令已发送。 Command 需要通过 CommandExecuterService 去解析和消费。");
        }

        private void btnSendSimpleString_Click(object sender, EventArgs e)
        {
            _rabbitMQService.Send("exchangeName_A", "routingKey_B", "123");
        }

      
    }
}
