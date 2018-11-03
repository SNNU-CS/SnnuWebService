using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SnnuWebService
{
    public class SpiderCard : Spider
    {
        private string[] data = { "卡号", "时间", "次数", "原金额", "交易额", "卡余额", "记录信息" };
        string pattern = @"<td>([\d.\s:\w-]+?)</td>";
        private string html = string.Empty;
        public SpiderCard() : base()
        {
            this.Url = "http://edutech.snnu.edu.cn/ecard/ccc.asp";
            Method = "POST";
            Create();
            request.ContentType = "application/x-www-form-urlencoded";
        }
        public string Search(string Id)
        {
            CookieContainer cookie = new CookieContainer();
            request.Headers.Add("Cookie", cookie.ToString());
            request.AllowAutoRedirect = true;
            string PostStr = string.Format("usernum={0}&search=查询&wx=", Id);
            byte[] Array = Encoding.UTF8.GetBytes(PostStr);
            request.ContentLength = Encoding.UTF8.GetByteCount(PostStr);
            System.IO.Stream MyStream = request.GetRequestStream();
            MyStream.Write(Array, 0, Array.Length);
            string reader = string.Empty;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                response.Cookies = cookie.GetCookies(response.ResponseUri);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encode));
                    reader = sr.ReadToEnd();
                    sr.Close();
                    response.Close();//关闭response响应流
                }
                else
                    throw new Exception();
            }
            catch
            {
                //Todo:Hanle Exception
            }
            html = reader;
            return reader;
        }

        public bool BeingUsed()
        {
            if (Regex.IsMatch(html,pattern))
                return true;
            else
                return false;
        }
        public List<Dictionary<string, string>> ConsumptionDetails()
        {
            List<Dictionary<string, string>> DATA = new List<Dictionary<string, string>>();
            Dictionary<string, string> x = new Dictionary<string, string>();
            int num = 0;
            foreach (Match match in Regex.Matches(html, pattern))
            {
                if (num != 0 && num % 7 == 0)
                {
                    DATA.Add(x);
                    x = new Dictionary<string, string>();
                }
                x.Add(data[num % 7], match.Groups[1].Value);
                num++;
            }
            return DATA;
        }
        public string getLastConsumptionTime()
        {
            int num = 1;
            string ret = string.Empty;
            foreach (Match match in Regex.Matches(html, pattern))
            {
                if (num == 2)
                {
                    ret = match.Groups[1].Value.ToString();
                    break;
                }
                num++;
            }
            return ret;
        }
       public  string getBalance()
        {
            int num = 1;
            string ret = string.Empty;
            foreach (Match match in Regex.Matches(html, pattern))
            {
                if (num == 6)
                {
                    ret = match.Groups[1].Value.ToString();
                    break;
                }
                num++;
            }
            return ret;
        }

    }
}