using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RpgEssentials;
[CustomEditor(typeof(BarManager))]
public class GUIBarManager : Editor
{
    
    public override void OnInspectorGUI()
    {
        BarManager bar = (BarManager)target;
        DrawDefaultInspector();
        if(GUILayout.Button("Consume"))
        {
            bar.ConsumeBar();
        }
        if(GUILayout.Button("Fill up"))
        {
            bar.FillUp();
        }
    }
}
