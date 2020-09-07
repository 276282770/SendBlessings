using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fu : MonoBehaviour
{
    public Animator anim;



    void OnEnable()
    {
        anim.Play("fu_fadeIn");
    }

}
