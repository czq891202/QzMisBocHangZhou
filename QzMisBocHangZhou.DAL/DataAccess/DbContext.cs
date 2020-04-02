using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace QzMisBocHangZhou.DAL
{
    public class DbContext<DbConnect> where DbConnect : DbConnection, new()
    {
        private readonly string m_ConnectString;
        private DbProvider<DbConnect> m_Provider;


        /// <summary>
        /// construct 
        /// </summary>
        /// <example>
        /// SqlServer: "server=139.196.224.238;database=xxx;uid=xxx;pwd=xxxx."
        /// Oracle: "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=ORCL)));User Id=scott;Password=1234pass;"
        /// </example>
        /// <param name="connectionStr"></param>
        public DbContext(string connectionStr)
        {
            if (string.IsNullOrWhiteSpace(connectionStr)) throw new ArgumentNullException(nameof(connectionStr));

            this.m_ConnectString = connectionStr;
            this.m_Provider = new DbProvider<DbConnect>();
        }


        #region 【ExecuteNonQuery】
        public int ExecuteNonQuery(string commandText, params DbParameter[] parms)
        {
            return ExecuteNonQuery(commandText, CommandType.Text, parms);
        }


        public int ExecuteNonQuery(string commandText, CommandType cmdType, params DbParameter[] parms)
        {
            return ExecuteDb(commandText, cmdType,
                (command) => command.ExecuteNonQuery(), parms);
        }


        public int ExecuteNonQuery(DbCommand cmd, string commandText, params DbParameter[] parms)
        {
            return ExecuteDb(cmd, commandText,
                (command) => command.ExecuteNonQuery(), parms);
        }


        public int ExecuteNonQuery(Func<DbCommand, int> fun)
        {
            if (fun == null) throw new ArgumentNullException(nameof(fun), "Sql执行代理不可为Null !");

            using (var connection = m_Provider.CreateConnection(this.m_ConnectString))
            {
                var trans = m_Provider.CreateTransaction(connection);
                using (var command = m_Provider.CreateCommond(connection))
                {
                    m_Provider.PrepareCommand(command, trans);

                    var retval = 0;
                    try
                    {
                        retval = fun(command);
                        trans.Commit();
                    }
                    catch
                    {
                        try
                        {
                            trans.Rollback();
                        }
                        catch { }
                        throw;
                    }
                    return retval;
                }
            }
        }

        #endregion


        #region 【ExecuteScalar】
        public TResult ExecuteScalar<TResult>(string commandText, params DbParameter[] parms)
        {
            return ExecuteScalar<TResult>(commandText, CommandType.Text, parms);
        }


        public TResult ExecuteScalar<TResult>(string commandText, CommandType cmdType, params DbParameter[] parms)
        {
            return ExecuteDb(commandText, cmdType,
                (command) =>
                {
                    object retval = command.ExecuteScalar();
                    return ConvertEx.To<TResult>(retval);
                }, parms);
        }


        public TResult ExecuteScalar<TResult>(DbCommand cmd, string commandText, params DbParameter[] parms)
        {
            return ExecuteDb(cmd, commandText,
                (command) => ConvertEx.To<TResult>(command.ExecuteScalar()), parms);
        }

        #endregion


        #region【ExecuteDataSet】
        public DataSet ExecuteDataSet(string cmdText, params DbParameter[] parms)
        {
            return ExecuteDataSet(cmdText, CommandType.Text, parms);
        }


        public DataSet ExecuteDataSet(string commandText, CommandType cmdType, params DbParameter[] parms)
        {
            return ExecuteDb(commandText, cmdType,
                (command) =>
                {
                    using (var adapter = m_Provider.CreateDataAdapter(command))
                    {
                        var data = new DataSet();
                        adapter.Fill(data);

                        return data;
                    }
                }, parms);
        }

        #endregion


        #region 【ExecuteDataTable】
        public DataTable ExecuteDataTable(string commandText, params DbParameter[] parms)
        {
            return ExecuteDataTable(commandText, CommandType.Text, parms);
        }


        public DataTable ExecuteDataTable(string commandText, CommandType cmdType, params DbParameter[] parms)
        {
            return ExecuteDb(commandText, cmdType,
                (command) =>
                {
                    using (var adapter = m_Provider.CreateDataAdapter(command))
                    {
                        var data = new DataTable();
                        adapter.Fill(data);

                        return data;
                    }
                }, parms);
        }


        #endregion


        #region 【ExecuteEntities】
        public TResult ExecuteEntity<TResult>(string commandText, params DbParameter[] parms) where TResult : new()
        {
            return ExecuteEntityList<TResult>(commandText, CommandType.Text, parms).FirstOrDefault();
        }

        public List<TResult> ExecuteEntityList<TResult>(string commandText, params DbParameter[] parms) where TResult : new()
        {
            return ExecuteEntityList<TResult>(commandText, CommandType.Text, parms);
        }


        public List<TResult> ExecuteEntityList<TResult>(string commandText, CommandType cmdType = CommandType.Text, params DbParameter[] parms) where TResult : new()
        {
            return ExecuteDb(commandText, cmdType,
                (command) =>
                {
                    using (var dbReader = command.ExecuteReader())
                    {
                        return SqlMapper.Parse<TResult>(dbReader).ToList();
                    }
                }, parms);
        }


        public List<TResult> ExecuteEntityListByPageing<TResult>(int page, int limit, string commandText, params DbParameter[] parms) where TResult : new()
        {
            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentNullException(nameof(commandText), "CommandText内容不可为Null !");

            var pageSql = @" SELECT * FROM 
                                (SELECT tt.*, ROWNUM AS rowno FROM ( {0} ) tt 
                                 WHERE ROWNUM <= :SysMaxRowNum) table_alias
                             WHERE table_alias.rowno >= :SysMinRowNum";

            pageSql = string.Format(pageSql, commandText);
            var listPars = new List<DbParameter>();
            if (parms != null && parms.Length > 0) listPars.AddRange(parms);
            listPars.Add(DBCache.DataBase.CreatDbParameter("SysMaxRowNum", page * limit));
            listPars.Add(DBCache.DataBase.CreatDbParameter("SysMinRowNum", (page - 1) * limit + 1));

            return ExecuteEntityList<TResult>(pageSql, listPars.ToArray());
        }


        public int GetRecordCount(string commandText, params DbParameter[] parms)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentNullException(nameof(commandText), "CommandText内容不可为Null !");

            commandText = $"select count(1) from ({commandText})";

            return ExecuteScalar<int>(commandText, parms);
        }



        #endregion


        #region 【CreatDbParameter】


        public DbParameter CreatDbParameter(string name, object value)
        {
            return CreatDbParameter(name, value, null, null);
        }


        public DbParameter CreatDbParameter(string name, object value, DbType type)
        {
            return CreatDbParameter(name, value, type, null);
        }


        public DbParameter CreatDbParameter(string name, object value, ParameterDirection direction)
        {
            return CreatDbParameter(name, value, null, direction);
        }


        public DbParameter CreatDbParameter(string name, object value, DbType? type, ParameterDirection? direction)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var para = this.m_Provider.CreateParameter();
            para.ParameterName = name;
            para.Value = value ?? DBNull.Value;
            if (type.HasValue) para.DbType = type.Value;
            if (direction.HasValue) para.Direction = direction.Value;

            return para;
        }


        #endregion


        private TResult ExecuteDb<TResult>(string commandText, CommandType cmdType, Func<DbCommand, TResult> execFunc, params DbParameter[] parms)
        {
            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentNullException(nameof(commandText), "CommandText内容不可为Null !");

            if (execFunc == null) throw new ArgumentNullException(nameof(execFunc), "Sql执行对象不可为空 !");


            using (var connection = m_Provider.CreateConnection(this.m_ConnectString))
            {
                using (var command = m_Provider.CreateCommond(connection))
                {
                    return ExecuteDb(command, commandText, execFunc, parms);
                }
            }
        }


        private TResult ExecuteDb<TResult>(DbCommand command, string commandText, Func<DbCommand, TResult> execFunc, params DbParameter[] parms)
        {
            if (command == null) throw new ArgumentNullException(nameof(command), "Command对象不可为Null !");
            if (execFunc == null) throw new ArgumentNullException(nameof(execFunc), "Sql执行对象不可为空 !");

            if (string.IsNullOrWhiteSpace(commandText))
                throw new ArgumentNullException(nameof(commandText), "CommandText内容不可为Null !");

            m_Provider.PrepareCommand(command, null, CommandType.Text, commandText, parms);
            return execFunc(command);
        }
    }
}