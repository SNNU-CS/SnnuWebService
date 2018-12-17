package com.example.j.webservice;

import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;
import com.tudou.demo.R;
import java.util.List;

/**
 * Created by J on 2018/11/28.
 */

public class ShowMessage2 extends RecyclerView.Adapter<ShowMessage2.ViewHolder>{
    private List<CardMessages> messageList;

    public ShowMessage2(List<CardMessages> messageList)
    {
        this.messageList = messageList;
    }

    static class ViewHolder extends RecyclerView.ViewHolder{

        TextView Frequency;
        TextView Date;
        TextView Location;
        TextView TransAmount;
        TextView Balance;
        public ViewHolder(View view)
        {
            super(view);
            Frequency = (TextView)view.findViewById(R.id.txt_Frequency);
            Date = (TextView)view.findViewById(R.id.txt_Date2);
            TransAmount = (TextView)view.findViewById(R.id.txt_TransAmount);
            Balance = (TextView)view.findViewById(R.id.txt_Balance);
            Location = (TextView)view.findViewById(R.id.txt_Location);
        }
    }
    @Override
    public int getItemCount() {
        return messageList.size();
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, int position) {
        CardMessages message = messageList.get(position);
        holder.Date.setText(message.GetDate());
        holder.Frequency.setText(message.GetFrequency());
        holder.TransAmount.setText(message.GetTransAmount());
        holder.Balance.setText(message.GetBalance());
        holder.Location.setText(message.GetLocation());
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.recyclerview2
                ,parent,false);
        ViewHolder holder = new ViewHolder(view);
        return holder;
    }

}
