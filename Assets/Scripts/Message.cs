using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public string msg;
    public string nickname;
    Text text;
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(rectTransform.rect.>Camera.main.orthographicSize)
        Debug.Log(rectTransform.rect.x);
    }
    public void SetMessage(string msg,string nickname)
    {
        text.text = $"【{nickname}】{msg}";
    }
}
