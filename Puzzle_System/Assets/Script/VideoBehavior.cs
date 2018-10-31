using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine;

public class VideoBehavior : MonoBehaviour {
    public RawImage _rawImage;
    public VideoPlayer _videoPlayer;


    // Use this for initialization
    void Start() {
        StartCoroutine(PlayVideo());

    }

    // Update is called once per frame
    IEnumerator PlayVideo()
    {
        _videoPlayer.Prepare();
        while(!_videoPlayer.isPrepared)
        {
            yield return null;
        }
        _rawImage.texture = _videoPlayer.texture;
        _videoPlayer.Play();
    }
}
