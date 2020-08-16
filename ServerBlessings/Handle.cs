using ClientController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Codu.Data.DataBase;
using System.Data;
using System.Data.SqlClient;

namespace Web
{
    public class Handle
    {
        public static Udp udpClient ;
        public static IPEndPoint iep;
        public static void Send(string data)
        {
            if (iep == null)
                return;
            udpClient.Send(data);
        }
        public static async void Listen()
        {
            
            while (true)
            {
                await udpClient.Receive();
            }
        }
        public static bool AddMsg(string msg,int userId,string objType,int objIdx)
        {
            string sql = "INSERT Content(UserID,Message,ObjectIndex,ObjectType) VALUES(@USERID,@Message,@ObjectIndex,@ObjectType)";
            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("@USERID",userId);
            pars.Add("@Message", msg);
            pars.Add("@ObjectIndex", objIdx);
            pars.Add("@ObjectType", objType);
            bool result = SqlHelper.ExecuteNonQuery(sql, pars);
            return result;
        }
        public static bool AddUser(string nickName,string openId)
        {
            string sql = "INSERT [USERS](Nickname,openId) VALUES(@Nickname,@openId)";
            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("@Nickname",nickName);
            pars.Add("@openId", openId);
            bool result = SqlHelper.ExecuteNonQuery(sql, pars);
            return result;
        }
        public static int GetUserId(string openId)
        {
            int id = -1;
            string sql = "SELECT ID FROM [USERS] WHERE OPENID=@OPENID";
            SqlParameter[] pars = new SqlParameter[] { 
            new SqlParameter("@OPENID",openId)};
            object o = SqlHelper.ExecuteScalar(sql,pars);
            if (o != null)
                id = (int)o;
            return id;
        }
        public static bool SetMsgReaded(int id)
        {
            string sql = "UPDATE Content SET ISSHOWED=1 WHERE ID=@ID";
            Dictionary<string, object> pars = new Dictionary<string, object>();
            pars.Add("@ID",id);
            bool result = SqlHelper.ExecuteNonQuery(sql, pars);
            return result;
        }
        public static DataTable GetMsg()
        {
            DateTime dt = default;
            string sql = @"SELECT C.ID, U.Nickname,C.Message,C.ObjectIndex,C.ObjectType FROM CONTENT C
LEFT JOIN [Users] U ON C.UserID=U.ID
WHERE ISSHOWED=0";
            DataTable result = SqlHelper.ExecuteSql(sql);
            return result;
        }
       
    }
}
