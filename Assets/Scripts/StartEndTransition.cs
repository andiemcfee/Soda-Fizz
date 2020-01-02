using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndTransition : MonoBehaviour
{
    [SerializeField] Animator anim;


    public void SetFBoolTrue()
    {
        anim.SetBool("isFinished", true);
    } 

    public void SetFBoolFalse()
    {
        anim.SetBool("isFinished", false);
    }

    public void SetRBoolTrue()
    {
        anim.SetBool("isRestarted", true);
    }

    public void SetRBoolFalse()
    {
        anim.SetBool("isRestarted", false);
    }
}
