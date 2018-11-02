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
    public class WebService1 : System.Web.Services.WebService
    {

        [WebMethod(Description =
            "查询校园卡余额")]
        public double getBalance(string id)
        {
            double ret=0;
            return ret;
        }
        [WebMethod(Description =
            "查询消费明细")]
        public Model.CardItem[] getConsumptionDdetails(string id)
        {
            List<Model.CardItem> ret = new List<Model.CardItem>();
            return ret.ToArray();
        }
        [WebMethod(Description =
            "查询最后一次消费时间")]
        public DateTime getLastConsumptionTime(string id)
        {
            DateTime ret = DateTime.MinValue;
            return ret;
        }
    }
}
