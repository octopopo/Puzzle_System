using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PuzzleSystem.PuzzleManagers.V1;

[CustomEditor(typeof(PuzzleManager))]
public class PuzzleManagerEditor : Editor {

	public override void OnInspectorGUI()
    {
        PuzzleManager mPM = (PuzzleManager)target;
        if(GUILayout.Button("Put pieces in order"))
        {

        }
    }
}
