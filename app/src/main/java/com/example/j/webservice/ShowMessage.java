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

public class ShowMessage extends RecyclerView.Adapter<ShowMessage.ViewHolder>{
    private List<NoticeMessages> messageList;

    public ShowMessage(List<NoticeMessages> messageList)
    {
        this.messageList = messageList;
    }

    static class ViewHolder extends RecyclerView.ViewHolder{

        TextView Title;
        TextView Date;
        TextView Department;
        public ViewHolder(View view)
        {
            super(view);
            Title = (TextView)view.findViewById(R.id.txt_Title);
            Date = (TextView)view.findViewById(R.id.txt_Date);
            Department = (TextView)view.findViewById(R.id.txt_Department);
        }
    }
    @Override
    public int getItemCount() {
        return messageList.size();
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, int position) {
        NoticeMessages message = messageList.get(position);
        holder.Title.setText(message.GetTitle());
        holder.Date.setText(message.GetDate());
        holder.Department.setText(message.GetDepartment());
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.recyclerview,parent,false);
        ViewHolder holder = new ViewHolder(view);
        return holder;
    }

}
