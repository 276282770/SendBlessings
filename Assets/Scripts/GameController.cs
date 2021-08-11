using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    string configPath ;
    //static string bgLightPath = Application.dataPath+"/bgLight.jpg";
    //static string bgFireworkPath = Application.dataPath+"/bgFirework.jpg";
    //static string bgTreePath = Application.dataPath+"/bgTree.jpg";
    public static GameController Instance;
    public GameObject[] scenes;
    public string[] bgPaths;
    Net net;
    public ObjectType objectType;

    private void Awake()
    {
        objectType = ObjectType.firework;
        configPath = Application.dataPath + "/config.txt";
        bgPaths = new string[3];
        bgPaths[0] = Application.dataPath + "/bgLight.jpg";
        bgPaths[1] = Application.dataPath + "/bgFirework.jpg";
        bgPaths[2] = Application.dataPath + "/bgTree.jpg";
        net = GetComponent<Net>();
        LoadBg();
        GetSettingType();
    }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
  
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1)){
            SetSettingType(0);
        } 
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            SetSettingType(1);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            SetSettingType(2);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    void LoadBg()
    {
        float cameraHeight= Camera.main.orthographicSize * 2;
        float cameraWidth = Camera.main.aspect * cameraHeight;
        
        for (int i=0;i<scenes.Length;i++)
        {
            LoadBg(i);
            //调整背景大小
            //Vector3 scale=scenes[i]
        }
        
    }
    
    void LoadBg(int i)
    {
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = Camera.main.aspect * cameraHeight;
        Size screenSize = new Size(1920, 1080);
        SpriteRenderer bg = scenes[i].GetComponent<SpriteRenderer>();
        Sprite sprite=GetSprite(bgPaths[i], screenSize);
        if (sprite == null)
            return;
        bg.sprite = sprite;
        Vector3 scale = scenes[i].transform.localScale;
        scale.y *= cameraHeight / screenSize.Height*100;
        scale.x *= cameraWidth / screenSize.Width*100;
        scenes[i].transform.localScale = scale;

        Debug.Log($"=加载场景{i}");
    }
    void GetSettingType()
    {
        int sceneIdx=net.GetSettingType();
        if (sceneIdx != -1)
        {
            SetScene(sceneIdx);
            net.isReceive = true;
        }
    }
    void SetSettingType(int i)
    {
        bool result = net.SetSettingType(i);
        if(result)
        {
            SetScene(i);
        }
        Debug.Log($"=发送设置场景请求  {result}");
    }
    void SetScene(int idx)
    {
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].SetActive(i==idx);
        }
        objectType = (ObjectType)idx;
        Debug.Log($"=设置场景#{idx}");
    }


    string GetConfig(string key)
    {
        Dictionary<string, string> config = ReadConfigFile();
        return config[key];
    }
    void SetConfig(string value1,string value2)
    {
        Dictionary<string, string> config = ReadConfigFile();
        config[value1] = value2;
        StreamWriter sw = new StreamWriter(configPath);
        foreach (string key in config.Keys)
        {
            sw.WriteLine($"{key}={config[key]}");
        }
        sw.Close();
    }
    public void CreateFile()
    {
        FileInfo fi = new FileInfo(Application.dataPath + "/config.dat");
        fi.Create();
        Debug.Log(fi.FullName);
    }
    public Dictionary<string, string> ReadConfigFile()
    {
        FileInfo fi = new FileInfo(configPath);
        Dictionary<string, string> result = new Dictionary<string, string>();
        if (!fi.Exists)
            return null;
        StreamReader sr = new StreamReader(fi.OpenRead());
        string line = sr.ReadLine();
        while (line != null)
        {
            Debug.Log("ReadLine=" + line);
            string[] keyAndValue = line.Split('=');
            result[keyAndValue[0].Trim()] = keyAndValue[1].Trim();
            line = sr.ReadLine();
        }
        return result;
    }
    public void ReadParam(out int health, out int addHealth, out float runTime)
    {
        Dictionary<string, string> config = ReadConfigFile();
        health = int.Parse(config["MaxHealth"]);
        addHealth = int.Parse(config["FoodValue"]);
        runTime = float.Parse(config["Inertia"]);
    }
    public Sprite GetSprite(string path,Size size)
    {
        FileInfo fi = new FileInfo(path);
        if (!fi.Exists)
            return null;
        FileStream fs = fi.OpenRead();
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, (int)fs.Length);
        Texture2D texture = new Texture2D(size.Width, size.Height);
        texture.LoadImage(buffer);
        Sprite result = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return result;
    }
}
