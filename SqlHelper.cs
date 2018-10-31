using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;

namespace SnnuWebService
{
    public class SqlHelper
    {
        //连接字符串
        public static string Conn = "Database='dbs';Data Source = 'localhost'; User Id = 'root'; Password='root';charset='utf8';pooling=true";

        //私有构造函数
        private SqlHelper() { }


        /// <summary> 
        /// 准备执行一个命令 
        /// </summary> 
        /// <param name="cmd">sql命令</param> 
        /// <param name="conn">OleDb连接</param> 
        /// <param name="cmdText">命令文本,例如:Select * from Products</param> 
        /// <param name="cmdParms">执行命令的参数</param> 
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (cmdParms != null)
            {

                foreach (MySqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }


        /// <summary> 
        /// 用指定的数据库连接执行一个命令并返回一个数据集的第一列 
        /// </summary> 
        /// <remarks> 
        /// 例如: 
        /// Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24)); 
        /// </remarks> 
        /// <param name="connection">一个存在的数据库连接</param> 
        /// <param name="cmdType">命令类型(存储过程, 文本, 等等)</param> 
        /// <param name="cmdText">存储过程名称或者sql命令语句</param> 
        /// <param name="commandParameters">执行命令所用参数的集合</param> 
        /// <returns>用 Convert.To{Type}把类型转换为想要的 </returns> 
        public static object ExecuteScalar(string SQLString)
        {
            /*
            MySqlCommand cmd = new MySqlCommand();

            PrepareCommand(cmd, connection, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
            */
            using (MySqlConnection connection = new MySqlConnection(Conn))
            {
                using (MySqlCommand mySqlCommand = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = mySqlCommand.ExecuteScalar();       //执行查询，并返回结果的第一行，其余结果将被忽略
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                            return null;
                        else
                            return obj;
                    }
                    catch (Exception e)
                    {
                        connection.Close();
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 执行查询语句（重载）
        /// </summary>
        /// <param name="SQLString">sql语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>（object类）查询结果</returns>
        public static object ExecuteScalar(string SQLString, MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(Conn))      //使用SQL string初始化connection
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))          //使用cmdText和connection初始化cmd
                {
                    try
                    {
                        connection.Open();
                        PrepareCommand(cmd, connection, SQLString, parameters);
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                            return null;
                        else
                            return obj;
                    }
                    catch (Exception e)
                    {
                        connection.Close();
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// 查看查询语句影响的记录数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteNonQuery(string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(Conn))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        int n = cmd.ExecuteNonQuery();
                        return n;
                    }
                    catch (Exception e)
                    {
                        connection.Close();
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 查看查询语句影响的记录数
        /// </summary>
        /// <param name="SQLString">sql语句</param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(Conn))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        PrepareCommand(cmd, connection, SQLString, cmdParms);
                        object n = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return Convert.ToInt32(n);
                    }
                    catch (Exception e)
                    {
                    }
                    return 0;
                }
            }
        }


        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="SQLStringList">sql语句集</param>
        /// <returns>成功完成事务的数目</returns>
        public static int ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (MySqlConnection conn = new MySqlConnection(Conn))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conn;
                MySqlTransaction tx = conn.BeginTransaction();      //开始事务
                cmd.Transaction = tx;
                int count = 0;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();        //提交事务
                }
                catch (Exception e)
                {
                    tx.Rollback();      //回滚事务
                }
                return count;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="parameters"></param>
        /// <param name="TabName"></param>
        /// <returns></returns>
        public static DataSet Query(string SQLString, MySqlParameter[] parameters, string TabName)
        {
            using (MySqlConnection connection = new MySqlConnection(Conn))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, SQLString, parameters);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))     //一组数据命令和一个数据库连接
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, TabName);
                        cmd.Parameters.Clear();
                    }
                    catch (Exception e)
                    {
                    }
                    return ds;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="TabName"></param>
        /// <returns></returns>
        public static DataSet Query(string SQLString, string TabName)
        {
            using (MySqlConnection connection = new MySqlConnection(Conn))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.Fill(ds, TabName);
                    connection.Close();
                }
                catch (Exception e)
                {
                }
                return ds;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="SQLString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static MySqlDataReader ExecuteReader(string SQLString, params MySqlParameter[] cmdParms)
        {
            MySqlConnection connection = new MySqlConnection(Conn);
            MySqlCommand cmd = new MySqlCommand(SQLString, connection);
            try
            {
                PrepareCommand(cmd, connection, SQLString, cmdParms);
                MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (Exception e)
            {
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public static MySqlDataReader ExecuteReader(string SQLString)
        {
            MySqlConnection connection = new MySqlConnection(Conn);
            MySqlCommand cmd = new MySqlCommand(SQLString, connection);
            try
            {
                connection.Open();
                MySqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (Exception e)
            {
            }
            return null;
        }
    }
}