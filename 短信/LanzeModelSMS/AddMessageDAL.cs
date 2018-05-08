using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JinRi.Air.Model.LanzSMSModel;
using System.Data.SqlClient;
using System.Data;
using System.IO;


namespace LanzeModelSMS
{
   public class AddMessageDAL
    {
       /// <summary>
       /// 查询一个小时内同一用户相同内容发送的次数
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool GetMessageNumByPhoneNum(LanzSMSSend model)
       {
           StringBuilder sql = new StringBuilder();
           sql.AppendFormat(@"select count(*)
                            from SendMessageLimit with(nolock)
                            where Content =@Content 
                            and  PhoneNum =@PhoneNum 
                            and CONVERT(varchar(16),sendTime,120) = CONVERT(varchar(16),GETDATE(),120)");
           SqlParameter[] para={
           new SqlParameter("@Content", SqlDbType.VarChar,500){Value=model.Content},   
           new SqlParameter("@PhoneNum",SqlDbType.VarChar,50){Value=model.Phones}
           };
           return (int)DataHelper.ExecuteScalar(sql.ToString(), para)>=5;
       }
       /// <summary>
       /// 添加信息
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool AddMessage(LanzSMSSend model)
       {
           string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\Time.txt";
           string Time = FileRead(path);
           string timeNow = System.DateTime.Now.ToString("yyyy-MM-dd");
           if (timeNow != Time)
           {
               DelMessage();
               FileWrite(path, timeNow);
           }
           StringBuilder sql = new StringBuilder();
           sql.Append(@"INSERT INTO SendMessageLimit(Content,PhoneNum)VALUES(@Content,@PhoneNum)");
           SqlParameter[] para ={
           new SqlParameter("@Content", SqlDbType.VarChar,500){Value=model.Content},   
           new SqlParameter("@PhoneNum",SqlDbType.VarChar,50){Value=model.Phones}
           };
           return DataHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(), para) > 0;
       }
       /// <summary>
       /// 删除前一天数据
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       public bool DelMessage()
       {
           string sql = @"delete from SendMessageLimit  where SendTime<CONVERT(varchar(10),GETDATE(),120)";
           return DataHelper.ExecuteNonQuery(CommandType.Text, sql.ToString(),null)>0;
       }
       public string FileRead(string path)
       {
           if (!File.Exists(path))
           {
               return "";
           }
           else
           {
               return System.IO.File.ReadAllText(path);
               //StreamReader sr = new StreamReader(path, Encoding.Default);
               //return sr.ReadLine();
           }
          
       }
       public void FileWrite(string path,string Time)
       {
               FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);//创建写入文件 
               StreamWriter sw = new StreamWriter(fs);
               sw.Write(Time);//开始写入值
               fs.Flush();
               sw.Close();
               fs.Close();
       }
    }
}
