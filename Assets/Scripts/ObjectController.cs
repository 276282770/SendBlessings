﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectController : MonoBehaviour
{
    public Firework[] preFireworks;
    public Light[] preLights;
    public GameObject preFu;
    public Transform[] parents;
    public int margin = 100;
    public static ObjectController Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            int idx = Random.Range(0, 9);
            CreateFireworks(idx);
        }
        if(Input.GetMouseButtonDown(1))
        {
            int idx = Random.Range(0, 9);
            CreateLight(idx);
        }
        if(Input.GetMouseButtonDown(2))
        {
            CreateTree();
        }
        
    }
    /// <summary>
    /// 创建烟花
    /// </summary>
    public void CreateFireworks(int index)
    {
        float orthSize = Camera.main.orthographicSize;
        float x = RandomX();
        float y = -(orthSize + 0.5f);
        float top = Random.Range(0, orthSize - 1);
        float scale = Random.Range(0.3f,1f);
        Firework fireworks= Instantiate(preFireworks[index], new Vector3(x, top), Quaternion.identity);
        fireworks.transform.parent = parents[1];
        fireworks.transform.localScale = new Vector3(scale,scale,1);
    }
    public void CreateLight(int index)
    {
        float orthSize = Camera.main.orthographicSize;
        float x = RandomX();
        float y = -(orthSize + 0.5f);
        float scale = Random.Range(0.2f, 0.5f);
        Light light = Instantiate(preLights[index], new Vector3(x, y), Quaternion.identity);
        light.transform.parent = parents[0];
        light.transform.localScale = new Vector3(scale, scale, 1);
    }
    private void CreateTree()
    {
        float x = Random.Range(-3.25f, 3.25f);
        float y = Random.Range(-3.52f, 2.17f);
        Instantiate(preFu, new Vector3(x, y, 0), Quaternion.identity, parents[2].GetChild(0));
    }
    float RandomX()
    {
        int pixelX = Random.Range(-Screen.width+margin, Screen.width - margin);
        float x = pixelX / (Screen.height / Camera.main.orthographicSize);
        return x;
    }
    public void Create(int index,ObjectType type)
    {
        switch (type)
        {
            case ObjectType.firework:CreateFireworks(index);break;
            case ObjectType.light:CreateLight(index);break;
            case ObjectType.tree:CreateTree();break;
        }
    }



    public void Create(int index, ObjectType type,string text,string imgUrl)
    {
        Create(index, type);
        MessageController.Instance.CreateText(text,imgUrl);
    }
}
