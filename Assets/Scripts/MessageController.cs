using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public Message preText;
    public static MessageController Instance;
    public int lineCount = 5;

    int maxY = 850;
    //const int maxX = 2880;
    int maxX = 2500;



    int lastLine = 0;
    float time = 5;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //maxY = Screen.height-50;
        //maxX = Screen.width+900;
        maxY = 0;
        maxX = Screen.width;
    }
    private void Update()
    {
        if (time<0&&lastLine > 0)
        {
            lastLine--;
            time = 5;
        }
        else
        {
            time -= Time.deltaTime;
        }
        if(Input.GetMouseButtonDown(0))
        {
            CreateText("你好，佩奇", "https://img2.baidu.com/it/u=2531709812,3517416922&fm=253&fmt=auto&app=120&f=JPEG?w=280&h=180");
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            var child = transform.GetChild(0).GetComponent<RectTransform>();
            //child.position = new Vector3(child.position.x,0);
            child.anchoredPosition = new Vector3(child.position.x,0);
            Debug.Log(child.position);
        }
    }
    public void CreateText(string text,string imgUrl)
    {
        //int y = maxY;
        //for (int j = maxY; j > maxY-80*lineCount; j-=80)
        //{
        //    bool match = false;
        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        Transform child = transform.GetChild(i);
        //        if (child != null)
        //        {
        //            if(child.position.y==j)
        //            {
        //                match = true;
        //                break;
        //            }
        //        }
        //    }
        //    if (!match)
        //    {
        //        y = j;
        //        break;
        //    }
        //}
        //y -= 80 * lastLine;
        //Vector3 position = new Vector3(maxX, y, 0);
        //Message newItem=Instantiate(preText, position, Quaternion.identity, transform);
        //newItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(maxX,y);
        //newItem.SetMessage(text,imgUrl);
        //lastLine++;
        //if(lastLine >= lineCount)
        //lastLine= 0;
        //time = 5;
        var textSize= preText.GetSize();
        int textHeight = 80;
        int lineCount = Screen.height / textHeight;

            int index = Random.Range(0, lineCount);
            int y = -index * textHeight;
            int x = maxX;

            Debug.Log("消息数量："+transform.childCount);
            for(int i=0;i<transform.childCount;i++)
            {
                var child = transform.GetChild(i);
                var childPos = child.GetComponent<Message>().GetPos();
                var childSize = child.GetComponent<Message>().GetSize();

                if (childPos.y != y)
                    continue;
                if (childPos.x+childSize.x > x  )
                {

                        x = (int)childPos.x+(int)childSize.x+50;
                    
                }

            }

                Vector3 position = new Vector3(x, y, 0);
                Message newItem = Instantiate(preText, position, Quaternion.identity, transform);
                newItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
                newItem.SetMessage(text, imgUrl);
                Debug.Log($"创建字幕:{position}");
            
            

    }
    

}
