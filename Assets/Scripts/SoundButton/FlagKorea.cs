using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagKorea : MonoBehaviour
{
    public AudioClip soundExplosion; //����� �Ҹ��� ������ ����ϴ�.
    AudioSource myAudio; //AudioSorce ������Ʈ�� ������ ����ϴ�.
    public static FlagKorea instance;  //�ڱ��ڽ��� ������ ����ϴ�.
    void Awake() //Start���ٵ� ����, ��ü�� �����ɶ� ȣ��˴ϴ�
    {
        if (FlagKorea.instance == null) //incetance�� ����ִ��� �˻��մϴ�.
        {
            FlagKorea.instance = this; //�ڱ��ڽ��� ����ϴ�.
        }
    }
    void Start()
    {
        myAudio = this.gameObject.GetComponent<AudioSource>(); //AudioSource ������Ʈ�� ������ ����ϴ�.
    }
    public void PlaySound()
    {
        myAudio.PlayOneShot(soundExplosion); //soundExplosion�� ����մϴ�.
    }
    void Update()
    {

    }
}