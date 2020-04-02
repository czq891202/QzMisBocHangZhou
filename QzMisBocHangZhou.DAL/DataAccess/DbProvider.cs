using System;
using System.Data;
using System.Data.Common;

namespace QzMisBocHangZhou.DAL
{
    class DbProvider<DbConnect> where DbConnect : DbConnection, new()
    {
        private DbProviderFactory m_Factory;
        private readonly int m_TimeOut = 600;
        private readonly CommandType m_DefaultCmdType = CommandType.Text;

        private bool m_IsOracle = false;

        public DbProvider()
        {
            if (typeof(DbConnect).Name.Contains("Oracle")) m_IsOracle = true;
            using (var conn = new DbConnect())
            {
                m_Factory = DbProviderFactories.GetFactory(conn);
            }
        }


        public DbConnection CreateConnection(string conStr)
        {
            var conn = m_Factory.CreateConnection();
            conn.ConnectionString = conStr;
            return conn;
        }


        public DbCommand CreateCommond(DbConnection connection)
        {
            if (m_IsOracle)
            {
                var cmd = connection.CreateCommand() as Oracle.ManagedDataAccess.Client.OracleCommand;
                cmd.BindByName = true;

                return cmd;
            }
            else
            {
                return connection.CreateCommand();
            }
        }


        public DbTransaction CreateTransaction(DbConnection connection)
        {
            if (connection.State != ConnectionState.Open) connection.Open();
            return connection.BeginTransaction();
        }


        public DbDataAdapter CreateDataAdapter(DbCommand command)
        {
            var adapter = m_Factory.CreateDataAdapter();
            adapter.SelectCommand = command;
            return adapter;
        }


        public DbParameter CreateParameter()
        {
            return m_Factory.CreateParameter();
        }


        public void PrepareCommand(DbCommand command)
        {
            PrepareCommand(command, null, m_DefaultCmdType, string.Empty, null);
        }


        public void PrepareCommand(DbCommand command, DbTransaction transaction)
        {
            PrepareCommand(command, transaction, m_DefaultCmdType, string.Empty, null);
        }


        public void PrepareCommand(DbCommand command, DbTransaction transaction, CommandType commandType, string commandText, params DbParameter[] parms)
        {
            if (command.Connection.State != ConnectionState.Open) command.Connection.Open();

            command.CommandTimeout = m_TimeOut;
            command.CommandType = commandType;
            command.CommandText = commandText;
            if (transaction != null) command.Transaction = transaction;

            command.Parameters.Clear();
            parms = PrepareCommandParms(parms);
            if (parms == null || parms.Length == 0) return;

            
            command.Parameters.AddRange(parms);
        }

        public DbParameter[] PrepareCommandParms(params DbParameter[] parms)
        {
            if (parms == null || parms.Length == 0) return null;

            foreach (var parameter in parms)
            {
                if ((parameter.Direction == ParameterDirection.InputOutput ||
                    parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                {
                    parameter.Value = DBNull.Value;
                }
            }

            return parms;
        }
    }
}
