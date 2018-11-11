using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SnnuWebService
{
    /// <summary>
    /// Libiary 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Libiary : System.Web.Services.WebService
    {
        Order spider = new Order();
        [WebMethod(Description =
            "获取图书预约到馆信息")]
        public Model.LibiaryItem[] GetBookBorrowInfo()
        {
            List<Model.LibiaryItem> ret = new List<Model.LibiaryItem>();
            List<Dictionary<string, string>> ListDic = new List<Dictionary<string, string>>();
            spider.Url = spider.GetSpiderUrl();
            spider.Search();
            ListDic = spider.GetList();
            foreach (Dictionary<string, string> dic in ListDic)
            {
                Model.LibiaryItem t = new Model.LibiaryItem();
                t.Name = dic["预约者"].ToString();
                t.Author = dic["著者"].ToString();
                t.Location = dic["取书地点"].ToString();
                t.Branch = dic["单册分馆"].ToString();
                t.Deadline = DateTime.Parse(dic["保留结束日期"].ToString());
                ret.Add(t);
            }
            return ret.ToArray();
        }
    }
}
