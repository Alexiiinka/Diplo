using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class menuScript : MonoBehaviour
{
    public GameObject panel;
    public GameObject AmbientMusic;
    public Vector2Int spravnaOdpoved;
    public TMP_InputField fieldX, fieldY,pocetik;
    public TMP_FontAsset fontik;
    public TMP_Text maly, velky;
    public bool xAy = false, zadajPocet = false, urciVzor = false; 
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ChangeOfVolume(Slider tento)
    {
        persistenceScript.volume = tento.value;
        persistenceScriptVolume.thisAmbientMusic.GetComponent<AudioSource>().volume = tento.value;
    }

    public void DajNovuScenu(string dalsiaScena)
    {
        SceneManager.LoadScene(dalsiaScena);
    }

    public void DalejSkontroluj()
    {
        if (xAy)
        {
            string x = spravnaOdpoved[0].ToString(), y = spravnaOdpoved[1].ToString();
            if (fieldX.text == x && fieldY.text == y)
            {
                if (SceneManager.GetActiveScene().name == "55")
                {
                }
                else{
                    int dalsiaScena = int.Parse(SceneManager.GetActiveScene().name)+1;
                    SceneManager.LoadScene(dalsiaScena.ToString());
                }
            }
            else
            {
                panel.SetActive(true);
            }
        }
        else if(zadajPocet)
        {
            string pocet = spravnaOdpoved[0].ToString();
            if (pocetik.text == pocet)
            {
                if (SceneManager.GetActiveScene().name == "55")
                {
                    Debug.Log("wuhu");
                }
                else{
                    int dalsiaScena = int.Parse(SceneManager.GetActiveScene().name)+1;
                    SceneManager.LoadScene(dalsiaScena.ToString());
                }
            }
            else{
                panel.SetActive(true);
            }
        }
        
    }

    public void Odpoved(bool hodnota)
    {
        if (hodnota)
        {
            velky.text = "GRATULUJEM";
            velky.font = fontik;
            maly.text = "Všetky odpadky sú pozbierané vďaka tebe! Šoko ti ďakuje, pretože teraz sa môže zúčastniť stretnutia košov. A na to sa veľmi tešil :)";
            panel.SetActive(true);
        }
        else
        {
            panel.SetActive(true);
        }
    }
}
