package com.example.j.webservice;
import android.app.DatePickerDialog;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.Toast;
import android.widget.TextView;
import java.util.Calendar;
import com.example.j.webservice.ShowMessage;

import com.tudou.demo.R;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Dictionary;
import java.util.List;
import java.util.Map;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {
    private List<NoticeMessages> messageList = new ArrayList<>();
    private Button btn1;
    private String input="";
    private String str;
    private Calendar cal;
    private int year,month,day;
    private TextView txt_DateShow;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        txt_DateShow = (TextView) findViewById(R.id.txt_DateShow);
        txt_DateShow.setOnClickListener(this);
        btn1 = (Button) findViewById(R.id.btn_query);
        getDate();
        btn1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                getDate();
                BtnClick();
                RecyclerView recyclerView = (RecyclerView)findViewById(R.id.recy_list);
                recyclerView.addItemDecoration(new MyDecoration(MainActivity.this, MyDecoration.VERTICAL_LIST));
                LinearLayoutManager layoutManager = new LinearLayoutManager(MainActivity.this);
                recyclerView.setLayoutManager(layoutManager);
                ShowMessage messageAdapter = new ShowMessage(messageList);
                recyclerView.setAdapter(messageAdapter);
            }
        });

    }

    private void getDate() {
        cal=Calendar.getInstance();
        year=cal.get(Calendar.YEAR);       //获取年月日时分秒
        Log.i("wxy","year"+year);
        month=cal.get(Calendar.MONTH);   //获取到的月份是从0开始计数
        day=cal.get(Calendar.DAY_OF_MONTH);
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.txt_DateShow:
                DatePickerDialog.OnDateSetListener listener=new DatePickerDialog.OnDateSetListener() {

                    @Override
                    public void onDateSet(DatePicker arg0, int year, int month, int day) {
                        txt_DateShow.setText(year+"-"+(++month)+"-"+day);  //将选择的日期显示到TextView中,因为之前获取month直接使用，所以不需要+1，这个地方需要显示，所以+1
                        input=year+"-"+(month)+"-"+day;
                    }
                };
                DatePickerDialog dialog=new DatePickerDialog(this,DatePickerDialog.THEME_DEVICE_DEFAULT_LIGHT ,listener,year,month,day);//主题在这里！后边三个参数为显示dialog时默认的日期，月份从0开始，0-11对应1-12个月
                dialog.show();
                break;

            default:
                break;
        }
    }

    private void BtnClick() {
        final  String SERVICE_NS = "http://webxml.zhaoqi.vip/";//命名空间
        final  String SOAP_ACTION = "http://webxml.zhaoqi.vip/getNoticeByDate";//用来定义消息请求的地址，也就是消息发送到哪个操作
        final  String SERVICE_URL = "http://webxml.zhaoqi.vip/Notice.asmx";//URL地址，这里写发布的网站的本地地址
        String methodName = "getNoticeByDate";
        //创建HttpTransportSE传输对象，该对象用于调用Web Service操作
        final HttpTransportSE ht = new HttpTransportSE(SERVICE_URL);
        //使用SOAP1.2协议创建Envelop对象。从名称上来看,SoapSerializationEnvelope代表一个SOAP消息封包；但ksoap2-android项目对
        //SoapSerializationEnvelope的处理比较特殊，它是HttpTransportSE调用Web Service时信息的载体--客户端需要传入的参数，需要通过
        //SoapSerializationEnvelope对象的bodyOut属性传给服务器；服务器响应生成的SOAP消息也通过该对象的bodyIn属性来获取。
        final SoapSerializationEnvelope envelope = new SoapSerializationEnvelope(SoapEnvelope.VER12);
        //实例化SoapObject对象，创建该对象时需要传入所要调用的Web Service的命名空间、Web Service方法名
        final SoapObject soapObject = new SoapObject(SERVICE_NS, methodName);
        //对dotnet webservice协议的支持,如果dotnet的webservice
        envelope.bodyOut = soapObject;
        envelope.setOutputSoapObject(soapObject);
        envelope.dotNet = true;
        soapObject.addProperty("date",input);
        Log.d("input :",input);
        //调用SoapSerializationEnvelope的setOutputSoapObject()方法，或者直接对bodyOut属性赋值，将前两步创建的SoapObject对象设为
        //SoapSerializationEnvelope的付出SOAP消息体
        new Thread(){
            @Override
            public void run() {
                try {
                    //调用WebService，调用对象的call()方法，并以SoapSerializationEnvelope作为参数调用远程Web Service
                    ht.call(SOAP_ACTION, envelope);
                    if(envelope.getResponse() != null){
                        //获取服务器响应返回的SOAP消息，调用完成后，访问SoapSerializationEnvelope对象的bodyIn属性，该属性返回一个
                        //SoapObject对象，该对象就代表了Web Service的返回消息。解析该SoapObject对象，即可获取调用Web Service的返回值
                        SoapObject so = (SoapObject) envelope.bodyIn;
                        //接下来就是从SoapObject对象中解析响应数据的过程了
                        //for (int i = 0; i < so.getPropertyCount(); i++) {
                          //  result.getGetNoticeByDateResult().getMessage().;
                       //}
                        String string = "";
                        String str_Title = "";
                        String str_Date = "";
                        String str_Department = "";
                        String str_Type = "";
                        String str_Link ="";

                        str = so.getProperty(0).toString();
                        Log.d("result ",str);

                        for(int i = 0 ; i < str.length() ; i++) {
                            if(i + 5 < str.length() && str.substring(i , i + 5).equals("Title"))
                            {

                                i += 6;
                                int tag1 = str.indexOf(';',i);
                                if(str.charAt(tag1 + 1)!=' ')
                                {
                                    tag1++;
                                    tag1 = str.indexOf(';' , tag1);
                                }
                                str_Title = str.substring(i , tag1);//"Title";
                                i = tag1 + 1;
                                i += 6;
                                int tag2 = str.indexOf(';',i);

                                str_Link = str.substring(i , tag2);//"Link";
                                i = tag2 + 1;
                                i += 6;
                                int tag3 = str.indexOf(';',i);
                                int tag3x = str.indexOf("T",i);
                                str_Date = str.substring(i , tag3x);//"Data";
                                i = tag3 + 1;
                                i += 6;
                                int tag4 = str.indexOf(';',i);

                                str_Type = str.substring(i , tag4);//"Type";
                                i = tag4 + 1;
                                i += 12;
                                int tag5 = str.indexOf(';',i);

                                str_Department = str.substring(i , tag5);//"Department";
                                i = tag5 + 1;
                                if(str_Title.length() > 17)
                                    str_Title = str_Title.substring(0,17) + "...";
                                NoticeMessages message = new NoticeMessages("   "+str_Title,"时间："+str_Date,str_Link,str_Type,str_Department);
                                messageList.add(message);
                            }

                        }
                        Log.d("result ",messageList.isEmpty()+"");
                    }else
                    {
                        Toast.makeText(MainActivity.this, "连接服务器失败", Toast.LENGTH_LONG).show();
                    }
                } catch (IOException e) {
                    e.printStackTrace();
                } catch (XmlPullParserException e) {
                    e.printStackTrace();
                }
            }
        }.start();
    }
}
