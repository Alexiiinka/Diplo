using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persistenceScript : MonoBehaviour
{
    public static persistenceScript Instance;
    public static float trashikSpeed = 0.5f;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
