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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            listView1.GridLines = true;
            //listView1.Location = new Point(20, 20);
            //listView1.Size = new Size(500, 500);
            listView1.View = View.Details;
            listView1.Columns.Add("标题", 300, HorizontalAlignment.Center);
            listView1.Columns.Add("时间", 90, HorizontalAlignment.Center);
            listView1.Columns.Add("链接", 350, HorizontalAlignment.Center);

            this.listView1.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度


        }


        private void BtnOk_Click(object sender, EventArgs e)
        {
            ServiceReference1.NoticeSoapClient s = new ServiceReference1.NoticeSoapClient();
            try
            {
                ServiceReference1.Message[] ret;
                if (!string.IsNullOrEmpty(comboBox1.Text) && !string.IsNullOrEmpty(TextBoxDate.Text))
                {
                    ret = s.getNoticeByDepartmentAndDate(comboBox1.Text.ToString(),TextBoxDate.Text.ToString()); 
                }
                else if(!string.IsNullOrEmpty(TextBoxDate.Text))
                {
                    ret = s.getNoticeByDate(TextBoxDate.Text.ToString());
                }
                else
                {
                    ret = s.getNoticeByDepartment(comboBox1.Text.ToString());
                }
                
                foreach (var t in ret)
                {
                    ListViewItem lvi = new ListViewItem();

                    lvi.Text = t.Title;

                    lvi.SubItems.Add(t.Date.ToString("yyyy-MM-dd"));

                    lvi.SubItems.Add(t.Link);

                    this.listView1.Items.Add(lvi);
                    //textBox2.Text += ToString(t);
                }
                this.listView1.EndUpdate();  //结束数据处理，UI界面一次性绘制。
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
            string para = "date=" + TextBoxDate.Text.ToString();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
    }
}
