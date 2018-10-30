using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SnnuWebService
{
    /// <summary>
    /// Notice 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Notice : System.Web.Services.WebService
    {
        private DAL.Message dal = new DAL.Message();

        [WebMethod(Description =
            "所支持查询的部门")]
        public string [] getSupportDep()
        {
            List<string> ret = new List<string>();
            MySqlDataReader reader = dal.AllDep();
            while(reader.Read())
            {
                ret.Add(reader.GetValue(0).ToString());
            }
            reader.Close();
            return ret.ToArray();
        }
        [WebMethod(Description =
            "查询某个部门最近两周内的通知")]
        public Model.Message[]getNoticeByDepartment(string dep)
        {
            List<Model.Message> ret = new List<Model.Message>();
            MySqlDataReader reader = dal.QueryByDepartment(dep,"通知");
            while (reader.Read())
            {
                Model.Message temp = new Model.Message();
                temp.Department = reader["Department"].ToString();
                temp.Title = reader["Title"].ToString();
                temp.Link = reader["Link"].ToString();
                temp.Type = "通知";
                temp.Date = DateTime.Parse(reader["Date"].ToString());
                ret.Add(temp);
            }
            return ret.ToArray();
        }
        [WebMethod(Description =
            "查询以某个日期为准前后一周的通知")]
        public Model.Message[]getNoticeByDate(string date)
        {
            List<Model.Message> ret = new List<Model.Message>();
            DateTime d;
            bool flag=DateTime.TryParse(date, out d);
            if (flag == false)
                return null;
            DateTime start = d.AddDays(-7);
            DateTime end = d.AddDays(7);
            MySqlDataReader reader = dal.QueryByDate(start, end, "通知");
            while (reader.Read())
            {
                Model.Message temp = new Model.Message();
                temp.Department = reader["Department"].ToString();
                temp.Title = reader["Title"].ToString();
                temp.Link = reader["Link"].ToString();
                temp.Type = "通知";
                temp.Date = DateTime.Parse(reader["Date"].ToString());
                ret.Add(temp);
            }
            return ret.ToArray();
        }
        [WebMethod(Description =
            "查询某个部门的某个日期前后一周的通知")]
        public Model.Message[]getNoticeByDepartmentAndDate(string dep,string date)
        {
            List<Model.Message> ret = new List<Model.Message>();
            DateTime d;
            bool flag = DateTime.TryParse(date, out d);
            if (flag == false)
                return null;
            DateTime start = d.AddDays(-7);
            DateTime end = d.AddDays(7);
            MySqlDataReader reader = dal.QueryByDateAndDep(start, end, dep,"通知");
            while (reader.Read())
            {
                Model.Message temp = new Model.Message();
                temp.Department = reader["Department"].ToString();
                temp.Title = reader["Title"].ToString();
                temp.Link = reader["Link"].ToString();
                temp.Type = "通知";
                temp.Date = DateTime.Parse(reader["Date"].ToString());
                ret.Add(temp);
            }
            return ret.ToArray();
        }
        [WebMethod(Description =
            "通过标题查询通知(模糊查询)")]
        public Model.Message[]getNoticeByLikeTitle(string keyword)
        {
            List<Model.Message> ret = new List<Model.Message>();
            MySqlDataReader reader = dal.QueryByLikeTitle(keyword, "通知");
            while(reader.Read())
            {
                Model.Message temp = new Model.Message();
                temp.Department = reader["Department"].ToString();
                temp.Title = reader["Title"].ToString();
                temp.Link = reader["Link"].ToString();
                temp.Type = "通知";
                temp.Date = DateTime.Parse(reader["Date"].ToString());
                ret.Add(temp);
            }
            return ret.ToArray();
        }
    }
}
