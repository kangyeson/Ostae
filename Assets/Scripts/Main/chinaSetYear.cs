using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class chinaSetYear : MonoBehaviour
{
    Touch touch;
    public TextMeshProUGUI cyear;


    private void Update(){
        int i=GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID();

        switch (i){
            case 0: cyear.text = ("17세기"); break;
            case 1: cyear.text = ("18세기"); break;
            case 2: cyear.text = ("19세기"); break;
            case 3: cyear.text = ("20세기"); break;
            case 4: cyear.text = ("6~8세기"); break;
            case 5: cyear.text = ("9~12세기"); break;
            case 6: cyear.text = ("13세기"); break;
            case 7: cyear.text = ("15~16세기"); break;
        } 
    }
    
}
