using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSpotBehavior : MonoBehaviour {
    [SerializeField]private int _spotNum;
    public int SpotNum
    {
        get { return _spotNum; }
    }
}
