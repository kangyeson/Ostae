using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChinaYearMove : MonoBehaviour
{
    private bool cardb;
    private bool startb = true;
    private int centernum;

    void Update()
    {
        if(startb == true){
            int startnum = MyApplication.centerClothe;
            GameObject.Find("Dchinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
            if(GameObject.Find("Dchinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == startnum){
                GameObject.Find("Dchinalist").GetComponent<ListPositionCtrl>().Stop();
                Debug.Log(GameObject.Find("Dchinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID());
                startb = false;
            }
        } else {
            Centernum=GameObject.Find("Dchinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID();
            cardb = false;
        }
    }

    public int Centernum
    {
        get{
            return centernum;
        } 
        set{
            if(value != centernum)
            {
                GameObject.Find("Cyearline").GetComponent<ListPositionCtrl>().MoveOneUnitUp();
                centernum = value;
                cardb = true;
                FallCard(); 
                if(GameObject.Find("Cyearline").GetComponent<ListPositionCtrl>().GetCenteredContentID() == value){
                        GameObject.Find("Cyearline").GetComponent<ListPositionCtrl>().Stop(); 
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

    void Start()
    {
        int centernum = PlayerPrefs.GetInt("centerClothe");
        GameObject.Find("Dchinalist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
        if(GameObject.Find("Dchinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == centernum){
                GameObject.Find("Dchinalist").GetComponent<ListPositionCtrl>().Stop();
        }
    }
}
