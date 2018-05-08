using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml;

namespace JinRi.Air.Model.LanzSMSModel
{    
    
    /// <summary>
    /// 网络工具类
    /// 2011-04-08
    /// </summary>
    public sealed class LanzWeb
    {
        public  LanzSMSReturn SendMessage(LanzSMSSend send)
        {
            try
            {
                string g_DnsUrl = LanzSMSBase.url;
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("UserID", LanzSMSBase.UserID);
                param.Add("Account", LanzSMSBase.Account);
                param.Add("Password", LanzSMSBase.Password);
                param.Add("SMSType", send.SMSType);
                param.Add("Content", System.Web.HttpUtility.UrlEncode(send.Content.Trim(), System.Text.Encoding.GetEncoding("gb2312")));
                param.Add("Phones", send.Phones);
                param.Add("SendDate", "");
                param.Add("SendTime", "");
                LanzWeb web = new LanzWeb();
                LanzSMSReturn ret = GetObj(web.DoPost(g_DnsUrl, param));
                ret.NoteCount = GetNoteCount(send.Content);
                return ret;
            }
            catch (Exception ex)
            {
                LogWriter.Write("dsadsa", ex.ToString(), "err");
                throw ex;
            }

        }
        /// <summary>
        /// 获取返回对象
        /// </summary>
        /// <param name="returnxml"></param>
        /// <returns></returns>
        public LanzSMSReturn GetObj(string returnxml)
        {
            try
            {
                LanzSMSReturn ret = new LanzSMSReturn();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(returnxml);
                XmlNode rootNode = doc.DocumentElement;
                for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    if (rootNode.ChildNodes[i].Name == "ErrorNum")
                    {
                        ret.ErrorNum = rootNode.ChildNodes[i].InnerText;
                    }
                    if (rootNode.ChildNodes[i].Name == "JobID")
                    {
                        ret.JobID = rootNode.ChildNodes[i].InnerText;
                    }
                    if (rootNode.ChildNodes[i].Name == "PhonesSend")
                    {
                        ret.PhonesSend = rootNode.ChildNodes[i].InnerText;
                    }
                    if (rootNode.ChildNodes[i].Name == "ErrPhones")
                    {
                        ret.ErrPhones = rootNode.ChildNodes[i].InnerText;
                    }
                }
                ret.ErrorNumDescs = ret.ErrorNumDesc;
                return ret;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取发送短信条数
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string GetNoteCount(string message)
        {
            if (message.Length <= 63)
            {
                return "1";
            }
            else
            {
                if (message.Length % 59 == 0)
                {
                    return (message.Length / 59).ToString();
                }
                else
                {
                    return (message.Length / 59 + 1).ToString();
                }
            }
        }


        private int _timeout = 100000;
        private bool m_bKeepCookie = false;
        private CookieContainer objCookieContainer = null;

        public LanzWeb(bool bKeepCookie)
        {
            m_bKeepCookie = bKeepCookie;
            if (bKeepCookie) objCookieContainer = new CookieContainer();
        }
        public LanzWeb()
        {
        }
        /// <summary>
        /// 请求与响应的超时时间
        /// </summary>
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> parameters)
        {
            //LogWriter.Write("lanz", url);

            HttpWebRequest objRequset = GetWebRequest(url, "POST");
            objRequset.ContentType = "application/x-www-form-urlencoded";

            byte[] postData = Encoding.GetEncoding("gb2312").GetBytes(BuildQuery(parameters));
            Stream reqStream = objRequset.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)objRequset.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);

            return GetResponseAsString(rsp, encoding);
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> parameters, CookieContainer cookieContainer)
        {
            HttpWebRequest objRequset = GetWebRequest(url, "POST");
            objRequset.ContentType = "application/x-www-form-urlencoded";
            objRequset.CookieContainer = cookieContainer;//构造cookie

            byte[] postData = Encoding.GetEncoding("gb2312").GetBytes(BuildQuery(parameters));
            Stream reqStream = objRequset.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)objRequset.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }


        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                if (url.Contains("?"))
                {
                    url = url + "&" + BuildQuery(parameters);
                }
                else
                {
                    url = url + "?" + BuildQuery(parameters);
                }
            }

            HttpWebRequest req = GetWebRequest(url, "GET");
            req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }

        public HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.KeepAlive = true;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "Vienna";
            req.Timeout = this._timeout;

            if (m_bKeepCookie)
                req.CookieContainer = objCookieContainer;

            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, System.Text.Encoding.GetEncoding("GB2312"));
                //  using (StreamReader sr = new StreamReader(sm, System.Text.Encoding.GetEncoding("GB2312"))) 

                // 每次读取不大于256个字符，并写入字符串
                char[] buffer = new char[256];
                int readBytes = 0;
                while ((readBytes = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    result.Append(buffer, 0, readBytes);
                }
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }

            return result.ToString();
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");
                    postData.Append(value);
                    hasParam = true;
                }
            }

            return postData.ToString();
        }

    }
}
