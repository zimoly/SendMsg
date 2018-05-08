using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JinRi.Air.Model.LanzSMSModel;
using LanzeModelSMS;
namespace TestSendMessage
{
    /// <summary>
    /// SendMessage 的摘要说明
    /// </summary>
    public class SendMessage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string mobile = context.Request["mobile"].ToString().Trim();
                string comment = HttpUtility.UrlDecode(context.Request["comment"].ToString().Trim(), System.Text.Encoding.UTF8);
                int index = comment.IndexOf("【今通国际】");
                if (index<0)
                {
                    comment = comment + "【今通国际】";
                }

                LogWriter.Write(mobile, comment,"SendMessage");                
                LanzSMSReturn ret = null;        
                LanzSMSSend send = new LanzSMSSend();
                send.Content = comment;
                send.Phones = mobile;
                send.SMSType = "1";
                send.SMSKey = "test";
                //TODO插入
                AddMessageDAL obj = new AddMessageDAL();
                if (obj.GetMessageNumByPhoneNum(send))
                {
                    LogWriter.Write(mobile, "一分钟内同一条数据已经发送5次", "SendMessage");
                    context.Response.Write("一分钟内同一条数据已经发送5次");
                }
                else
                {
                    //发送短信
                    LanzWeb bll = new LanzWeb();
                    ret = bll.SendMessage(send);
                    obj.AddMessage(send);
                    LogWriter.Write(mobile, ret.ErrorNumDesc, "SendMessage");
                    context.Response.Write(ret.ErrorNumDesc);
                }

               
            }
            catch (Exception ex)
            {
                LogWriter.Write(context.Request["mobile"].ToString().Trim(), ex.ToString(), "SendMessage"); 
                context.Response.Write(ex.ToString());
            }
        }


     

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}