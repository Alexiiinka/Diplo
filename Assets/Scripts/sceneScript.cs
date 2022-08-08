using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneScript : MonoBehaviour
{
    public GameObject fdPf, ltPf, rtPf, repeatPf, stopRepPf, onePf, twoPf, threePf, fourPf, fivePf, sixPf ; //forwardPrefab etc.. 
    public Vector3 startingPoint = new Vector3(-11.93f, -6.43f, -6.0f);
    public float distanceBetween = 1.1f;
    float startPointX = -11.9f, endPointX = 12.0f;
    BoxCollider2D sizeOfCard;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        startPointX = startingPoint[0];
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void canTrashMove()
    {
        if (startPointX >= endPointX)
        {
            canMove = false;
        }
    }

    public void putFdCard()
    {
        if (startPointX < endPointX)
        {
            Instantiate(fdPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), fdPf.transform.rotation);
            sizeOfCard = fdPf.GetComponent<BoxCollider2D>();
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
        }
        canTrashMove();
        
    }
    public void putLeftCard()
    {
        if (startPointX < endPointX)
        {
            Instantiate(ltPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), ltPf.transform.rotation);
            sizeOfCard = ltPf.GetComponent<BoxCollider2D>();
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
        }
        canTrashMove();
        
    }
    public void putRightCard()
    {
        if (startPointX < endPointX)
        {
            Instantiate(rtPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), rtPf.transform.rotation);
            sizeOfCard = rtPf.GetComponent<BoxCollider2D>();
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
        }
        canTrashMove();
        
    }
}
