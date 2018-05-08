using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Air.Model.LanzSMSModel
{
    //短信发送结果类
    public class LanzSMSReturn
    {

        /// <summary>
        /// 错误参数
        /// </summary>
        public string ErrorNumDesc
        {
            get
            {
                return LanzSMSError.GetError(ErrorNum);
            }
        }


        /// <summary>
        /// 错误参数
        /// </summary>        
        public string ErrorNum { set; get; }
                
        /// <summary>
        /// 分配的群发短信任务号
        /// </summary>        
        public string JobID { set; get; }

        /// <summary>
        /// 服务器接收的有效手机号码数量
        /// </summary>
        public string PhonesSend { set; get; }

        /// <summary>
        /// 服务器拒绝的短信，用”;”分隔
        /// </summary>
        public string ErrPhones { set; get; }
        
        /// <summary>
        /// 短信发送条数
        /// </summary>
        public string NoteCount { set; get; }


        /// <summary>
        /// 错误信息集(群发专用)
        /// </summary>
        public string ErrorNumDescs { set; get; }


    }
}
