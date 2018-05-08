using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Air.Model.LanzSMSModel
{
    //短信发送发送类
    public class LanzSMSSend
    {
        public LanzSMSSend()
        {
            SMSType = "1";
            Content = "";
            Phones = "";
            PostFixNum = "";
            SendDate = "";
            SendTime = "";
            
        }

        /// <summary>
        ///短信类型	1表示移动、联通	2表示小灵通
        /// </summary>
        public string SMSType { set; get; }

        /// <summary>
        /// 短信内容，一般不超过70个字，超长部分自动分段，总长不超过300个字
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 手机号码，，最后一个不带“;”（分号）
        /// </summary>
        public string Phones { set; get; }

        /// <summary>
        /// 用户自定义的特服号码扩展，系统会自动加在分配给用户的特服号码之后，长度最长不超过6位
        /// </summary>
        public string PostFixNum { set; get; }

        /// <summary>
        /// 定时发送的日期，立即发送可以不输入或者为空
        /// </summary>
        public string SendDate { set; get; }

        /// <summary>
        /// 定时发送的时间，立即发送可以不输入或者为空
        /// </summary>
        public string SendTime { set; get; }

        /// <summary>
        /// 短信关键字
        /// </summary>
        public string SMSKey { set; get; }
    }
}
