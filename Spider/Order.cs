using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace SnnuWebService
{
    public class Order : Spider
    {
        private string[] data = { "预约者", "书名", "著者", "保留结束日期", "单册分馆", "取书地点"};
        string pattern = @"<td> \n([\s\S]+?)\n</td>";
        string urlpattern = @"电子书架</a>[\s\S]+?<a href=""([\s\S]+?)"" class=""blue"" title=""预约到馆""";
        private string html = string.Empty;
        public Order() : base()
        {
            this.Method = "GET";
            //encode = "GBK";
        }
        public string Search()
        {
            Create();
            request.AllowAutoRedirect = true;
            string reader = string.Empty;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
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
        public List<Dictionary<string, string>> GetList()
        {
            List<Dictionary<string, string>> DATA = new List<Dictionary<string, string>>();
            Dictionary<string, string> x = new Dictionary<string, string>();
            int num = 0;
            foreach (Match match in Regex.Matches(html, pattern))
            {
                if (num != 0 && num % 6 == 0)
                {
                    DATA.Add(x);
                    x = new Dictionary<string, string>();
                    //x.Clear();
                }
                x.Add(data[num % 6], match.Groups[1].Value.Replace("\n", "").Trim(' '));
                num++;
            }
            return DATA;
        }
        public string GetSpiderUrl()
        {
            this.Url = "http://opac.snnu.edu.cn:8991/F";
            Create();
            request.AllowAutoRedirect = true;
            string reader = string.Empty;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
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
            string url = "";
            foreach (Match match in Regex.Matches(reader, urlpattern))
                url = match.Groups[1].Value;
            return url;
        }
    }
}
