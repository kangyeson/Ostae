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

    [Header("사운드 등록")]
    [SerializeField] Sound[] sfxSounds;

    [Header("효과음 플레이어")]
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
                Debug.Log("모든 효과 플레이어가 사용중입니다!.");
                return;
            }
        }
        Debug.Log("등록된 사운드가 없습니다.");
    }

}