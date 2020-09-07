using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
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
       // HttpClient client = new HttpClient();
       //HttpResponseMessage response= client.GetAsync(headImgUrl).Result;
       // byte[] buffer = response.Content.ReadAsByteArrayAsync().Result;
       // UnityWebRequest www = UnityWebRequestTexture.GetTexture(headImgUrl);
       // Texture2D texture =DownloadHandlerTexture.GetContent(www);

        StartCoroutine("GetText", headImgUrl);
        
    }
    IEnumerator GetText(string imgUrl)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imgUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }
}
