using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Fireworks : MonoBehaviour
{
   
    public float speed=10;
    public float x=0;
    public float life = 15;
    public float scale = 1;
    Animator anim;
    bool bombing;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if(transform.position.y<bombHeight)
        //{
        //    transform.Translate(Vector2.up * speed * Time.deltaTime);
        //    if (transform.position.y >= bombHeight)
        //        Bomb();
        //}
        if (life < 0)
        {
            Destroy(gameObject);
        }
        else
            life -= Time.deltaTime;

    }
    void Bomb()
    {
        anim.Play("Bombing");
        Invoke("DestroyObj", 10f);
    }
    void DestroyObj()
    {
        Destroy(gameObject);
    }
}
