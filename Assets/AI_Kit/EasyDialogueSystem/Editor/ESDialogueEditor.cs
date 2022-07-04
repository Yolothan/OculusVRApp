using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(ESDialgoueScriptableObj))]
public class ESDialogueEditor : Editor
{
    public ESDialgoueScriptableObj scriptableObj;

    public override void OnInspectorGUI()
    {
        scriptableObj = target as ESDialgoueScriptableObj;

        if (GUILayout.Button("Launch Node Editor ;)", GUILayout.Width(Screen.width / 1.19f), GUILayout.Height(30)))
        {
            //launch widow
            ESDialogueWindow.Init();
        }
    }
}
