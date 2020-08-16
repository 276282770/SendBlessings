using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public Message preText;
    public static MessageController Instance;

    const int maxY = 1050;
    const int maxX = 2880;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
    public void CreateText(string text)
    {
        int y = 0;
        for (int j = 1050; j > 0; j-=60)
        {
            bool match = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i) != null)
                {
                    if(transform.position.y==j)
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
        Vector3 position = new Vector3(maxX, maxY, 0);
        Message newItem=Instantiate(preText, position, Quaternion.identity, transform);
        newItem.SetMessage(text);
    }

}
