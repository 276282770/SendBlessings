using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y<Camera.main.orthographicSize+1)
        {
            transform.Translate((Vector3.up + Vector3.right * 0.1f)*Time.deltaTime*speed);
            if(transform.position.y >= Camera.main.orthographicSize + 1)
            {
                Destroy(gameObject);
            }
        }
    }

}
