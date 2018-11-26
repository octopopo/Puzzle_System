using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour {

    [SerializeField] Vector2[] _cameraPosition;
    //The size would affect the visual area
    [SerializeField] float[] _cameraSize;
    [SerializeField] private Camera _cameraComponent;
    int gamePhase = 0;

	// Use this for initialization
	void Start () {
        if (_cameraComponent == null)
        {
            _cameraComponent = GetComponent<Camera>();
        }
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
        transform.SetPositionAndRotation(newPos, Quaternion.identity);
        _cameraComponent.orthographicSize = _cameraSize[gamePhase];
    }
}
