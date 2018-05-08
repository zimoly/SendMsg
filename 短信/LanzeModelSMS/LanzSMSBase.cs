using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 

namespace JinRi.Air.Model.LanzSMSModel
{
    //短信发送基础类
    public static class LanzSMSBase
    {
        public static string UserID { get { return AppConfig.GetAppSetting("LanzUserID"); } }
        //用户帐号
        public static string Account { get { return AppConfig.GetAppSetting("LanzAccount"); } }
        //用户密码
        public static string Password { get { return AppConfig.GetAppSetting("LanzPassword"); } }
        

        //发送短信请求地址
        public static string url { get { return "http://www.lanz.net.cn/LANZGateway/DirectSendSMSs.asp"; } }
        //登录请求地址
        public static string loginurl { get { return "http://www.lanz.net.cn/LANZGateway/Login.asp"; } } 
        //注销请求地址
        public static string logoffurl { get { return "http://www.lanz.net.cn/LANZGateway/Logoff.asp"; } }
        //获取短信数量请求地址
        public static string stockurl { get { return "http://www.lanz.net.cn/LANZGateway/GetSMSStock.asp"; } }
    }
}
