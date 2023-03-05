using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class podmienkyScript : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
