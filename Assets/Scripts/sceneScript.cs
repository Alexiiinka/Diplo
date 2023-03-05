using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class sceneScript : MonoBehaviour
{
    public bool planned = false;
    private bool plannedReserve = false;
    public GameObject fdPf, ltPf, rtPf, repeatPf, stopRepPf, repeatPfv, stopRepPfv, onePf, twoPf, threePf, fourPf, fivePf, sixPf ; //forwardPrefab etc.. 
    public Vector3 startingPoint = new Vector3(-11.93f, -6.43f, -6.0f); // prvy prikaz do listy prikazov
    public float distanceBetween = 1.1f, distanceForNumber = 2.0f, tuningOfBigRepeatCard = 1.5f, maxXforRepeat = 10.34f; 
    //vzdialenost medzi kartickami, vzdialenost pre cisla v karticke, doladenie velkej karty repeat, maximalne x pre repeat
    private float startPointX = -11.9f; //startingpoint[0],
    public float endPointX = 12.0f;  //suradnica x pre posledny prikaz v liste prikazov
    BoxCollider2D sizeOfCard;
    public bool canMove = true; //ked je unplanned tak predef je true
    public List<int> instructs; //-- zapis instrukcii -- 1-6> pocet opakovani, 7-zac. opak, 8-koniec opak., 10-fd, 11-right, 12-left
    public List<int> indexOfInstructs; //na kolkej sme instrukcii
    public List<int> cycleRunning, cycleRunning2; //cyc2 len aby som si niekde drzala hodnoty az do vykonania instrukcii
    public bool needNumberInCycle = false;
    trashMove trashSc;
    public Sprite repeatButt,repeatButtv, stopButt, stopButtv; //"v" for violet, tito su tu preto, aby sme zmenili vzhlad buttonov
    public Button b_RepeatButt, b_StopButt;//tychto buttonov pri kliknuti na nich
    public GameObject panel;
    public GameObject tile;
    public int muchTilesX = 4, muchTilesY = 4; 
    public float stepTiles = 2.8f, leftDownTileX = -11.0f, leftDownTileY = -4.0f;
    public float trashSpeed = 0.5f;
    public Slider Trspeed;
    public Slider slVolume;
    public GameObject AmbientMusic;
    public podmienkyScript skriptikPodmienok;
    public int kolkoOpakovaniJeDanych = -1;
    public TextMeshProUGUI cervenyTextik; //text co sa objavi ak nieco nebolo splnene - vystrazny
    public TextMeshProUGUI bielyTextik; // text ako nad nim, ale biely - doplnkovy, vysvetlovaci
    //private checkSpace checkingSpaceScript;
    //public GameObject checker;

    // Start is called before the first frame update
    void Start()
    {
        startPointX = startingPoint[0];
        trashSc = GameObject.Find("KosZhora+Checker").GetComponent<trashMove>();
        //checkingSpaceScript = checker.GetComponent<checkSpace>();
        cycleRunning = new List<int>();
        cycleRunning2 = new List<int>();
        instructs = new List<int>();
        indexOfInstructs = new List<int>();
        skriptikPodmienok = GameObject.Find("Podmienky").GetComponent<podmienkyScript>();
        plannedReserve = planned;
        MakeTiles();
        if (planned)
        {
         RepeatCycle();   
        }
        trashSpeed = persistenceScript.trashikSpeed;
        Trspeed.value = persistenceScript.trashikSpeed;
        slVolume.value = persistenceScript.volume;
    }

    private void MakeTiles()
    {
        for (int i = 0;  i < muchTilesX; i++)
        {
            for (int j = 0; j < muchTilesY; j++)
            {
                Instantiate(tile, new Vector3(leftDownTileX + i*stepTiles, leftDownTileY + j*stepTiles, 0), tile.transform.rotation);
            }
        }
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
        if (b_RepeatButt.GetComponent<Image>().sprite.name == "repeat_withoutSp")
        {
            Instantiate(repeatPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]+0.1f), repeatPf.transform.rotation);
        }
        else
        {
            Instantiate(repeatPfv, new Vector3(startPointX, startingPoint[1], startingPoint[2]+0.1f), repeatPfv.transform.rotation);
        }
        sizeOfCard = repeatPf.GetComponent<BoxCollider2D>();
        startPointX += sizeOfCard.size[0]/2.5f - distanceForNumber; //doladenie pre polozenie cisla do OPAKUJ karticky
        if (cycleRunning.Count != 0)
            {
                instructs.Add(7);
            } 
        canTrashMove();  
    }
    public void putStopCard()
    {   
        if (b_StopButt.GetComponent<Image>().sprite.name == "endCycle_smaller")
        {
            Instantiate(stopRepPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), stopRepPf.transform.rotation);
        }
        else
        {
             Instantiate(stopRepPfv, new Vector3(startPointX, startingPoint[1], startingPoint[2]), stopRepPfv.transform.rotation);
        }
        
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
            kolkoOpakovaniJeDanych = numbr;

        }
        canTrashMove();
    }

    public void RepeatCycle()
    {   if (planned == true)
        {
            planned = false;
            cycleRunning.Clear();
            cycleRunning2.Clear();
            instructs.Clear();
            cycleRunning.Add(55);
            cycleRunning2.Add(55);
            canMove = false;
            cycleRunning[cycleRunning.Count-1] = 1; //for 1 cycle
            cycleRunning2[cycleRunning2.Count-1] = 1;
        }
        else if (startPointX < endPointX && needNumberInCycle == false && startPointX < maxXforRepeat)
        {
            if (cycleRunning.Count == 0)
            {
                putRepeatCard();
                cycleRunning.Clear();
                cycleRunning2.Clear();
                instructs.Clear();
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
            if (b_RepeatButt.GetComponent<Image>().sprite.name == "repeat_withoutSp")
            {
                b_RepeatButt.GetComponent<Image>().sprite = repeatButtv;
                b_StopButt.GetComponent<Image>().sprite = stopButt;
            }
            else
            {
                b_RepeatButt.GetComponent<Image>().sprite = repeatButt;
                b_StopButt.GetComponent<Image>().sprite = stopButtv;
            }
        }
    }
    public void StopRepeat()
    {
        if (startPointX < endPointX && (cycleRunning.Count != 0) && needNumberInCycle == false)
        {
            if ((cycleRunning.Count == 1 && !plannedReserve) || cycleRunning.Count > 1)
            {
                putStopCard();
                instructs.Add(8);
                cycleRunning.RemoveAt(cycleRunning.Count-1);
                if (b_RepeatButt.GetComponent<Image>().sprite.name == "repeat_withoutSp")
                {
                    b_StopButt.GetComponent<Image>().sprite = stopButt;
                }
                else
                {
                    b_StopButt.GetComponent<Image>().sprite = stopButtv;
                }   
            }
        }
        if (cycleRunning.Count == 0 && cycleRunning2.Count != 0)
        {
            canMove = true;
            StartCoroutine(RepeatInstructions(0));
        }
    }

    private IEnumerator RepeatInstructions(int indexOfRepeat)
    {   
        TurnButtonsUnactive();
        for (int i = 0; i < cycleRunning2[indexOfRepeat]; i++)
        {
            if (indexOfRepeat != 0)
            {
                indexOfInstructs[indexOfRepeat] = indexOfInstructs[indexOfRepeat-1]+1;
            }
            else
            {
                indexOfInstructs.Clear();
                indexOfInstructs.Add(0);
            }
            while (instructs[indexOfInstructs[indexOfRepeat]] != 8)
            {   yield return new WaitForSeconds(trashSpeed);
                if(instructs[indexOfInstructs[indexOfRepeat]] == 10){trashSc.moveFd();}
                if(instructs[indexOfInstructs[indexOfRepeat]] == 11){trashSc.rightRotate();}
                if(instructs[indexOfInstructs[indexOfRepeat]] == 12){trashSc.leftRotate();}  
                if(instructs[indexOfInstructs[indexOfRepeat]] == 7)
                {   
                    indexOfInstructs.Add((indexOfInstructs[indexOfRepeat]+1));
                    yield return StartCoroutine(RepeatInstructions(indexOfRepeat+1));
                }
                else
                {
                    indexOfInstructs[indexOfRepeat]++;
                    yield return new WaitForSeconds(trashSpeed);
                }     
            }
        }

        if (indexOfRepeat != 0)
            {
                indexOfInstructs[indexOfRepeat-1] = indexOfInstructs[indexOfRepeat]+1;
            }
        if (indexOfRepeat == 0)
        {
            if (skriptikPodmienok.suPodmienky)
            {
                KontrolaPodmienok();
            }
            instructs.Clear();
            cycleRunning2.Clear();
            if (!plannedReserve){TurnButtonsActive();}
        }
        
    }

    private void TurnButtonsActive()
    {
        foreach (string butt in new string[] {"b_turnRight","b_turnLeft","b_forward","b_repeat","b_stopRepeat","b_one","b_two","b_three","b_four","b_five","b_six"})
        {
            GameObject.Find(butt).GetComponent<Button>().interactable = true;
        }
    }
    private void TurnButtonsUnactive()
    {
        foreach (string butt in new string[] {"b_turnRight","b_turnLeft","b_forward","b_repeat","b_stopRepeat","b_one","b_two","b_three","b_four","b_five","b_six"})
        {
            GameObject.Find(butt).GetComponent<Button>().interactable = false;
        }
    }

    public void playInstructs()
    {
        if (instructs.Count != 0)
        {
            instructs.Add(8);
            cycleRunning.RemoveAt(cycleRunning.Count-1);
            if (cycleRunning.Count == 0 && cycleRunning2.Count != 0)
            {
                canMove = true;
                TurnButtonsUnactive();
                StartCoroutine(RepeatInstructions(0));
           
            }
            else{
                cervenyTextik.text = "Neukončil si správne OPAKOVANIE!";
                bielyTextik.text = "Každý cyklus (opakovanie) musí niekde začínať, aj končiť, aby bolo jasné, ktoré príkazy patria do tela cyklu. V tele cyklu sú príkazy, ktoré sa majú vykonať viackrát. Príkazy pred cyklum, alebo za cyklom, sa vykonajú iba raz.";
                panel.SetActive(true);
            }
        }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeOfSpeed()
    {
        trashSpeed = Trspeed.value;
        persistenceScript.trashikSpeed = trashSpeed;
    }
    public void ChangeOfVolume(Slider tento)
    {
        persistenceScript.volume = tento.value;
        persistenceScriptVolume.thisAmbientMusic.GetComponent<AudioSource>().volume = tento.value;
    }

    public void KontrolaPodmienok()
    {
        if (skriptikPodmienok.naCyklus)
        {
            bool najdenyCyk = false;
            bool spravnyPocetOpak = false;
            for (int q = 0; q < instructs.Count; q++)
            {
                if(instructs[q] == 7){najdenyCyk = true;} 
                if(kolkoOpakovaniJeDanych == skriptikPodmienok.kolkoOpakovani){spravnyPocetOpak = true;}
            }
            if (!najdenyCyk || !spravnyPocetOpak)
            {
                cervenyTextik.text = "Nepoužil si cyklus s počtom opakovaním " + skriptikPodmienok.kolkoOpakovani;
                if (skriptikPodmienok.podmienkaNaMaxPrikazov)
                {
                    bielyTextik.text = "Pozor na podmienky. V tejto úlohe máš zadané, že musíš využiť cyklus, ktorý sa vykoná " + skriptikPodmienok.kolkoOpakovani +"-krát. Tiež môžeš využiť MAXIMÁLNE " +skriptikPodmienok.maxPrikazov + " príkazpv. Skús tak naprogramovať Šoka. Určite to zvládneš!";
                }
                else
                {
                    bielyTextik.text = "Pozor na podmienky. V tejto úlohe máš zadané, že musíš využiť cyklus, ktorý sa vykoná " + skriptikPodmienok.kolkoOpakovani +"-krát. Skús tak naprogramovať Šoka. Určite to zvládneš!";
                }
                
                panel.SetActive(true);
                return;
            }
        }
        if (skriptikPodmienok.podmienkaNaMaxPrikazov)
        {
            while (instructs.Contains(8))
            {
                instructs.Remove(8);
            }
            if (instructs.Count > skriptikPodmienok.maxPrikazov)
            {
                cervenyTextik.text = "Použil si veľa príkazov";
                bielyTextik.text = "Pozor na podmienky. V tejto úlohe môžeš využiť maximálne " + skriptikPodmienok.maxPrikazov +" príkazov. Ak nevieš, ako sa príkazy počítajú, prečítaj si inštrukcie (klik na tlačidlo). Určite to zvládneš!";
                panel.SetActive(true);
                return;
            }
        }
        if (skriptikPodmienok.specifickePodmienky) //ak su specificke podmienky, tak musi byt aj MAX PRIKAZOV!!!!
        {
            for (int prikazik = 0; prikazik < skriptikPodmienok.speciPrikazy.Count; prikazik++)
            {
                if (instructs[prikazik] != skriptikPodmienok.speciPrikazy[prikazik])
                {
                    cervenyTextik.text = "Nemáš to správne vyriešené :(";
                    bielyTextik.text = "Pozor na podmienky v úlohe. Je presne dané, akou postupnosťou sa má Šoko správať. Určite to zvládneš!";
                    panel.SetActive(true);
                    return;
                }
            }
        }


    }
}
