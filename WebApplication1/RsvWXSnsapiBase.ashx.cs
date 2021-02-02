using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Codu.Data.DataBase;
using System.Data.SqlClient;

namespace WebApplication1
{
    /// <summary>
    /// RsvWXSnsapiBase 的摘要说明
    /// </summary>
    public class RsvWXSnsapiBase : IHttpHandler
    {
        NameValueCollection paras;
        //string url_home = "http://zf.cracre.vip:81/default.html";
        string url_home = "http://zf.cyhdzy.com:81/default.html";
        public  void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            paras = context.Request.QueryString;
            WriteFile();
            string code = paras["code"];
            string state = paras["state"];
            //context.Response.Write("清除缓存");
            //context.Response.End();
            //return;

            //HttpClient client = new HttpClient();
            //HttpResponseMessage response= client.GetAsync("http://www.baidu.com").Result;
            //string s = response.Content.ReadAsStringAsync().Result;
            //context.Response.Write(s);
            //return;
            //string s = GetString("http://www.baidu.com");
            //context.Response.Write(s)
            //    ;
            //return;
            SqlHelper.ConnectionString = "server=.;uid=sa;pwd=123456;database=bless";

            try
            {
                if (!string.IsNullOrEmpty(code))
                {
                    //string url_getOpenId = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid=wx34ee0c35e400d9d6&secret=a8771dcd6a044ef1b9402e1f570c29a1&code={code}&grant_type=authorization_code";
                    string url_getOpenId = $"https://api.weixin.qq.com/sns/oauth2/access_token?appid=wxc151bbaf83fd2f29&secret=a34afb6d56209769b004d28e230f7df6&code={code}&grant_type=authorization_code";
                    JObject ret = Get(url_getOpenId);
                    string openId = (string)ret["openid"];
                    string access_token = (string)ret["access_token"];
                    int id = 0;
                    if (openId != null)
                        id = GetUserId(openId);
                    else
                    {
                        context.Response.Write($"错误:{ret.ToString()}");
                        return;
                    }
                    if (id > 0)
                    {
                        context.Response.Redirect(url_home + "?userid=" + id.ToString());
                    }
                    else
                    {
                        string url_getUserInfo = $"https://api.weixin.qq.com/sns/userinfo?access_token={access_token}&openid={openId}&lang=zh_CN";
                        JObject retUserInfo = Get(url_getUserInfo);
                        if (retUserInfo != null)
                        {
                            id = AddUser((string)retUserInfo["openid"], (string)retUserInfo["nickname"], (string)retUserInfo["province"], (int)retUserInfo["sex"] == 1 ? true : false, (string)retUserInfo["city"],
                                (string)retUserInfo["country"], (string)retUserInfo["headimgurl"], retUserInfo["privilege"].ToString(), (string)retUserInfo["unionid"]);
                            context.Response.Redirect(url_home + "?userid=" + id.ToString());
                        }
                    }
                }
            }
            catch(Exception e)
            {
                WriteFile(e.Message);
            }


            context.Response.Write("完成");
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        void WriteFile()
        {
            string fileName = $@"{System.AppDomain.CurrentDomain.BaseDirectory}123.txt";
            FileInfo fi = new FileInfo(fileName);
            
            using (StreamWriter sw = new StreamWriter(fi.OpenWrite()))
            {
                if (paras != null)
                {
                    for (int i = 0; i < paras.Count; i++)
                    {
                        sw.WriteLine($"{paras.GetKey(i)}:{paras[i]}");
                    }   
                }
            }
        }
        void WriteFile(string txt)
        {
            string fileName = $@"{System.AppDomain.CurrentDomain.BaseDirectory}log.txt";
            FileInfo fi = new FileInfo(fileName);
            StreamWriter sw;
            if (!fi.Exists)
            {
                sw = new StreamWriter(fi.Create());
            }
            else {
                sw = fi.AppendText();
           }
            
                
                if (txt != null)
                {
                    for (int i = 0; i < paras.Count; i++)
                    {
                        sw.WriteLine($"{txt}");
                    }
                }
            sw.Close();
        }
        async Task<JObject> GetAsync(string url)
        {
            JObject jRet;
            HttpClient client = new HttpClient();
            HttpResponseMessage response= client.GetAsync( url).Result;
            try
            {
                string ret =await response.Content.ReadAsStringAsync();
                jRet = JObject.Parse(ret);
            }
            catch (Exception)
            {

                throw;
            }
            return jRet;
        }
        async Task<string> GetStringAsync(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response =  client.GetAsync(url).Result;
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
        JObject Get(string url)
        {
            Task<JObject> task = GetAsync(url);
             task.Wait();
            return task.Result;
        }
        string GetString(string url)
        {
            Task<string> task = GetStringAsync(url);
            return task.Result;
        }
        int GetUserId(string openId)
        {
            int id = -1;
            string sql = $"SELECT ID from [users] where openid='{openId}'";
            object o = SqlHelper.ExecuteScalar(sql);
            if (o != null)
                id = (int)o;
            return id;
        }
        int AddUser(string openId,string nickname,string province,bool sex,string city,string country,string headimgurl,string privilege,string unionid)
        {
            if (unionid == null)
                unionid = "";
            int id=-1;
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
            object o=SqlHelper.ExecuteScalar(sql, pars);
            if(o!=null)
                id=int.Parse(o.ToString());
            return id;
        }
        //JObject GetOpenId(string code)
        //{

        //}
    }
}