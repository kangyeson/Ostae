using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;


public class japanSetYear : MonoBehaviour
{
    Touch touch;
    public TextMeshProUGUI jyear;

    private void Update(){
        int i=GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID();

        switch (i){
            case 0: jyear.text = ("17세기"); break;
            case 1: jyear.text = ("18세기"); break;
            case 2: jyear.text = ("19세기"); break;
            case 3: jyear.text = ("20세기"); break;
            case 4: jyear.text = ("6~8세기"); break;
            case 5: jyear.text = ("9~12세기"); break;
            case 6: jyear.text = ("13세기"); break;
            case 7: jyear.text = ("15~16세기"); break;
        }
        
    }
}
