using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class sceneScript : MonoBehaviour
{
    public GameObject trashik;
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
    public GameObject tileTrigerko;
    public GameObject tileCesticka;
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
    public TextMeshProUGUI levelCisloTextik; // text ako nad nim, ale biely - doplnkovy, vysvetlovaci
    public TMP_FontAsset ziarivyZelenyText;
    public bool jeVCieliZelenom = false;
    public TextMeshProUGUI podmienkovyText;
    public List<int> listPriamychInstrukcii = new List<int>(); // na kontrolu priameho rezimu - overenie - pre podmienky dane
    //private checkSpace checkingSpaceScript;
    //public GameObject checker;
    [Header("Prefaby odpadkov")]
    public List<GameObject> odpadPf;
    [Header("Je VIDEO")]
    public bool jeVideo = false;
    public GameObject panelInfoVideo;



    // Start is called before the first frame update
    void Start()
    {
        if (jeVideo){panelInfoVideo.SetActive(true);}
        startPointX = startingPoint[0];
        //trashSc = GameObject.Find("KosZhora+Checker").GetComponent<trashMove>();
        trashSc = trashik.GetComponent<trashMove>();
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
        if (skriptikPodmienok.suPodmienky || skriptikPodmienok.jeTriggerPodmienka){ZmenaTextuPodmienok();}
        else{podmienkovyText.text = skriptikPodmienok.textPreDusu;}
        trashSpeed = persistenceScript.trashikSpeed;
        Trspeed.value = persistenceScript.trashikSpeed;
        slVolume.value = persistenceScript.volume;
        levelCisloTextik.text = "Level: " + SceneManager.GetActiveScene().name;
    }

    private void MakeTiles()
    {
        for (int i = 0;  i < muchTilesX; i++)
        {
            for (int j = 0; j < muchTilesY; j++)
            {
                if (skriptikPodmienok.jeTriggerPodmienka && skriptikPodmienok.kolkaX == i && skriptikPodmienok.kolkaY == j)
                {
                    Instantiate(tileTrigerko, new Vector3(leftDownTileX + i*stepTiles, leftDownTileY + j*stepTiles, 0), tile.transform.rotation);
                }
                else
                {
                    if (skriptikPodmienok.jeCesticka)
                    {
                        if (skriptikPodmienok.cesticka.Contains(new Vector2Int(i,j)))
                        {
                            Instantiate(tileCesticka, new Vector3(leftDownTileX + i*stepTiles, leftDownTileY + j*stepTiles, 0), tile.transform.rotation);
                        }
                        else
                        {
                            Instantiate(tile, new Vector3(leftDownTileX + i*stepTiles, leftDownTileY + j*stepTiles, 0), tile.transform.rotation);
                        }  
                    }
                    else
                    {
                        Instantiate(tile, new Vector3(leftDownTileX + i*stepTiles, leftDownTileY + j*stepTiles, 0), tile.transform.rotation);
                    } 
                }
                if (skriptikPodmienok.jeOdpad && skriptikPodmienok.odpadky.Contains(new Vector2Int(i,j)))
                {
                    Instantiate(odpadPf[Random.Range(0,odpadPf.Count)], new Vector3(leftDownTileX + i*stepTiles, leftDownTileY + j*stepTiles + 0.3f, -1),odpadPf[0].transform.rotation);
                }
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
    {   canTrashMove();
        if (startPointX < endPointX && needNumberInCycle == false)
        {
            Instantiate(fdPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), fdPf.transform.rotation);
            sizeOfCard = fdPf.GetComponent<BoxCollider2D>();
            if (cycleRunning.Count != 0)
            {
                instructs.Add(10);
            }
            listPriamychInstrukcii.Add(10);
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
        }
        
    }
    public void putLeftCard()
    {
        canTrashMove();
        if (startPointX < endPointX && needNumberInCycle == false)
        {
            Instantiate(ltPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), ltPf.transform.rotation);
            sizeOfCard = ltPf.GetComponent<BoxCollider2D>();
            if (cycleRunning.Count != 0)
            {
                instructs.Add(12);
            } 
            listPriamychInstrukcii.Add(12);
            
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
        }
        
        
    }
    public void putRightCard()
    {
        canTrashMove();
        if (startPointX < endPointX && needNumberInCycle == false)
        {
            Instantiate(rtPf, new Vector3(startPointX, startingPoint[1], startingPoint[2]), rtPf.transform.rotation);
            sizeOfCard = rtPf.GetComponent<BoxCollider2D>();
            if (cycleRunning.Count != 0)
            {
                instructs.Add(11);
            } 
            listPriamychInstrukcii.Add(11);
            
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
        }
        
        
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
        listPriamychInstrukcii.Add(7);
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
        listPriamychInstrukcii.Add(8);
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
            needNumberInCycle = false;
            cycleRunning[cycleRunning.Count-1] = numbr;
            cycleRunning2[cycleRunning2.Count-1] = numbr;
            kolkoOpakovaniJeDanych = numbr;
            listPriamychInstrukcii.Add(numbr);
            canTrashMove();
            startPointX += sizeOfCard.size[0]/2.5f + distanceBetween;
        }
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
            if (plannedReserve)
            {   bool splenePodminecky = true;
                if (skriptikPodmienok.suPodmienky){splenePodminecky = KontrolaPodmienok(instructs);}
                if (skriptikPodmienok.jeTriggerPodmienka && !jeVCieliZelenom)
                {
                    if (!splenePodminecky)
                    {
                        cervenyTextik.text = "Nedostal si sa do cieľa";
                        bielyTextik.text = bielyTextik.text + "Skús naprogramovať Šoka tak, aby sa dostal na ZELENÉ políčko.";
                    }
                    else
                    {
                        cervenyTextik.text = "Nedostal si sa do cieľa";
                        bielyTextik.text = "Skús naprogramovať Šoka tak, aby sa dostal na ZELENÉ políčko.";
                    }   
                }
                else
                {
                    if (splenePodminecky)
                    {
                        cervenyTextik.font = ziarivyZelenyText;
                        cervenyTextik.text = "SUPEEEER";
                        bielyTextik.text = "Hor sa na ďalší level!";
                        panel.transform.Find("b_NextLevel").gameObject.SetActive(true);
                    }       
                }
                panel.SetActive(true);
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

    public bool KontrolaPodmienok(List<int> listocek)
    {
        string sklonovanie = "ahoj";
        if (skriptikPodmienok.podmienkaNaMaxPrikazov)
        {   if (skriptikPodmienok.maxPrikazov < 5){sklonovanie = " príkazy. ";}
            else{sklonovanie = " príkazov. ";}
        }
        if (skriptikPodmienok.naCyklus)
        {
            bool najdenyCyk = false;
            bool spravnyPocetOpak = false;
            for (int q = 0; q < listocek.Count; q++)
            {
                if(listocek[q] == 7){najdenyCyk = true;} 
                if(kolkoOpakovaniJeDanych == skriptikPodmienok.kolkoOpakovani){spravnyPocetOpak = true;}
            }
            if (!najdenyCyk || !spravnyPocetOpak)
            {
                cervenyTextik.text = "Nepoužil si cyklus s počtom opakovaním " + skriptikPodmienok.kolkoOpakovani;
                if (skriptikPodmienok.podmienkaNaMaxPrikazov)
                {
                    bielyTextik.text = "Pozor na podmienky. V tejto úlohe máš zadané, že musíš využiť cyklus, ktorý sa vykoná " + skriptikPodmienok.kolkoOpakovani +"-krát. Tiež môžeš využiť MAXIMÁLNE " +skriptikPodmienok.maxPrikazov + sklonovanie + "Skús tak naprogramovať Šoka. Určite to zvládneš! ";
                }
                else
                {
                    bielyTextik.text = "Pozor na podmienky. V tejto úlohe máš zadané, že musíš využiť cyklus, ktorý sa vykoná " + skriptikPodmienok.kolkoOpakovani +"-krát. Skús tak naprogramovať Šoka. Určite to zvládneš! ";
                }
                return false;
            }
        }
        List<int> newInstructs = new List<int>(listocek); //tu mam ulozeny povodny list aj s ukoncovanim cyklov (8)
        if (skriptikPodmienok.podmienkaNaMaxPrikazov)
        {
            while (listocek.Contains(8)) //dam si prec vsetky ukoncenia (8) aby sa to nepocitalo ako prikaz
            {
                listocek.Remove(8);
                for (int i = 1; i < 7; i++)
                {
                    newInstructs.Remove(i);
                    listocek.Remove(i);//trochu blby sposob ako zrusit pocet opakovani z priameho programovania
                }//kedze v nepriamom sa nepocita do "instrukcii" ale do "cykly running"
            }
            if (listocek.Count > skriptikPodmienok.maxPrikazov)
            {
                print("pocet listocek>" + listocek.Count);
                print("pocet max> " + skriptikPodmienok.maxPrikazov);
                cervenyTextik.text = "Použil si veľa príkazov";
                bielyTextik.text = "Pozor na podmienky. V tejto úlohe môžeš využiť maximálne " + skriptikPodmienok.maxPrikazov + sklonovanie + "Ak nevieš, ako sa príkazy počítajú, prečítaj si inštrukcie (klik na tlačidlo). Určite to zvládneš! ";
                return false;
            }
        }
        if (skriptikPodmienok.specifickePodmienky) //ak su specificke podmienky, tak musi byt aj MAX PRIKAZOV!!!!
        {
            if(plannedReserve){newInstructs.RemoveAt(newInstructs.Count-1);} //ak je planovany, tak nech sa posledna 8micka vymaze
            if (newInstructs.Count != skriptikPodmienok.speciPrikazy.Count)
            {   
                cervenyTextik.text = "Nemáš to správne vyriešené :(";
                bielyTextik.text = "Pozor na podmienky v úlohe. Je presne dané, akou postupnosťou sa má Šoko správať. Určite to zvládneš! ";
                return false;
            }
            for (int prikazik = 0; prikazik < skriptikPodmienok.speciPrikazy.Count-1; prikazik++)
            {
                if (newInstructs[prikazik] != skriptikPodmienok.speciPrikazy[prikazik])
                {
                    cervenyTextik.text = "Nemáš to správne vyriešené :(";
                    bielyTextik.text = "Pozor na podmienky v úlohe. Je presne dané, akou postupnosťou sa má Šoko správať. Určite to zvládneš! ";
                    return false;
                }
            }
        }
        cervenyTextik.font = ziarivyZelenyText;
        cervenyTextik.text = "SUPEEEER";
        bielyTextik.text = "Hor sa na ďalší level!";
        return true;
    }

    private void ZmenaTextuPodmienok()
    {   string sklonovanie = "ahoj";
        if (skriptikPodmienok.podmienkaNaMaxPrikazov)
        {   if (skriptikPodmienok.maxPrikazov < 5){sklonovanie = " príkazy. ";}
            else{sklonovanie = " príkazov. ";}
        }
        podmienkovyText.text = "PODMIENKY: ";
        if (skriptikPodmienok.jeTriggerPodmienka){podmienkovyText.text += "Dostaň sa na zelený cieľ. ";}
        if (skriptikPodmienok.suPodmienky)
        {
            if (skriptikPodmienok.naCyklus){podmienkovyText.text+= "Použi cyklus s počtom opakovaní: " + skriptikPodmienok.kolkoOpakovani + ". ";}
            if (skriptikPodmienok.podmienkaNaMaxPrikazov){podmienkovyText.text += "Môžeš využiť max. " + skriptikPodmienok.maxPrikazov + sklonovanie;}
            if (skriptikPodmienok.specifickePodmienky){
                podmienkovyText.text = skriptikPodmienok.speciPrikazyText + "Môžeš využiť max. " + skriptikPodmienok.maxPrikazov + sklonovanie;}
        }
    }
    public void OvereniePriamy()
    {
        bool splnenysen = true;
        if (skriptikPodmienok.suPodmienky){splnenysen = KontrolaPodmienok(listPriamychInstrukcii);}
        if (skriptikPodmienok.jeTriggerPodmienka && !jeVCieliZelenom)
        {
            if (!splnenysen)
            {
                cervenyTextik.text = "Nedostal si sa do cieľa";
                bielyTextik.text = bielyTextik.text + "Skús naprogramovať Šoka tak, aby sa dostal na ZELENÉ políčko. ";
            }
            else
            {
                cervenyTextik.text = "Nedostal si sa do cieľa";
                bielyTextik.text = "Skús naprogramovať Šoka tak, aby sa dostal na ZELENÉ políčko. ";
            }   
        }
        else
        {
            if (splnenysen)
            {
                cervenyTextik.font = ziarivyZelenyText;
                cervenyTextik.text = "SUPEEEER";
                bielyTextik.text = "Hor sa na ďalší level!";
                panel.transform.Find("b_NextLevel").gameObject.SetActive(true);
            }       
        }
        panel.SetActive(true);
    }
    public void ZmenScenu(string dalsia)
    {
        SceneManager.LoadScene(dalsia);
    }
    
}
