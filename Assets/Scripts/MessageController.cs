using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public Message preText;
    public static MessageController Instance;
    public int lineCount = 5;

    const int maxY = 1000;
    const int maxX = 2880;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
    public void CreateText(string text,string imgUrl)
    {
        int y = maxY;
        for (int j = maxY; j > maxY-80*lineCount; j-=80)
        {
            bool match = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child != null)
                {
                    if(child.position.y==j)
                    {
                        match = true;
                        break;
                    }
                }
            }
            if (!match)
            {
                y = j;
                break;
            }
        }
        Vector3 position = new Vector3(maxX, y, 0);
        Message newItem=Instantiate(preText, position, Quaternion.identity, transform);
        newItem.SetMessage(text,imgUrl);
    }

}
