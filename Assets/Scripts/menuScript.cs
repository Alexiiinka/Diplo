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
}
