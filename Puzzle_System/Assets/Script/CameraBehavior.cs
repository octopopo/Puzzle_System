using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    [SerializeField] Vector2[] CameraPosition;
    //The size would affect the visual area
    [SerializeField] float[] CameraSize;
    int gamePhase = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeCameraPhase(int phase)
    {
        gamePhase = phase;
    }
}
