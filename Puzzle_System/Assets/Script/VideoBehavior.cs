using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine;

public class VideoBehavior : MonoBehaviour {
    public RawImage _rawImage;
    public VideoPlayer _videoPlayer;
    public VideoClip[] _videoClips;


    // Use this for initialization
    void Start() {
        //Hide the video player at the beginning
        _rawImage.gameObject.SetActive(false);
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

    public void DisplayAndPlay()
    {
        _rawImage.gameObject.SetActive(true);
        StartCoroutine(PlayVideo());
    }

    public void HideAndStop()
    {
        _rawImage.gameObject.SetActive(false);
        _videoPlayer.Stop();
    }

    public void ChangeVideo(int videoNum)
    {

    }
}
