using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class CardDrop : MonoBehaviour
{
    public GameObject block;
    public float rotationSpeed;
    public float centeringSpeed;
    public bool singleScene;
    public Sprite[] CardImage;
    public Image CardSpace;
    public int choiceCountry;
    private Rigidbody rbody;
    public bool isFalling;
    private Vector3 cardFallRotation;
    public bool fallToZero;
    private float startXPos;
    private float startYPos;
    private float startZPos;

    public static CardDrop instance;

    void Awake()
    {
        instance = this;
    }
    

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        rbody.useGravity = false;
        startZPos = transform.position.z;
        startXPos = transform.position.x;
        startYPos = transform.position.y;
        
    }

    void Update()
    {
        if (isFalling)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(cardFallRotation), Time.deltaTime * rotationSpeed);
        }

        //이 조건부로 팝업이 제 자리에 딱 들어맞음
        if (fallToZero)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(startXPos, startYPos, startZPos), Time.deltaTime * centeringSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * centeringSpeed);
            if (Vector3.Distance(transform.position, new Vector3(startXPos, startYPos, startZPos)) < 0.025f)
            {
                transform.position = new Vector3(startXPos, startYPos, startZPos);
                fallToZero = false;
            }
        }

        if (transform.position.y < -10)
        {
            moveCare();
            isFalling = false;
            rbody.useGravity = false;
            rbody.velocity = Vector3.zero;
            transform.position = new Vector3(startXPos, 8, startZPos);
            if (singleScene)
            {
                CardEnter();
            }
        }
    }

    public void moveCare()
    {
        Sprite p;

        if(choiceCountry == 0){ //한국
            int i=GameObject.Find("Dkorealist").GetComponent<ListPositionCtrl>().GetCenteredContentID();
            p = CardImage[i];
            CardSpace.sprite = p;
        } else if(choiceCountry == 1){ //중국
            int i=GameObject.Find("Dchinalist").GetComponent<ListPositionCtrl>().GetCenteredContentID();
            p = CardImage[i];
            CardSpace.sprite = p;
        } else if(choiceCountry == 2){ //일본
            int i=GameObject.Find("Djapanlist").GetComponent<ListPositionCtrl>().GetCenteredContentID();
            p = CardImage[i];
            CardSpace.sprite = p;
        }
        
    }
    public void CardEnter()
    {
        fallToZero = true;
        block.gameObject.SetActive(false);
    }

    public void CardFallAway()
    {
        block.gameObject.SetActive(true);
        rbody.useGravity = true;
        isFalling = true;
        cardFallRotation = new Vector3(0, 0, 10);
    }
}