using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MoveNextScene : MonoBehaviour
{
    public int NextSceneNumber;
    public GameObject Mapsplash;

    
    public void move()
    {
        SceneManager.LoadScene(NextSceneNumber);
    }

    public void SplashScene()
    {
        Mapsplash.gameObject.SetActive(true);
        move();
    }

}