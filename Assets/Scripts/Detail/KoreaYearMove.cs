using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoreaYearMove : MonoBehaviour
{
    private bool cardb;
    private bool startb = true;
    private int centernum;

    void Update()
    {
        if(startb == true){
            int startnum = MyApplication.centerClothe;
            GameObject.Find("Dkorealist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
            if(GameObject.Find("Dkorealist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == startnum){
                GameObject.Find("Dkorealist").GetComponent<ListPositionCtrl>().Stop();
                Debug.Log(GameObject.Find("Dkorealist").GetComponent<ListPositionCtrl>().GetCenteredContentID());
                startb = false;
            }
        } else {
            Centernum=GameObject.Find("Dkorealist").GetComponent<ListPositionCtrl>().GetCenteredContentID();
            cardb = false;
        }
    }

//변수 변화 감지 프로퍼티 get, set
    public int Centernum
    {
        get{
            return centernum;
        } 
        set{
            if(value != centernum)
            {
                GameObject.Find("Kyearline").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                centernum = value;
                cardb = true;
                FallCard();
                if(GameObject.Find("Kyearline").GetComponent<ListPositionCtrl>().GetCenteredContentID() == value){
                        GameObject.Find("Kyearline").GetComponent<ListPositionCtrl>().Stop(); 
                }
            } else {
                centernum = value;
                cardb = false;
            }
        }
    } 

    public void FallCard()
    {
        if(cardb == true){
            CardDrop.instance.CardFallAway();
            cardb = false;
        }
    }
            
}
