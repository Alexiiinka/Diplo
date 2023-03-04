using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistenceScriptVolume : MonoBehaviour
{
    public static persistenceScriptVolume InstanceVolume;
    public static GameObject thisAmbientMusic;
    public static bool ano = true;

    private void Awake()
    {
        if (InstanceVolume != null)
        {
            Destroy(gameObject);
            return;
        }
        InstanceVolume = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        thisAmbientMusic = gameObject;
    }
}