Sheng.RabbitMQ.CommandExecuter 是使用 .Net 对 RabbitMQ 的一个简单封装。

它通过XML配置文件定义Exchange及队列等信息，根据此配置文件自动声明及初始化相关队列信息，方便 .Net 开发人员使用 RabbitMQ。

并实现了一个基于 MQ 的命令执行器，将 MQ 消息抽象化为命令，发布端和订阅端通过命令进行交互。默认实现了两个命令：
1）HTTP请求转发，将收到的MQ消息的指定内容转发到指定URL上；
2）数据库同步，通过预先定义的配置文件，指明不同数据库和表之间的关联关系，发送端向 MQ 中发布数据库同步命令后，订阅方（可作为 windows 服务部署，已在工程中实现）负责解析并执行数据库同步工作。

你可以直接使用基本的 RabbitMQ 封装，也可以在此命令模式的基础上实现你自己的命令。

详细说明请浏览：
http://sheng.city/post/sheng-rabbitmq-commandexecuter
