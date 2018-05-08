using System;
using System.IO;

namespace JinRi.Air.Model.LanzSMSModel
{
    public class LogWriter
    {
        private static readonly string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "applog\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";

        private delegate void WLog(string account, string comment, string filename, string dir);

        public static void AsyncWrite(string account, string comment, string filename = "system", string dir = "")
        {
            new WLog(LogWriter.Write).BeginInvoke(account, comment, filename, dir, null, null);
        }

        public static void Write(string account, string comment, string filename = "system", string dir = "")
        {
            try
            {
                string fielname = filename + ".log";
                if (!Directory.Exists(path + dir))
                {
                    Directory.CreateDirectory(path + dir);
                }
				if (dir.Length > 0) {
					dir += "\\";
				}
                StreamWriter sw = File.AppendText(path + dir + fielname);
                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff ") + account + " 执行了 " + comment);
				sw.Dispose();
            }
            catch
            {
            }
        }
    }
}
