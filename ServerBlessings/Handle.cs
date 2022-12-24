using ClientController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Codu.Data.DataBase;
using System.Data;
using System.Data.SqlClient;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using NLog;

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
            msg = FilterWord(msg);
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
        public static DataTable GetMsg(int tpIdx)
        {
            DateTime dt = default;
            string[] typeStrings = { "light","firework","tree"};
            string sql = @"SELECT C.ID, U.Nickname,U.Headimgurl,C.Message,C.ObjectIndex,C.ObjectType FROM CONTENT C
LEFT JOIN [Users] U ON C.UserID=U.ID
WHERE ISSHOWED=0 ";
            if (tpIdx > 0)
            {
                sql += $"AND C.OBJECTTYPE='{typeStrings[tpIdx]}'";
            }
            DataTable result = SqlHelper.ExecuteSql(sql);
            return result;
        }
       public static int GetSettingType()
        {
            int result = -1;
            string sql = "select setvalue from setting where setname='type'";
            object o=SqlHelper.ExecuteScalar(sql);
            result = int.Parse((string)o);
            return result;
        }
        public static bool SetType(int i)
        {
            string sql = $"update setting set setvalue={i} where setname='Type'";
            bool result = SqlHelper.ExecuteNonQuery(sql);
            return result;
        }
        public static string FilterWord(string txt)
        {
            string fileName = $@"{System.Environment.CurrentDirectory}\wwwroot\zidingyi.txt";
            FileInfo fi = new FileInfo(fileName);
            using (StreamReader sr=new StreamReader(fi.OpenRead()))
            {
                string word = sr.ReadLine();
                while (null!=word)
                {
                    if(word!="")
                    if (txt.Contains(word))
                    {
                        txt = txt.Replace(word, "*");
                    }
                    word = sr.ReadLine();
                }
            }
            return txt;
        }
        public static string GetPath()
        {
            return System.Environment.CurrentDirectory;
        }
        /// <summary>
        /// 获取OPENID
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Task<JObject> GetOpenId(string code)
        {
            string url = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid=wx34ee0c35e400d9d6&secret=08756fc67900bf6735a62ca410cd9145&code={code}&grant_type=authorization_code";
            return GetAsync(url);
        }
        public static Task<JObject> GetUserInfo(string openId,string access_token)
        {
            string url = $"https://api.weixin.qq.com/sns/userinfo?access_token={access_token}&openid={openId}&lang=zh_CN";
            return GetAsync(url);
        }
        static async Task<JObject> GetAsync(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var retContent = await response.Content.ReadAsStringAsync();
            JObject jRet = JObject.Parse(retContent);
            return jRet;
        }
        static int AddUser(string openId, string nickname, string province, bool sex, string city, string country, string headimgurl, string privilege, string unionid)
        {
            if (unionid == null)
                unionid = "";
            int id = -1;
            string sql = @"INSERT USERS(OPENID,NICKNAME,PROVINCE,SEX,CITY,COUNTRY,HEADIMGURL,PRIVILEGE,UNIONID) 
                VALUES(@OPENID,@NICKNAME,@PROVINCE,@SEX,@CITY,@COUNTRY,@HEADIMGURL,@PRIVILEGE,@UNIONID)
                SELECT @@IDENTItY";
            SqlParameter[] pars = new SqlParameter[] {
            new SqlParameter("@OPENID",openId),
            new SqlParameter("@NICKNAME",nickname),
            new SqlParameter("@PROVINCE",province),
            new SqlParameter("@SEX",sex),
            new SqlParameter("@CITY",city),
            new SqlParameter("@COUNTRY",country),
            new SqlParameter("@HEADIMGURL",headimgurl),
            new SqlParameter("@PRIVILEGE",privilege),
            new SqlParameter("@UNIONID",unionid)
            };
            object o = SqlHelper.ExecuteScalar(sql, pars);
            if (o != null)
                id = int.Parse(o.ToString());
            return id;
        }
        public async static Task<int> GetUserIDByCode(string code)
        {
            var logger=LogManager.GetCurrentClassLogger();
            logger.Info("获取用户ID方法 GetUserIDByCode");
            logger.Debug($"?{code}");
            int id = -1;
            try
            {
                var jOpenId = await GetOpenId(code);
                var openId = (string)jOpenId["openid"];
                logger.Debug($"获取OpenId#{openId}");
                 id = GetUserId(openId);
                logger.Debug($"获取数据库中用户ID#{id}");
                if (id > 0)
                    return id;
                var accessToken = (string)jOpenId["access_token"];

                var userInfo = await GetUserInfo(openId, accessToken);
                logger.Debug($"获取用户信息#{userInfo.ToString(Newtonsoft.Json.Formatting.None)}");
                string nickName = (string)userInfo["nickname"];
                string province = (string)userInfo["province"];
                bool sex = (int)userInfo["sex"] == 1 ? true : false;
                string city = (string)userInfo["city"];
                string country = (string)userInfo["country"];
                string headimgurl = (string)userInfo["headimgurl"];
                string privilege = userInfo["privilege"].ToString();
                string unionid = (string)userInfo["unionid"];
                id = AddUser(openId, nickName, province, sex, city, country, headimgurl, privilege, unionid);
                logger.Debug($"添加数据库用户#{id}");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return id;

        }
    }
}
