package com.example.j.webservice;

/**
 * Created by J on 2018/11/28.
 */

public class CardMessages {
    private String OrigiAmount;
    private String Date;
    private String Frequency;
    private String TransAmount;
    private String Balance;
    private String Location;
    public CardMessages(String Date , String Frequency , String OrigiAmount ,String TransAmount ,String Balance , String Location)
    {
        this.Date = Date;
        this.Frequency = Frequency;
        this.OrigiAmount = OrigiAmount;
        this.TransAmount = TransAmount;
        this.Balance = Balance;
        this.Location = Location;
    }
    public String GetOrigiAmount()
    {
        return this.OrigiAmount;
    }
    public String GetFrequency()
    {
        return this.Frequency;
    }
    public String GetDate()
    {
        return this.Date;
    }
    public String GetTransAmount()
    {
        return this.TransAmount;
    }
    public String GetBalance()
    {
        return this.Balance;
    }
    public String GetLocation(){return this.Location;}
}
