using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Data;

namespace Web.Controller
{
    [ApiController]
    [Route("API")]
    public class BlessController:ControllerBase
    {
        [HttpPost()]
        public string Post(JObject value)
        {
            JObject ret=new JObject();
            ret["Result"] = false;
            if (value == null)
                ret["Message"] = "不能传空值";
            if (value["index"] != null && value["type"] != null)
            {
                Handle.Send(value.ToString());
                ret["Result"] = true;
                ret["Message"] = "已发送";
            }else
            {
                ret["Message"] = "传递参数不正确";
            }
            return ret.ToString();
        }
        [HttpGet()]
        public string Get(JObject value)
        {
            return "返回：" + value;
        }
        [HttpPost("addmsg")]
        public JObject AddMsg(JObject value)
        {
            int userId = (int)value["UserId"];
            string objType = (string)value["ObjType"];
            string msg = (string)value["Msg"];
            int objIdx = (int)value["ObjIdx"];
            bool result=Handle.AddMsg(msg,userId,objType,objIdx);
            JObject json = new JObject();
            json["Result"] = result;
            return json;
        }
        [HttpPost("adduser")]
        public JObject AddUser(JObject value)
        {
            string openId = (string)value["OpenId"];
            string nickName = (string)value["Nickname"];
            bool result = Handle.AddUser(nickName, openId);
            JObject json = new JObject();
            json["Result"] = result;
            return json;
        }
        [HttpPost("GetMsg")]
        public JArray GetMsg()
        {
            JArray result = new JArray();
            DataTable dt= Handle.GetMsg();
            foreach (DataRow row in dt.Rows)
            {
                JObject item = new JObject();
                item["ID"] = (int)row["ID"];
                item["UserID"] = (int)row["UserID"];
                item["Message"] = (string)row["Message"];
                item["CreateTime"] = (DateTime)row["CreateTime"];
                item["ObjectIndex"] = (int)row["ObjectIndex"];
                item["ObjectType"] = (string)row["ObjectType"];
                result.Add(item);
            }
            return result;
        }
        [HttpPost("getuserid")]
        public JObject GetUserId(JObject jsonIn)
        {
            JObject json = new JObject();
            json["Result"] = -1;
            if (jsonIn == null|| jsonIn["OpenId"]==null)
                return json;
             json["Result"]=Handle.GetUserId(jsonIn["OpenId"].ToString());
            return json;
        }
        [HttpPost("setmsgreaded")]
        public JObject SetMsgReaded(JObject jsonIn)
        {
            int id = (int)jsonIn["ID"];
            JObject json = new JObject();
            json["Result"]= Handle.SetMsgReaded(id);
            return json;
        }
    }
}
