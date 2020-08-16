using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{

    
    RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
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
    public void SetMessage(string msg)
    {
       GetComponent<Text>().text = msg;
    }
}
