using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}
public class TestButton : MonoBehaviour {
    public static TestButton instance;

    [Header("���� ���")]
    [SerializeField] Sound[] sfxSounds;

    [Header("ȿ���� �÷��̾�")]
    [SerializeField] AudioSource[] sfxPlayer;

    void Start()
    {
        instance = this;
    }

    public void playerSE(string _soundName)
    {
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (_soundName == sfxSounds[i].soundName)
            {
                for (int x = 0; x < sfxPlayer.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying)
                    {
                        sfxPlayer[x].clip = sfxSounds[i].clip;
                        sfxPlayer[x].Play();
                        return;
                    }
                }
                Debug.Log("��� ȿ�� �÷��̾ ������Դϴ�!.");
                return;
            }
        }
        Debug.Log("��ϵ� ���尡 �����ϴ�.");
    }

}