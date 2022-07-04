using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class ESMenu_ : MonoBehaviour
{

    [MenuItem("GameObject/AI KIT/HumanNode", false, 10)]
    static void CreatePathParentGameObject(MenuCommand menuCommand)
    {
        //creates a parent
        GameObject goparent = new GameObject("HumanNodeParent");
        // Create a custom game object
        GameObject go = new GameObject("NodePath");
        //go.AddComponent<ESNodeSystem>();


        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        GameObjectUtility.SetParentAndAlign(goparent, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(goparent, "Create " + goparent.name);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
        go.AddComponent<ESNodePlace>();
        go.transform.parent = goparent.transform;

    }
}
#endif
