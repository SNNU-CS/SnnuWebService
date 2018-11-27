using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace WebServiceDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            ServiceReference1.NoticeSoapClient s = new ServiceReference1.NoticeSoapClient();
            try
            {
                ServiceReference1.Message[] ret = s.getNoticeByDate(TextBoxDate.Text.ToString());
                foreach (var t in ret)
                {
                    textBox2.Text += ToString(t);
                }
                textBox1.Text = getXmlText();
            }
            catch
            {
                MessageBox.Show("请检查网络");
            }
        }
        private string ToString(ServiceReference1.Message m)
        {
            return string.Format("标题:{0} 时间:{1} 链接:{2}", m.Title, m.Date.ToString("yyyy-MM-dd"), m.Link);

        }
        private string getXmlText()
        {
            string service = "http://webxml.zhaoqi.vip/Notice.asmx/getNoticeByDate";
            string contenttype = "application/x-www-form-urlencoded";//更网站该方法支持的类型要一致
                                                                     //根据接口，写参数
            string para = "date="+TextBoxDate.Text.ToString();
            //+ TextBoxDate.ToString();
            //发送请求
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(service);
            myRequest.Method = "POST";
            myRequest.ContentType = contenttype;
            myRequest.ContentLength = para.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] postdata = encoding.GetBytes(para);
            newStream.Write(postdata, 0, para.Length);
            newStream.Close();
            // Get response
            HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
            StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
            string content = reader.ReadToEnd();//得到结果
            return content;
        }
    }
}
