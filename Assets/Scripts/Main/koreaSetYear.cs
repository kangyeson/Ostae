using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class koreaSetYear : MonoBehaviour
{
    Touch touch;
    public TextMeshProUGUI kyear;
    
    private void Update(){
        int i=GameObject.Find("korealist").GetComponent<ListPositionCtrl>().GetCenteredContentID();

        switch (i){
            case 0: kyear.text = ("17세기"); break;
            case 1: kyear.text = ("18세기"); break;
            case 2: kyear.text = ("19세기"); break;
            case 3: kyear.text = ("20세기"); break;
            case 4: kyear.text = ("6~8세기"); break;
            case 5: kyear.text = ("9~12세기"); break;
            case 6: kyear.text = ("13세기"); break;
            case 7: kyear.text = ("15~16세기"); break;
        }
    }
}
