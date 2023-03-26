using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Networking;

public class LoadVideo : MonoBehaviour
{
    // Start is called before the first frame update
    public string videoName;
    void Start()
    {   
        gameObject.GetComponent<VideoPlayer>().url = Application.streamingAssetsPath + "/" + videoName + ".mp4";
        // gameObject.GetComponent<VideoPlayer>().url = UnityWebRequestMultimedia.GetMovieTexture + "/" + videoName + ".mp4";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
