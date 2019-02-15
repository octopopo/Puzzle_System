using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine;
using PuzzleSystem.PuzzleManagers.V1;

public class VideoBehavior : MonoBehaviour {
    public RawImage _rawImage;
    public VideoPlayer _videoPlayer;
    public VideoClip[] _videoClips;
    public Button _skipButton;
    public SpriteRenderer[] _piecesSprite;
    public PuzzleManager puzzleManager;
    public bool noVideo = true;


    // Use this for initialization
    void Start() {
        //Hide the video player at the beginning
        _rawImage.gameObject.SetActive(false);
        _skipButton.onClick.AddListener(HideAndStop);
    }

    // Update is called once per frame
    IEnumerator PlayVideo()
    {
        _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
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
        if(noVideo)
        {
            puzzleManager.GameProgessTracktor();
            return;
        }
        _rawImage.gameObject.SetActive(true);
        StartCoroutine(PlayVideo());
    }

    public void HideAndStop()
    {
        _rawImage.gameObject.SetActive(false);
        StopCoroutine(PlayVideo());
        _videoPlayer.Stop();
    }

    public void ChangeVideo(int videoNum)
    {

    }

    public void DisplayAndPlayOnPieces(int num)
    {
        StartCoroutine(PlayVideoOnPiece(num));
        
    }

    IEnumerator PlayVideoOnPiece(int num)
    {
        _videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        //_videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        _videoPlayer.Prepare();
        while (!_videoPlayer.isPrepared)
        {
            yield return null;
        }
        _videoPlayer.targetMaterialRenderer = _piecesSprite[num];
        //_piecesSprite[0].material.mainTexture = _videoPlayer.texture;
        _videoPlayer.Play();
    }
}
