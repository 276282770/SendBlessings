using ClientController;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Net : MonoBehaviour
{
    public int timeRate = 2;
    // Start is called before the first frame update
    Udp udpClient;
    string ip;
    int point;
    string serverUrlBase = "http://zf.cyhdzy.com:81/API/";
    //string serverUrlBase = "http://zf.cracre.vip:81/API/";
    //string serverUrlBase = "http://localhost:5000/API/";
    HttpClient client = new HttpClient();
    public bool isReceive = false;

    float count=0;
    List<int> playedIds = new List<int>();
    void Start()
    {
        //if (udpClient == null)
        //    udpClient = new Udp();
        //ReadIPEndPoint();
        //Debug.Log(ip+" "+point);
        //SendEcho();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (((int)Time.time) % 300 == 0)
        //{
        //    SendEcho();
        //    Debug.Log(Time.time);
        //}
        if (isReceive&& count <= 0)
        {
            count = timeRate;
            ReceiveMsg();
        }
        else
        {
            count -= Time.deltaTime;
        }
    }
    private void OnDestroy()
    {
        if(udpClient!=null)
        udpClient.Close();
        udpClient = null;
    }

    void SendEcho()
    {
        string data = "echo";
        byte[] buffer = Encoding.UTF8.GetBytes(data);
        udpClient.Send(buffer, ip, point);
    }
    void ReadIPEndPoint()
    {
        string fileName = Application.dataPath+"/config.ini";
        FileInfo fi = new FileInfo(fileName);
        if(!fi.Exists)
        {
            return;
        }
        using (StreamReader sr = new StreamReader(fi.OpenRead()))
        {
            string line;
            do
            {
                line = sr.ReadLine();
                if (line != null)
                {
                    string[] kv = line.Split('=');
                    if (kv[0] == "ip")
                    {
                        ip = kv[1];
                    }
                    if (kv[0] == "port")
                        point =int.Parse( kv[1]);
                }
            }
            while (line != null);
        }
    }
    async System.Threading.Tasks.Task ReceiveAsync()
    {
        string msg=await udpClient.Receive();
        JObject ret = JObject.Parse(msg);
        //ObjectController.Instance.Create((int)ret["index"],(string)ret["type"]);
        ReceiveAsync();
    }
    public async void ReceiveMsg()
    {
        isReceive = false;
        try
        {
            string urlGetMsg = serverUrlBase + "GetMsg";
            string urlSetMsgReaded = serverUrlBase + "setmsgreaded";
            //HttpResponseMessage response=await client.PostAsync(urlGetMsg, null);
            JObject jGetMsgPara = new JObject();
            jGetMsgPara["TPIDX"] = (int)GameController.Instance.objectType;
            string result = await PostAsync(urlGetMsg, jGetMsgPara.ToString());
            JArray json = JArray.Parse(result);
            if (json == null)
                return;
            if (json.Count == 0)
                return;
            Debug.Log($"[{DateTime.Now}]接收到{json.Count}条消息.");
            for (int i = 0; i < json.Count; i++)
            {
                int id = (int)json[i]["ID"];
                foreach (int pid in playedIds)
                {
                    if (id == pid)
                        return;
                }


                ObjectType type = (ObjectType)Enum.Parse(typeof(ObjectType), (string)json[i]["ObjectType"]);
                int index = (int)json[i]["ObjectIndex"];
                string text = $"【{json[i]["Nickname"]}】{json[i]["Message"]}";
                string imgUrl = (string)json[i]["Headimgurl"];
                ObjectController.Instance.Create(index, type, text, imgUrl);

                Debug.Log($"[{DateTime.Now}]播放#{id}消息");

                JObject jSetMsgReaded = new JObject();
                jSetMsgReaded["ID"] = json[i]["ID"];
                string retSetMsgReaded = await PostAsync(urlSetMsgReaded, jSetMsgReaded.ToString());
                Debug.Log(retSetMsgReaded);
                playedIds.Add(id);
            }
        }
        catch(Exception ex)
        {
            Debug.Log("出错："+ex.Message);
        }
        finally
        {
            isReceive = true;
        }
        
    }
    public async Task<string> PostAsync(string url,string jParameters)
    {
        
        if (jParameters == null)
            jParameters = new JObject().ToString();
        StringContent content = new StringContent(jParameters);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        HttpResponseMessage response = await client.PostAsync(url, content);
        string sResult = response.Content.ReadAsStringAsync().Result;
        return sResult;
    }
    public int GetSettingType()
    {
        string url =serverUrlBase+"GetSettingType";
        StringContent content = new StringContent("{}");
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        HttpResponseMessage response= client.PostAsync(url, content).Result;
        string result = response.Content.ReadAsStringAsync().Result;
        //string result =await PostAsync(url, null);

        int idx = -1;
        if (result != null)
        {
            JObject jRet = JObject.Parse(result);
            int.TryParse((string)jRet["Data"], out idx);
        }
        return idx;
    }

    public bool SetSettingType(int i)
    {
        string url = $"{serverUrlBase}SetSettingType";
        JObject jData = new JObject();
        jData["index"] = i;
        StringContent content = GetContent(jData.ToString());
        HttpResponseMessage response= client.PostAsync(url, content).Result;
        string resultString = response.Content.ReadAsStringAsync().Result;
        JObject jRet = JObject.Parse(resultString);
        bool result =  (bool)jRet["Result"];
        return result;
    }
    StringContent GetContent(string json)
    {
        StringContent content = new StringContent(json);
        content.Headers.ContentType= new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
        return content;
    }
}
