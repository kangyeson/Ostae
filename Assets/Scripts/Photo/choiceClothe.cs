using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class choiceClothe : MonoBehaviour
{
    public Sprite[] ClotheImage;
    public Image ClotheSpace;
    public int choiceCountry;


    public void SelectClothe()
    {
        Sprite p;
        if(choiceCountry == 0){ //한국
            int i=GameObject.Find("kclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID();
            p = ClotheImage[i];
            ClotheSpace.sprite = p;
        } else if(choiceCountry == 1){ //중국
            int i=GameObject.Find("cclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID();
            p = ClotheImage[i];
            ClotheSpace.sprite = p;
        } else if(choiceCountry == 2){ //일본
            int i=GameObject.Find("jclothelist").GetComponent<ListPositionCtrl>().GetCenteredContentID();
            p = ClotheImage[i];
            ClotheSpace.sprite = p;
        }
    }
}
