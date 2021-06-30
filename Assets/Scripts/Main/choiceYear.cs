using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class choiceYear : MonoBehaviour
{
    Touch touch;
    public TextMeshProUGUI kyear;
    public TextMeshProUGUI cyear;
    public TextMeshProUGUI jyear;
    public GameObject kchoice;
    public GameObject cchoice;
    public GameObject jchoice;
    
    private void Update(){
        int i=GameObject.Find("line").GetComponent<ListPositionCtrl>().GetCenteredContentID();

        switch (i){
            case 4: kyear.text = ("6~8세기");
                    cyear.text = ("6~8세기");
                    jyear.text = ("6~8세기"); 
                    GameObject.Find("korealist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("korealist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 4){
                            GameObject.Find("korealist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 4){
                            GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 4){
                            GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().Stop(); 
                    } break;
            case 3: kyear.text = ("9~12세기");
                    cyear.text = ("9~12세기");
                    jyear.text = ("9~12세기");
                    GameObject.Find("korealist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("korealist").GetComponent<ListPositionCtrl>().GetCenteredContentID() ==5){
                            GameObject.Find("korealist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 5){
                            GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 5){
                            GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().Stop(); 
                    } break;
            case 2: kyear.text = ("13세기");
                    cyear.text = ("13세기");
                    jyear.text = ("13세기"); 
                    GameObject.Find("korealist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("korealist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 6){
                            GameObject.Find("korealist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 6){
                            GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 6){
                            GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().Stop(); 
                    } break;
            case 1: kyear.text = ("15~16세기");
                    cyear.text = ("15~16세기");
                    jyear.text = ("15~16세기"); 
                    GameObject.Find("korealist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("korealist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 7){
                            GameObject.Find("korealist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 7){
                            GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 7){
                            GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().Stop(); 
                    }  break;
            case 0: kyear.text = ("17세기");
                    cyear.text = ("17세기");
                    jyear.text = ("17세기");
                    GameObject.Find("korealist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("korealist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 0){
                            GameObject.Find("korealist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 0){
                            GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 0){
                            GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().Stop(); 
                    }  break;
            case 7: kyear.text = ("18세기");
                    cyear.text = ("18세기");
                    jyear.text = ("18세기"); 
                    GameObject.Find("korealist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("korealist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 1){
                            GameObject.Find("korealist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 1){
                            GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 1){
                            GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().Stop(); 
                    }  break;
            case 6: kyear.text = ("19세기");
                    cyear.text = ("19세기");
                    jyear.text = ("19세기"); 
                    GameObject.Find("korealist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("korealist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 2){
                            GameObject.Find("korealist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 2){
                            GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 2){
                            GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().Stop(); 
                    }  break;
            case 5: kyear.text = ("20세기");
                    cyear.text = ("20세기");
                    jyear.text = ("20세기"); 
                    GameObject.Find("korealist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("korealist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 3){
                            GameObject.Find("korealist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 3){
                            GameObject.Find("chinalist").GetComponent<ListPositionCtrl>().Stop(); 
                    }
                    GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 3){
                            GameObject.Find("japanlist").GetComponent<ListPositionCtrl>().Stop(); 
                    }  break;
        }
    }

}
