using Linkup.Data;
using Linkup.DataRelationalMapping;
using Newtonsoft.Json;
using Sheng.RabbitMQ.CommandExecuter.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.RabbitMQ.CommandExecuter.Core
{
    class DatabaseSyncCommandReceiver : CommandReceiver
    {
        private DatabaseSyncConfig_Root _databaseSyncConfig = DatabaseSyncConfig.GetDatabaseSyncConfig();
        private Dictionary<string, DatabaseWrapper> _dataBaseList = new Dictionary<string, DatabaseWrapper>();

        public DatabaseSyncCommandReceiver()
        {
            base.CommandType = DatabaseSyncCommand.CommandTypeName;

            InitDatabase();
        }

        private void InitDatabase()
        {
            foreach (var connection in _databaseSyncConfig.ConnectionList.Connection)
            {
                DatabaseWrapper database = new DatabaseWrapper(connection.ConnectionString, true);
                _dataBaseList.Add(connection.Name, database);
            }
        }

        public override void Handle(string routingKey, string strCommand)
        {
            DatabaseSyncCommand command = JsonConvert.DeserializeObject<DatabaseSyncCommand>(strCommand);

            foreach (DatabaseSyncConfig_Consumer consumer in _databaseSyncConfig.ConfigList.ConsumerList)
            {
                Consume(routingKey, consumer, command);
            }
        }

        private void Consume(string routingKey, DatabaseSyncConfig_Consumer consumer, DatabaseSyncCommand command)
        {
            List<DatabaseSyncConfig_Producer> producerList = (from c in consumer.ProducerList.Producer
                                                              where c.RoutingKey == routingKey
                                                              select c).ToList();

            foreach (var producer in producerList)
            {
                Consume(consumer, producer, command);
            }
        }

        private void Consume(DatabaseSyncConfig_Consumer consumer, DatabaseSyncConfig_Producer producer, DatabaseSyncCommand command)
        {
            DatabaseWrapper consumerDatabase = _dataBaseList[consumer.Connection];
            DatabaseWrapper producerDatabase = _dataBaseList[producer.Connection];

            List<SqlExpression> sqlExpressionList = new List<SqlExpression>();

            foreach (DatabaseSyncItem syncItem in command.SyncItemList)
            {
                List<DatabaseSyncConfig_Table> tableList = (from c in producer.TableDefinition.TableList
                                                            where c.Name == syncItem.Table
                                                            select c).ToList();

                foreach (DatabaseSyncConfig_Table table in tableList)
                {
                    List<CommandParameter> parameterList = new List<CommandParameter>();
                    parameterList.Add(new CommandParameter("@primaryKeyValue", syncItem.PrimaryKeyValue));

                    DataSet dataSet = producerDatabase.ExecuteDataSet(
                        $"SELECT * FROM [{table.Name}] WHERE [{table.PrimaryKey}] = @primaryKeyValue",
                       parameterList, new string[] { table.Name });

                    if (dataSet.Tables[0].Rows.Count == 0)
                        continue;

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        SqlStructureBuild sqlStructureBuild = new SqlStructureBuild();
                        sqlStructureBuild.Table = table.ConsumerTable;

                        switch (syncItem.Action)
                        {
                            case DatabaseSyncAction.Add:
                                sqlStructureBuild.Type = SqlExpressionType.Insert;
                                break;
                            case DatabaseSyncAction.Update:
                                sqlStructureBuild.Type = SqlExpressionType.Update;
                                break;
                            case DatabaseSyncAction.Delete:
                                sqlStructureBuild.Type = SqlExpressionType.Delete;
                                break;
                            default:
                                break;
                        }

                        foreach (DatabaseSyncConfig_Field field in table.Field)
                        {
                            sqlStructureBuild.AddParameter(field.ConsumerField, row[field.Name],
                                field.ConsumerField == table.ConsumerTablePrimaryKey);
                        }

                        SqlExpression sqlExpression = sqlStructureBuild.GetSqlExpression();
                        sqlExpressionList.Add(sqlExpression);
                    }
                }
            }

            consumerDatabase.ExcuteSqlExpression(sqlExpressionList);

        }

    }
}
