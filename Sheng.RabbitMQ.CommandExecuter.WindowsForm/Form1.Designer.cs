namespace Sheng.RabbitMQ.CommandExecuter.WindowsForm
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSendDatabaseSyncCommand = new System.Windows.Forms.Button();
            this.btnStartClient = new System.Windows.Forms.Button();
            this.btnStartService = new System.Windows.Forms.Button();
            this.btnSendSimpleString = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSendDatabaseSyncCommand
            // 
            this.btnSendDatabaseSyncCommand.Location = new System.Drawing.Point(14, 313);
            this.btnSendDatabaseSyncCommand.Name = "btnSendDatabaseSyncCommand";
            this.btnSendDatabaseSyncCommand.Size = new System.Drawing.Size(195, 23);
            this.btnSendDatabaseSyncCommand.TabIndex = 1;
            this.btnSendDatabaseSyncCommand.Text = "SendDatabaseSyncCommand";
            this.btnSendDatabaseSyncCommand.UseVisualStyleBackColor = true;
            this.btnSendDatabaseSyncCommand.Click += new System.EventHandler(this.btnSendDatabaseSyncCommand_Click);
            // 
            // btnStartClient
            // 
            this.btnStartClient.Location = new System.Drawing.Point(12, 12);
            this.btnStartClient.Name = "btnStartClient";
            this.btnStartClient.Size = new System.Drawing.Size(195, 23);
            this.btnStartClient.TabIndex = 2;
            this.btnStartClient.Text = "CommandExecuterService";
            this.btnStartClient.UseVisualStyleBackColor = true;
            this.btnStartClient.Click += new System.EventHandler(this.btnStartClient_Click);
            // 
            // btnStartService
            // 
            this.btnStartService.Location = new System.Drawing.Point(12, 155);
            this.btnStartService.Name = "btnStartService";
            this.btnStartService.Size = new System.Drawing.Size(195, 23);
            this.btnStartService.TabIndex = 3;
            this.btnStartService.Text = "Start RabbitMQService";
            this.btnStartService.UseVisualStyleBackColor = true;
            this.btnStartService.Click += new System.EventHandler(this.btnStartService_Click);
            // 
            // btnSendSimpleString
            // 
            this.btnSendSimpleString.Location = new System.Drawing.Point(12, 243);
            this.btnSendSimpleString.Name = "btnSendSimpleString";
            this.btnSendSimpleString.Size = new System.Drawing.Size(195, 23);
            this.btnSendSimpleString.TabIndex = 4;
            this.btnSendSimpleString.Text = "SendSimpleString";
            this.btnSendSimpleString.UseVisualStyleBackColor = true;
            this.btnSendSimpleString.Click += new System.EventHandler(this.btnSendSimpleString_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(402, 53);
            this.label1.TabIndex = 5;
            this.label1.Text = "初始化一个CommandExecuterService，监听配置文件中所定义的队列的消息。并视MQ队列中的所有消息均是派生自Command的类型，尝试解析并执行这" +
    "些Command。";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(402, 45);
            this.label2.TabIndex = 6;
            this.label2.Text = "启动一个RabbitMQService对象，这是一个基本的RabbitMQ的封装，它通过配置文件声明和初始化队列，随后可通过RabbitMQService提供的方" +
    "法订阅消息或发送消息。";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(402, 26);
            this.label3.TabIndex = 7;
            this.label3.Text = "使用上面初始化的RabbitMQService发送一个简单的字符串。";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 349);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(402, 26);
            this.label4.TabIndex = 8;
            this.label4.Text = "使用上面初始化的RabbitMQService发送一个 DatabaseSyncCommand（数据同步命令）";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 431);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSendSimpleString);
            this.Controls.Add(this.btnStartService);
            this.Controls.Add(this.btnStartClient);
            this.Controls.Add(this.btnSendDatabaseSyncCommand);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSendDatabaseSyncCommand;
        private System.Windows.Forms.Button btnStartClient;
        private System.Windows.Forms.Button btnStartService;
        private System.Windows.Forms.Button btnSendSimpleString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

