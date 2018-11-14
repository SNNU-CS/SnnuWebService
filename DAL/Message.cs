﻿using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Text;
using System.Data;

namespace SnnuWebService.DAL
{
    public class Message
    {
       private string Conn                                                                                                                                                                                                                                                                                                                                                              
            =ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;

        public MySqlDataReader AllDep()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Department from Message");
            return SqlHelper.ExecuteReader(Conn,CommandType.Text, strSql.ToString());
        }
        
        public MySqlDataReader QueryByDate(DateTime start,DateTime end,string type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Message");
            strSql.Append(" where Type='" + type + "' ");
            strSql.Append("and Date between '"+start.ToString("yyyy-MM-dd") +"' and '");
            strSql.Append(end.ToString("yyyy-MM-dd") + "'");
            return SqlHelper.ExecuteReader(Conn,CommandType.Text, strSql.ToString());
        }
        public MySqlDataReader QueryByDateAndDep(DateTime start, DateTime end, string dep,string type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Message");
            strSql.Append(" where Type='" + type + "' ");
            strSql.Append("and " + "Department='" + dep + "' ");
            strSql.Append("and Date between '" + start.ToString("yyyy-MM-dd") + "' and '");
            strSql.Append(end.ToString("yyyy-MM-dd") + "'");
            return SqlHelper.ExecuteReader(Conn, CommandType.Text, strSql.ToString());
        }
        public MySqlDataReader QueryByLikeTitle(string keyworld,string type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from Message");
            strSql.Append(" where Type='" + type + "' ");
            strSql.Append("and title like '%" + keyworld + "%'");
            return SqlHelper.ExecuteReader(Conn, CommandType.Text, strSql.ToString());
        }
        public MySqlDataReader QueryByDepartment(string dep,string type)
        {
            DateTime d = DateTime.Now;
            DateTime start = d.AddDays(-7);
            DateTime end = d.AddDays(7);
            return QueryByDateAndDep(start, end, dep, type);
        }
    }
}