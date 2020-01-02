using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour
{

    public AudioSource buttonClickSfx;
    public AudioSource nextLvlSfx;

    public void playButtonClickSfx()
    {
        buttonClickSfx.Play();
    }

    public void playNextLvlSfx()
    {
        nextLvlSfx.Play();
    }
}
