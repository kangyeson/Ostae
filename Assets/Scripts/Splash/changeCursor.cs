using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeCursor : MonoBehaviour
{
    public Texture2D Whitecursor;
    private Vector3 hotSpot;
    public void Start(){
        StartCoroutine ("MyCursor");
    }

    IEnumerator MyCursor()
    {
        yield return new WaitForEndOfFrame();
        hotSpot.x = Whitecursor.width / 2;
        hotSpot.y = Whitecursor.height / 2;
        hotSpot.z = -342;
        Cursor.SetCursor(Whitecursor, hotSpot, CursorMode.ForceSoftware);
    }
}
