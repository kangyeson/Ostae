using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sutter : MonoBehaviour
{
    public AudioSource mySutter;
    public AudioClip backClip;
    public AudioClip clickClip;

    public void clickSound()
    {
        mySutter.PlayOneShot(clickClip);
    }
}
