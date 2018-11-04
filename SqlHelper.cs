using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Data;
using System.Text;

namespace SnnuWebService
{
    /// <summary>  
    /// SqlServer数据访问帮助类  
    /// </summary>  
    public sealed class SqlHelper
    {

        #region 私有构造函数和方法结束  
        private SqlHelper() { }

        /// <summary>
        /// 将MySqlParameter参数数组(参数值)分配给MySqlCommand命令.  
        /// 这个方法将给任何一个参数分配DBNull.Value;  
        /// 该操作将阻止默认值的使用. 
        /// </summary>
        /// <param name="command">命令名</param>
        /// <param name="commandParameters">MySqlParameters数组</param>
        private static void AttachParameters(MySqlCommand command, MySqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (MySqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // 检查未分配值的输出参数,将其分配以DBNull.Value.  
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary> 
        /// 预处理用户提供的命令,数据库连接/命令类型/参数  
        /// </summary> 
        /// <param name="cmd">要处理的MySqlCommand</param> 
        /// <param name="conn">数据库连接<</param> 
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="cmdText">存储过程名或都T-SQL命令文本</param> 
        /// <param name="cmdParms">和命令相关联的MySqlParameter参数数组,如果没有参数为’null’</param> 
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, CommandType commandType, string cmdText, MySqlParameter[] cmdParms)
        {
            if (cmd == null) throw new ArgumentNullException("command");
            if (cmdText == null || cmdText.Length == 0) throw new ArgumentNullException("commandText");
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (cmdParms != null)
            {
                AttachParameters(cmd, cmdParms);
            }
            return;
        }
        #endregion

        #region  ExecuteNonQuery命令
        /// <summary>  
        /// 执行指定连接字符串,类型的MySqlCommand.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders");  
        /// </remarks>  
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <returns>返回命令影响的行数</returns> 
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, (MySqlParameter[])null);
        }

