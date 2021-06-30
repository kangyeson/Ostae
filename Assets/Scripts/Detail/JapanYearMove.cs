using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JapanYearMove : MonoBehaviour
{
    private bool cardb;
    private bool startb = true;
    private int centernum;

    void Update()
    {
        if(startb == true){
            int startnum = MyApplication.centerClothe;
            GameObject.Find("Djapanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
            if(GameObject.Find("Djapanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == startnum){
                GameObject.Find("Djapanlist").GetComponent<ListPositionCtrl>().Stop();
                Debug.Log(GameObject.Find("Djapanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID());
                startb = false;
            }
        } else {
            Centernum=GameObject.Find("Djapanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID();
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
                GameObject.Find("Jyearline").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
                centernum = value;
                cardb = true;
                FallCard(); 
                if(GameObject.Find("Jyearline").GetComponent<ListPositionCtrl>().GetCenteredContentID() == value){
                        GameObject.Find("Jyearline").GetComponent<ListPositionCtrl>().Stop();
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
        GameObject.Find("Djapanlist").GetComponent<ListPositionCtrl>().MoveOneUnitUp(); 
        if(GameObject.Find("Djapanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID() == centernum){
                GameObject.Find("Djapanlist").GetComponent<ListPositionCtrl>().Stop();
        }
    }
}
