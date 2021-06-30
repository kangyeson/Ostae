using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
    public AudioSource myBack;
    public AudioClip backClip;
    public AudioClip clickClip;

    public void backSound()
    {
        myBack.PlayOneShot(backClip);
    }

    public void clickSound()
    {
        myBack.PlayOneShot(clickClip);
    }
}
