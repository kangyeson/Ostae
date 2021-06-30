using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public AudioSource myInfo;
    public AudioClip backClip;
    public AudioClip clickClip;

    public void clickSound()
    {
        myInfo.PlayOneShot(clickClip);
    }
}
