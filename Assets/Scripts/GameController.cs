using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static string configPath = Application.dataPath+"/config.txt";
    public static GameController Instance;
    public GameObject[] scenes;
    Net net = new Net();
    public ObjectType objectType;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        GetScenes();
    }
    void Update()
    {

    }
    void GetSettingType()
    {
        int sceneIdx=net.GetSettingType();
        scenes[sceneIdx].SetActive(true);
        objectType = (ObjectType)sceneIdx;
    }
    bool SetSettingType()
    {

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
        FileStream fs = fi.OpenRead();
        byte[] buffer = new byte[fs.Length];
        fs.Read(buffer, 0, (int)fs.Length);
        Texture2D texture = new Texture2D(size.Width, size.Height);
        Debug.Log(texture.width + " " + texture.height);
        texture.LoadImage(buffer);
        Sprite result = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        return result;
    }
}
