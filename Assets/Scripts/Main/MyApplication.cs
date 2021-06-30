using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyApplication : MonoBehaviour
{
    public ListPositionCtrl list;
    public static int centerClothe;

    public void GetSelectedContentID(int selectedContentID)
    {
        centerClothe = list.GetCenteredContentID();
        SceneManager.LoadSceneAsync(selectedContentID);
        Debug.Log("nextScene: " + selectedContentID +
                  ", clothe: " + centerClothe);
    }
}
