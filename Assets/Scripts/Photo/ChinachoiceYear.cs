using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class ChinachoiceYear : MonoBehaviour
{
    Touch touch;
    
    private void Update(){
        int i=GameObject.Find("line").GetComponent<ListPositionCtrl>().GetCenteredContentID();

        switch (i){
            case 4: 
                    GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 4){
                            GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().Stop(); 
                    } break;
            case 3: 
                    GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID() ==5){
                            GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().Stop(); 
                    } break;
            case 2: 
                    GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 6){
                            GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().Stop(); 
                    } break;
            case 1: 
                    GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 7){
                            GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().Stop(); 
                    }  break;
            case 0: 
                    GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 0){
                            GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().Stop(); 
                    } break;
            case 7: 
                    GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 1){
                            GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().Stop(); 
                    }  break;
            case 6: 
                    GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 2){
                            GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().Stop(); 
                    }  break;
            case 5: 
                    GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                    if(GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == 3){
                            GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().Stop(); 
                    } break;
        }
    }

}
