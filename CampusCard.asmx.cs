using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace SnnuWebService
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class CampusCard : System.Web.Services.WebService
    {
        SpiderCard spider = new SpiderCard();
        [WebMethod(Description =
            "查询校园卡余额")]
        public double getBalance(string id)
        {
            double ret = 0;
            spider.Search(id);
            if (spider.BeingUsed())
                ret = Convert.ToDouble(spider.getBalance());
            else
                ret = -1;
            return ret;
        }
        [WebMethod(Description =
            "查询最近一周的消费明细")]
        public Model.CardItem[] getConsumptionDdetails(string id)
        {
            List<Model.CardItem> ret = new List<Model.CardItem>();
            List<Dictionary<string, string>> ListDic = new List<Dictionary<string, string>>();
            spider.Search(id);
            if (spider.BeingUsed())
            {
                ListDic = spider.ConsumptionDetails();
            }
            foreach (Dictionary<string, string> dic in ListDic)
            {
                Model.CardItem t = new Model.CardItem();
                t.Balance = Convert.ToDouble(dic["卡余额"].ToString());
                t.Date = DateTime.Parse(dic["时间"].ToString());
                t.Id = dic["卡号"].ToString();
                t.Frequency = Convert.ToInt32(dic["次数"].ToString());
                t.OrigiAmount = Convert.ToDouble(dic["原金额"].ToString());
                t.TransAmount = Convert.ToDouble(dic["交易额"].ToString());
                t.Location = dic["记录信息"].ToString();
                ret.Add(t);
            }
            return ret.ToArray();
        }
        [WebMethod(Description =
            "查询最后一次消费时间")]
        public DateTime getLastConsumptionTime(string id)
        {
            DateTime ret = DateTime.MinValue;
            spider.Search(id);
            if (spider.BeingUsed())
                ret = DateTime.Parse(spider.getLastConsumptionTime());
            return ret;
        }
    }
}
