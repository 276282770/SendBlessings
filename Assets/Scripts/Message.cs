using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEditor.iOS;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Message : MonoBehaviour
{

    
    RectTransform rectTransform;
    Image image;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = transform.GetChild(0).GetComponent<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x>= -960)
        {
            transform.Translate(Vector3.left);
        }
        else
        {
            Destroy(gameObject);
        }
        //Debug.Log(rectTransform.position.x+" "+transform.position.x);
        //Debug.LogError(transform.position.x+" "+transform.position.y);
    }
    public void SetMessage(string msg,string headImgUrl)
    {
       GetComponent<Text>().text = msg;
        HttpClient client = new HttpClient();
       HttpResponseMessage response= client.GetAsync(headImgUrl).Result;
        byte[] buffer = response.Content.ReadAsByteArrayAsync().Result;
        UnityWebRequestTexture
        image.sprite = ;
    }
}
