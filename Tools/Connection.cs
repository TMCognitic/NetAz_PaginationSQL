using System.Data.Common;
using System.Data;

namespace Tools
{
    public class Connection
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _factory;

        public Connection(DbProviderFactory factory, string connectionString)
        {
            _connectionString = connectionString;
            _factory = factory;

            using (DbConnection dbConnection = CreateConnection())
            {
                dbConnection.Open();
            }
        }

        public object? ExecuteScalar(Command command)
        {
            using (DbConnection dbConnection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(dbConnection, command))
                {
                    dbConnection.Open();
                    object? result = dbCommand.ExecuteScalar();
                    SyncParameter(command, dbCommand);
                    return result is DBNull ? null : result;
                }
            }
        }       

        public int ExecuteNonQuery(Command command)
        {
            using (DbConnection dbConnection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(dbConnection, command))
                {
                    dbConnection.Open();
                    int rows = dbCommand.ExecuteNonQuery();
                    SyncParameter(command, dbCommand);
                    return rows;
                }
            }
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector, bool immediately)
        {
            return immediately ? ExecuteReader(command, selector).ToList() : ExecuteReader(command, selector);
        }

        public IEnumerable<TResult> ExecuteReader<TResult>(Command command, Func<IDataRecord, TResult> selector)
        {
            using (DbConnection dbConnection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(dbConnection, command))
                {
                    dbConnection.Open();
                    using (DbDataReader reader = dbCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return selector(reader);
                        }
                    }

                    SyncParameter(command, dbCommand);
                }
            }
        }

        public DataTable GetDataTable(Command command)
        {
            using (DbConnection dbConnection = CreateConnection())
            {
                using (DbCommand dbCommand = CreateCommand(dbConnection, command))
                {
                    using (DbDataAdapter? dbDataAdapter = _factory.CreateDataAdapter())
                    {
                        if (dbDataAdapter is null)
                            throw new InvalidOperationException();

                        dbDataAdapter.SelectCommand = dbCommand;
                        DataTable dataTable = new DataTable();
                        dbDataAdapter.Fill(dataTable);
                        SyncParameter(command, dbCommand);
                        return dataTable;
                    }
                }
            }
        }

        private DbCommand CreateCommand(DbConnection dbConnection, Command command)
        {
            DbCommand dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = command.Query;
            if (command.IsStoredProcedure)
                dbCommand.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, Parameter> kvp in command.Parameters)
            {
                Parameter parameter = kvp.Value;
                
                DbParameter dbParameter = dbCommand.CreateParameter();
                dbParameter.ParameterName = kvp.Key;
                dbParameter.Value = parameter.Value;
                
                if(parameter.Direction == Direction.Output)
                {
                    dbParameter.Direction = ParameterDirection.InputOutput;
                }

                dbCommand.Parameters.Add(dbParameter);
            }

            return dbCommand;
        }

        private DbConnection CreateConnection()
        {
            DbConnection? dbConnection = _factory.CreateConnection();

            if (dbConnection is null)
                throw new InvalidOperationException();

            dbConnection.ConnectionString = _connectionString;
            return dbConnection;
        }

        private void SyncParameter(Command command, DbCommand dbCommand)
        {
            foreach (DbParameter dbParameter in dbCommand.Parameters)
            {
                if (dbParameter.Direction == ParameterDirection.InputOutput)
                {
                    command[dbParameter.ParameterName] = dbParameter.Value;
                }
            }
        }
    }
}