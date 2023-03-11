using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class podmienkyScript : MonoBehaviour
{
    public bool jeTriggerPodmienka = false; //ci sa ma Šoko niekam dostať
    public int kolkaX = 3, kolkaY = 0; //kolkata je trigger v X = v stlpci, zlava doprava,/ kolkata je v Y zhora dole
    // to jest x = 3, tak 4. stlpec, y = 0 tak 0ty riadok = spodny
    public bool suPodmienky = false; // ci su zadane vobec nejake podmienky v ulohe
    public bool naCyklus = false; // ci je v ulohe podmienka na pocet cyklov
    public int kolkoOpakovani = 1; // kolko OPAKOVI cyklu musi byt ak je splnena podmienka naCyklus
    public bool podmienkaNaMaxPrikazov = false; 
    public int maxPrikazov = 5; // kolko moze byt maximalne prikazov
    /* pricom>>> CYKLUS sa rata ako prikaz vzdy!! To jest ak je podmienka na vyuzitie 2 opakovani
    a zaroven maxPrikazov == 5, tak budu prikazy: 1 cyklus, 4 ine
    pretoze pocet opakovani v cykle sa nerata ako prikaz, ale len samostatny cyklus je prikaz! */
    public bool specifickePodmienky = false;
    //-- zapis instrukcii -- 1-6> pocet opakovani, 7-zac. opak, 8-koniec opak., 10-fd, 11-right, 12-left
    public List<int> speciPrikazy;
    public string speciPrikazyText;
    [Header("Vyznacena cesticka")]
    public bool jeCesticka = false;
    public List<Vector2Int> cesticka;
    [Header("Flase")]
    public bool jeOdpad = false;
    public List<Vector2Int> odpadky;
    [Header("Textik pre potechu duse")]
    public string textPreDusu;
     
}
