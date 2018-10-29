using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SnnuWebService
{
    /// <summary>
    /// News 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class News : System.Web.Services.WebService
    {

        [WebMethod(Description =
            "所支持查询的部门")]
        public string[] getSupportDep()
        {
            List<string> ret = new List<string>();
            return ret.ToArray();
        }
        [WebMethod(Description =
            "查询某个部门最近两周内的新闻")]
        public Model.Message[] getNewsByDepartment(string dep)
        {
            List<Model.Message> ret = new List<Model.Message>();
            return ret.ToArray();
        }
        [WebMethod(Description =
            "查询以某个日期为准前后两周的新闻")]
        public Model.Message[] getNewsByDate(string date)
        {
            List<Model.Message> ret = new List<Model.Message>();
            return ret.ToArray();
        }
        [WebMethod(Description =
            "查询某个部门的某个日期前后两周的新闻")]
        public Model.Message[] getNewsByDepartmentAndDate(string dep, string date)
        {
            List<Model.Message> ret = new List<Model.Message>();
            return ret.ToArray();
        }
        [WebMethod(Description =
            "通过标题查询新闻(模糊查询)")]
        public Model.Message[] getNewsByLikeTitle(string keyword)
        {
            List<Model.Message> ret = new List<Model.Message>();
            return ret.ToArray();
        }
    }
}
