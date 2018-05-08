using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using JinRi.Air.Model.LanzSMSModel;
using System.Security.Cryptography;

namespace LanzeModelSMS
{
   public static class DataHelper
    {
        static string connstring = Decrypt(AppConfig.GetConnectionSetting("JinRiuserdbConnectionString"), AppConfig.GetAppSetting("ConnectKey"), System.Text.Encoding.Default);// "server=192.168.2.229;database=IFltUserDB;uid=all;pwd=SDFer98kd*&8834sdlfjerfgs";;
      
         private static string Decrypt(string encrypted, string key, Encoding encoding)
        {
            byte[] buff = Convert.FromBase64String(encrypted);
            byte[] kb = System.Text.Encoding.Default.GetBytes(key);
            return encoding.GetString(Decrypt(buff, kb));
        }

        /// <summary>
        /// 使用指定密钥解密数据
        /// </summary>
        /// <param name="encrypted">密文</param>
        /// <param name="key">密钥</param>
        /// <returns>明文</returns>
        private static byte[] Decrypt(byte[] encrypted, byte[] key)
        {
            try { 
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = MakeMD5(key);
            des.Mode = CipherMode.ECB;
            return des.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
            }
            catch(Exception ex)
            {  
                return null;
            }
            

           
        }
        private static byte[] MakeMD5(byte[] original)
        {
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyhash = hashmd5.ComputeHash(original);
            hashmd5 = null;
            return keyhash;
        }


        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }
            command.Connection = connection;

            command.CommandText = commandText;

            command.CommandTimeout = 0;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            command.CommandType = commandType;

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }

        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        public static object ExecuteScalar(string commandText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                connection.Open();
                return ExecuteScalar(connection, CommandType.Text, commandText, commandParameters);
            }
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            SqlCommand cmd = new SqlCommand();

            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            object retval = cmd.ExecuteScalar();

            cmd.Parameters.Clear();

            if (mustCloseConnection)
                connection.Close();

            return retval;
        }
        public static int ExecuteNonQuery( CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                connection.Open();
                // Call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);

            }
        }
        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            //WriteSQLLog.WriteLog(commandText + ParaTostr(commandParameters));

            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            // Finally, execute the command
            int retval = cmd.ExecuteNonQuery();

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }

        public static DataSet ExecuteDataset( CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {

            
                using (SqlConnection connection = new SqlConnection(connstring))
                {
                    connection.Open();
                    return ExecuteDataset(connection, commandType, commandText, commandParameters);
                }
           
        }

        public static DataTable GetDataTable( string commandText, params SqlParameter[] commandParameters)
        {
              return ExecuteDataset(CommandType.Text, commandText, commandParameters).Tables[0];
        }


        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException("connection");

            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);


            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                da.Fill(ds);

                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();
                return ds;
            }

        }




    }
}
