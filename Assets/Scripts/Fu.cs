using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fu : MonoBehaviour
{

    public float lift = 60;


    private void Update()
    {
        if (lift <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            lift -= Time.deltaTime;
        }
    }

}