        /// <summary>  
        /// 执行指定连接字符串,类型的MySqlCommand.如果没有提供参数,不返回结果.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));  
        /// </remarks>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <param name="commandParameters">MySqlParameter参数数组</param>
        /// <returns>返回命令影响的行数</returns>  
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>  
        /// 执行指定数据库连接对象的命令   
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders");  
        /// </remarks>  
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <returns>返回影响的行数</returns>  
        public static int ExecuteNonQuery(MySqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, (MySqlParameter[])null);
        }

        /// <summary>  
        /// 执行指定数据库连接对象的命令  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));  
        /// </remarks>  
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="commandType">命令类型 (存储过程,命令文本, 其它.)</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <param name="commandParameters">MySqlParameter参数数组</param>
        /// <returns>返回影响的行数</returns>  
        public static int ExecuteNonQuery(MySqlConnection connection, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            // 创建SqlCommand命令,并进行预处理  
            MySqlCommand cmd = new MySqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection,  commandType, commandText, commandParameters);

            // Finally, execute the command  
            int retval = cmd.ExecuteNonQuery();

            // 清除参数,以便再次使用.  
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }
        #endregion

        #region 返回结果集中的第一行第一列
        /// <summary>  
        /// 执行指定数据库连接字符串的命令,返回结果集中的第一行第一列.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount");  
        /// </remarks>  
        /// <param name="connectionString">一个有效的数据库连接字符串</param>  
        /// <param name="commandType"> 命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText"param>存储过程名称或T-SQL语句</param>  
        /// <returns>返回结果集中的第一行第一列</returns>  
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法  
            return ExecuteScalar(connectionString, commandType, commandText, (MySqlParameter[])null);
        }

        /// <summary>  
        /// 执行指定数据库连接字符串的命令,指定参数,返回结果集中的第一行第一列.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  int orderCount = (int)ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));  
        /// </remarks>  
        /// <param name="connectionString">一个有效的数据库连接字符串</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名称或T-SQL语句</param>  
        /// <param name="commandParameters">分配给命令的SqlParamter参数数组</param>  
        /// <returns>返回结果集中的第一行第一列</returns>  
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            // 创建并打开数据库连接对象,操作完成释放对象.  
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // 调用指定数据库连接字符串重载方法.  
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>  
        /// 执行指定数据库连接对象的命令,返回结果集中的第一行第一列.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");  
        /// </remarks>  
        /// <param name="connection">一个有效的数据库连接对象</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名称或T-SQL语句</param>  
        /// <returns>返回结果集中的第一行第一列</returns> 
        public static object ExecuteScalar(MySqlConnection connection, CommandType commandType, string commandText)
        {
            // 执行参数为空的方法  
            return ExecuteScalar(connection, commandType, commandText, (MySqlParameter[])null);
        }

        /// <summary>  
        /// 执行指定数据库连接对象的命令,指定参数,返回结果集中的第一行第一列.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));  
        /// </remarks>  
        /// <param name="connection">一个有效的数据库连接对象</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名称或T-SQL语句</param>  
        /// <param name="commandParameters">分配给命令的MySqlParamter参数数组</param>  
        /// <returns>返回结果集中的第一行第一列</returns>  
        public static object ExecuteScalar(MySqlConnection connection, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            // 创建SqlCommand命令,并进行预处理  
            MySqlCommand cmd = new MySqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, commandType, commandText, commandParameters);

            // 执行SqlCommand命令,并返回结果.  
            object retval = cmd.ExecuteScalar();

            // 清除参数,以便再次使用.  
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }
        #endregion 返回结果集中的第一行第一列    

        #region ExecuteReader 数据阅读器
        /// <summary>  
        /// 枚举,标识数据库连接是由SqlHelper提供还是由调用者提供  
        /// </summary> 
        private enum SqlConnectionOwnership
        {
            /// <summary>由SqlHelper提供连接</summary>  
            Internal,
            /// <summary>由调用者提供连接</summary>  
            External
        }
        /// <summary>  
        /// 执行指定数据库连接对象的数据阅读器.  
        /// </summary>  
        /// <remarks>  
        /// 如果是SqlHelper打开连接,当连接关闭DataReader也将关闭.  
        /// 如果是调用都打开连接,DataReader由调用都管理.  
        /// </remarks>  
        /// <param name="connection">一个有效的数据库连接对象</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名或T-SQL语句</param>  
        /// <param name="commandParameters">MySqlParameters参数数组,如果没有参数则为’null’</param>  
        /// <param name="connectionOwnership">标识数据库连接对象是由调用者提供还是由SqlHelper提供</param>  
        /// <returns>返回包含结果集的MySqlDataReader</returns>  
        private static MySqlDataReader ExecuteReader(MySqlConnection connection,CommandType commandType, string commandText, MySqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            bool mustCloseConnection = false;
            // 创建命令  
            MySqlCommand cmd = new MySqlCommand();
            try
            {
                PrepareCommand(cmd, connection,commandType, commandText, commandParameters);

                // 创建数据阅读器  
                MySqlDataReader dataReader;
                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                // 清除参数,以便再次使用..  
                // HACK: There is a problem here, the output parameter values are fletched   
                // when the reader is closed, so if the parameters are detached from the command  
                // then the SqlReader can磘 set its values.   
                // When this happen, the parameters can磘 be used again in other command.  
                bool canClear = true;
                foreach (MySqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                        canClear = false;
                }

                if (canClear)
                {
                    cmd.Parameters.Clear();
                }
                return dataReader;
            }
            catch
            {
                if (mustCloseConnection)
                    connection.Close();
                throw;
            }
        }

        /// <summary>  
        /// 执行指定数据库连接字符串的数据阅读器.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders");  
        /// </remarks>  
        /// <param name="connectionString">一个有效的数据库连接字符串</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名或T-SQL语句</param>  
        /// <returns>返回包含结果集的MySqlDataReader</returns>  
        public static MySqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, (MySqlParameter[])null);
        }

        /// <summary>  
        /// 执行指定数据库连接字符串的数据阅读器,指定参数.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  SqlDataReader dr = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));  
        /// </remarks>  
        /// <param name="connectionString">一个有效的数据库连接字符串</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名或T-SQL语句</param>  
        /// <param name="commandParameters">MySqlParamter参数数组(new SqlParameter("@prodid", 24))</param>  
        /// <returns>返回包含结果集的MySqlDataReader</returns>  
        public static MySqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
                return ExecuteReader(connection, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch
            {
                // If we fail to return the SqlDatReader, we need to close the connection ourselves  
                if (connection != null) connection.Close();
                throw;
            }

        }
        #endregion

        #region ExecuteDataset方法
        /// <summary>  
        /// 执行指定数据库连接字符串的命令,返回DataSet.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders");  
        /// </remarks>  
        /// <param name="connectionString">一个有效的数据库连接字符串</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名称或T-SQL语句</param>  
        /// <returns>返回一个包含结果集的DataSet</returns>  
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, (MySqlParameter[])null);
        }

        /// <summary>  
        /// 执行指定数据库连接字符串的命令,返回DataSet.  
        /// </summary>  
        /// <remarks>  
        /// 示例:   
        ///  DataSet ds = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));  
        /// </remarks>  
        /// <param name="connectionString">一个有效的数据库连接字符串</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名称或T-SQL语句</param>  
        /// <param name="commandParameters">MySqlParamters参数数组</param>  
        /// <returns>返回一个包含结果集的DataSet</returns>  
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
            // 创建并打开数据库连接对象,操作完成释放对象.  
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                // 调用指定数据库连接字符串重载方法.  
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
        }

        /// <summary>  
        /// 执行指定数据库连接对象的命令,返回DataSet.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");  
        /// </remarks>  
        /// <param name="connection">一个有效的数据库连接对象</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名或T-SQL语句</param>  
        /// <returns>返回一个包含结果集的DataSet</returns>  
        public static DataSet ExecuteDataset(MySqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, (MySqlParameter[])null);
        }

        /// <summary>  
        /// 执行指定数据库连接对象的命令,指定存储过程参数,返回DataSet.  
        /// </summary>  
        /// <remarks>  
        /// 示例:    
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new MySqlParameter("@prodid", 24));  
        /// </remarks>  
        /// <param name="connection">一个有效的数据库连接对象</param>  
        /// <param name="commandType">命令类型 (存储过程,命令文本或其它)</param>  
        /// <param name="commandText">存储过程名或T-SQL语句</param>  
        /// <param name="commandParameters">MySqlParamter参数数组</param>  
        /// <returns>返回一个包含结果集的DataSet</returns>  
        public static DataSet ExecuteDataset(MySqlConnection connection, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            // 预处理  
            MySqlCommand cmd = new MySqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection,commandType, commandText, commandParameters);

            // 创建SqlDataAdapter和DataSet.  
            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                // 填充DataSet.  
                da.Fill(ds);

                cmd.Parameters.Clear();
                if (mustCloseConnection)
                    connection.Close();
                return ds;
            }
        }
        #endregion

    }
}