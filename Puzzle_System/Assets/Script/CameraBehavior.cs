using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    [SerializeField] Vector2[] _cameraPosition;
    //The size would affect the visual area
    [SerializeField] float[] _cameraSize;
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
        Debug.Log(gamePhase);
        Debug.Log(_cameraPosition[gamePhase]);
        Vector3 newPos = new Vector3(_cameraPosition[gamePhase].x, _cameraPosition[gamePhase].y, -10);
        Camera.main.transform.SetPositionAndRotation(newPos, Quaternion.identity);
        Camera.main.orthographicSize = _cameraSize[gamePhase];
    }
}
