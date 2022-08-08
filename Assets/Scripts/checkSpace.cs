using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkSpace : MonoBehaviour
{
    public bool freeSpace = true;
    
    void OnTriggerEnter(Collider other)
    {
        freeSpace = false;
    }
    
}
