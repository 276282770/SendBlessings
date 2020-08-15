using ClientController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http;

public class Net : MonoBehaviour
{
    // Start is called before the first frame update
    Udp udpClient;
    string ip;
    int point;
    string serverUrl;
    HttpClient client=new HttpClient();
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
        ObjectController.Instance.Create((int)ret["index"],(string)ret["type"]);
        ReceiveAsync();
    }
    public async void GetMsg()
    {
        //StringContent content = new StringContent();
        HttpResponseMessage response=await client.PostAsync(serverUrl, null);
    }
}
