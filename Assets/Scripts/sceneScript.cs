using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneScript : MonoBehaviour
{
    public GameObject fdPf, ltPf, rtPf, repeatPf, stopRepPf, onePf, twoPf, threePf, fourPf, fivePf, sixPf ; //forwardPrefab etc.. 
    public Vector3 startingPoint = new Vector3(-11.93f, -6.43f, -6.0f);
    public float distanceBetween = 1.1f, distanceForNumber = 2.0f, tuningOfBigRepeatCard = 1.5f;
    float startPointX = -11.9f, endPointX = 12.0f;
    BoxCollider2D sizeOfCard;
    public bool canMove = true;
    private List<int> instructs; //1-6> pocet opakovani, 7-zac. opak, 8-koniec opak., 10-fd, 11-right, 12-left
    public int indexOfInstructs = 0;
    public List<int> cycleRunning, cycleRunning2; //2 len aby som si niekde drzala hodnoty az do vykonania instrukcii
    public bool needNumberInCycle = false;
    trashMove trashSc;

    // Start is called before the first frame update
    void Start()
    {
        startPointX = startingPoint[0];
        trashSc = GameObject.Find("Kos").GetComponent<trashMove>();
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
        if (startPointX < endPointX && needNumberInCycle == false)
        {
            Instantiate(fdPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), fdPf.transform.rotation);
            sizeOfCard = fdPf.GetComponent<BoxCollider2D>();
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
            if (cycleRunning.Count != 0)
            {
                instructs.Add(10);
            } 
        }
        canTrashMove();
        
    }
    public void putLeftCard()
    {
        if (startPointX < endPointX && needNumberInCycle == false)
        {
            Instantiate(ltPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), ltPf.transform.rotation);
            sizeOfCard = ltPf.GetComponent<BoxCollider2D>();
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
            if (cycleRunning.Count != 0)
            {
                instructs.Add(12);
            } 
        }
        canTrashMove();
        
    }
    public void putRightCard()
    {
        if (startPointX < endPointX && needNumberInCycle == false)
        {
            Instantiate(rtPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), rtPf.transform.rotation);
            sizeOfCard = rtPf.GetComponent<BoxCollider2D>();
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
            if (cycleRunning.Count != 0)
            {
                instructs.Add(11);
            } 
        }
        canTrashMove();
        
    }
    public void putRepeatCard()
    {   
        startPointX += tuningOfBigRepeatCard; //doladenie posunutia pre velke opakuj
        Instantiate(repeatPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]+0.1f), repeatPf.transform.rotation);
        sizeOfCard = repeatPf.GetComponent<BoxCollider2D>();
        startPointX += sizeOfCard.size[0]/2.5f - distanceForNumber; //doladenie pre polozenie cisla do OPAKUJ karticky
        canTrashMove();  
    }
    public void putStopCard()
    {   
        Instantiate(stopRepPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), stopRepPf.transform.rotation);
        sizeOfCard = stopRepPf.GetComponent<BoxCollider2D>();
        startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
        canTrashMove();  
    }

    public void PutOne() { PutNumber(onePf, 1);}
    public void PutTwo() { PutNumber(twoPf, 2);}
    public void PutThree() { PutNumber(threePf, 3);}
    public void PutFour() { PutNumber(fourPf, 4);}
    public void PutFive() { PutNumber(fivePf, 5);}
    public void PutSix() { PutNumber(sixPf, 6);}

    private void PutNumber(GameObject prefabNum, int numbr)
    {
        if (needNumberInCycle == true)
        {
            Instantiate(prefabNum, new Vector3(startPointX, startingPoint[1], startingPoint[2]), prefabNum.transform.rotation);
            sizeOfCard = prefabNum.GetComponent<BoxCollider2D>();
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
            needNumberInCycle = false;
            cycleRunning[cycleRunning.Count-1] = numbr;
            cycleRunning2[cycleRunning2.Count-1] = numbr;

        }
        canTrashMove();
    }

    public void RepeatCycle()
    { 
        if (startPointX < endPointX && needNumberInCycle == false)
        {
            if (cycleRunning.Count == 0)
            {
                Debug.Log("IstNull");
                putRepeatCard();
                indexOfInstructs = 0;
                cycleRunning = new List<int>();
                instructs = new List<int>();
                cycleRunning.Add(55);
                cycleRunning2.Add(55);
                needNumberInCycle = true;
                canMove = false;
            }
            else
            {   
                putRepeatCard();
                cycleRunning.Add(55);
                cycleRunning2.Add(55);
                needNumberInCycle = true;
            }
        }
    }
    public void StopRepeat()
    {
        if (startPointX < endPointX && (cycleRunning.Count != 0) && needNumberInCycle == false)
        {
            putStopCard();
            instructs.Add(8);
            cycleRunning.RemoveAt(cycleRunning.Count-1);
        }
        if (cycleRunning.Count == 0)
        {
            canMove = true;
            RepeatInstructions(0);
            instructs.Clear();
            cycleRunning2.Clear();
        }
    }
    private void RepeatInstructions(int indexOfRepeat)
    {
        for (int i = 0; i < cycleRunning2[indexOfRepeat]; i++)
        {
            Debug.Log(i);
            if(instructs[indexOfInstructs] == 10){trashSc.moveFd();}
            if(instructs[indexOfInstructs] == 11){trashSc.rightRotate();}
            if(instructs[indexOfInstructs] == 12){trashSc.leftRotate();}
        }
    }
}
