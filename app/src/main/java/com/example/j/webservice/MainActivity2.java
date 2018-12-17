package com.example.j.webservice;

/**
 * Created by J on 2018/11/29.
 */

import android.app.DatePickerDialog;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.tudou.demo.R;

import org.ksoap2.SoapEnvelope;
import org.ksoap2.serialization.SoapObject;
import org.ksoap2.serialization.SoapSerializationEnvelope;
import org.ksoap2.transport.HttpTransportSE;
import org.xmlpull.v1.XmlPullParserException;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Map;

public class MainActivity2 extends AppCompatActivity implements View.OnClickListener {
    private List<CardMessages> messageList2 = new ArrayList<>();
    private Button btn1;
    private String input="";
    EditText editText;
    private String str;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main2);
        editText = (EditText)findViewById(R.id.edit_CardNum);
        btn1 = (Button) findViewById(R.id.btn_query2);
        btn1.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                input = editText.getText().toString();
                BtnClick();
                RecyclerView recyclerView = (RecyclerView)findViewById(R.id.recy_list2);
                recyclerView.addItemDecoration(new MyDecoration(MainActivity2.this, MyDecoration.VERTICAL_LIST));
                LinearLayoutManager layoutManager = new LinearLayoutManager(MainActivity2.this);
                recyclerView.setLayoutManager(layoutManager);
                ShowMessage2 messageAdapter = new ShowMessage2(messageList2);
                recyclerView.setAdapter(messageAdapter);
            }
        });

    }

    @Override
    public void onClick(View view) {
    }

    private void BtnClick() {
        final  String SERVICE_NS = "http://webxml.zhaoqi.vip/";//命名空间
        final  String SOAP_ACTION = "http://webxml.zhaoqi.vip/getConsumptionDdetails";//用来定义消息请求的地址，也就是消息发送到哪个操作
        final  String SERVICE_URL = "http://webxml.zhaoqi.vip/CampusCard.asmx";//URL地址，这里写发布的网站的本地地址
        String methodName = "getConsumptionDdetails";
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
        soapObject.addProperty("id",input);
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
                        String str_Date = "";
                        String str_Frequency = "";
                        String str_OrigiAmount = "";
                        String str_TransAmount = "";
                        String str_Balance ="";
                        String str_Location = "";

                        str = so.getProperty(0).toString();
                        Log.d("result ",str);

                        for(int i = 0 ; i < str.length() ; i++) {
                            if(i + 4 < str.length() && str.substring(i , i + 4).equals("Date"))
                            {
                                i += 5;
                                int tag1 = str.indexOf(';',i);
                                str_Date = str.substring(i , tag1);
                                str_Date = str_Date.replace("T" , "  ");  //Date
                                Log.d("result ",str_Date);
                                i = tag1+1;
                                i += 11;
                                int tag2 = str.indexOf(';' , i);
                                str_Frequency = str.substring(i , tag2);    //Frequency
                                Log.d("result ",str_Frequency);
                                i = tag2 + 1;
                                i += 13;
                                int tag3 = str.indexOf(';' , i);
                                str_OrigiAmount = str.substring(i , tag3);    //OrigiAmount
                                Log.d("result ",str_OrigiAmount);
                                i = tag3 + 1;
                                i +=13;
                                int tag4 = str.indexOf(';' , i);
                                str_TransAmount = str.substring(i , tag4);    //TransAmount
                                Log.d("result ",str_TransAmount);
                                i = tag4 + 1;
                                i += 9;
                                int tag5 = str.indexOf(';' , i);
                                str_Balance = str.substring(i , tag5);
                                Log.d("result ",str_Balance);
                                i = tag5 + 1;
                                i += 10;
                                int tag6 = str.indexOf(';' , i);
                                str_Location = str.substring(i , tag6);
                                Log.d("result ",str_Location);
                                i = tag6 + 1;
                                CardMessages message = new CardMessages("时间："+str_Date , "次数："+str_Frequency,str_OrigiAmount,"消费："+str_TransAmount+"元","余额："+str_Balance+"元","地点："+str_Location);
                                messageList2.add(message);
                            }

                        }
                        Log.d("result ",messageList2.isEmpty()+"");

                    }else
                    {
                        Toast.makeText(MainActivity2.this, "连接服务器失败", Toast.LENGTH_LONG).show();
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
