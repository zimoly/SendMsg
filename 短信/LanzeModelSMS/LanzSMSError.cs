using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JinRi.Air.Model.LanzSMSModel
{
    //短信发送错误类
    public class LanzSMSError
    {
        public static string GetError(string err)
        {
            string ret = string.Empty;
            switch (err)
            {
                case "1000":
                    ret = "当前用户已经登录";
                    break;
                case "1001":
                    ret = "当前用户没有登录";
                    break;
                case "1002":
                    ret = "登录被拒绝";
                    break;
                case "2001":
                    ret = "短信发送失败";
                    break;
                case "2002":
                    ret = "短信库存不足";
                    break;
                case "2003":
                    ret = "存在无效的手机号码";
                    break;
                case "2004":
                    ret = "短信内容包含禁用词语";
                    break;
                case "3001":
                    ret = "没有要接收的短信";
                    break;
                case "3002":
                    ret = "没有要接收的回复状态";
                    break;
                case "9001":
                    ret = "JobID参数不符合要求";
                    break;
                case "9002":
                    ret = "SendDate或SendTime参数不是有效日期";
                    break;
                case "9003":
                    ret = "短信内容长度超过300";
                    break;
                case "9004":
                    ret = "参数不符合要求";
                    break;
                case "9099":
                    ret = "其它系统错误";
                    break;
                case "-1":
                    ret = "系统错误 错误信息详见短信日志";
                    break;
                case "0":
                    ret = "发送短信成功";
                    break;
                case "9527":
                    ret = "充值用户短信余额不足";
                    break;
            }
            return ret;
        }
    }
}
