
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ESNodePlace))]
public class ESNodePlaceEditor : Editor
{
    public ESNodePlace place;
    public override void OnInspectorGUI()
    {
        place = target as ESNodePlace;
        EditorGUI.BeginChangeCheck();


        if (EditorGUI.EndChangeCheck())
        {
            Undo.RegisterCompleteObjectUndo(place, "fuck");
        }
        //
        GameObject RowPar = null;
        if (place.AutoItrate)
        {
            GUILayout.Label("AutoIterateIsOn", EditorStyles.boldLabel);
            if (GUILayout.Button("Iterate", GUILayout.MinWidth(150), GUILayout.MinHeight(25)))
            {
                if (place.RowParent != null)
                {
                    DestroyImmediate(place.RowParent);
                }
            }
        }
        else
        {
            GUILayout.Label("AutoIterateIsoff", EditorStyles.boldLabel);
            string RowName = "RowParent";
            EditorGUILayout.HelpBox("Create GameObject and child to this parent make the name corresponds with 'RowParent' U can Copy Paste from RowName", MessageType.Info);
            EditorGUILayout.TextField("RowParent", RowName);

            RowPar = (GameObject)EditorGUILayout.ObjectField("RowParent", place.RowParent, typeof(GameObject), true) as GameObject;
        }
        //
        if (GUI.changed)
            EditorUtility.SetDirty(place);
        base.OnInspectorGUI();
    }
    //
    public void OnSceneGUI()
    {
        place = target as ESNodePlace;
        Event e = Event.current;
        place.Node = place.Node == null ? Resources.Load("Node/Node") as GameObject : place.Node;
        //
        if (Event.current.type == EventType.KeyDown)
        {
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(place, "this biiiish");
            }
            if (e.keyCode == KeyCode.A && !place.done)
            {
                CallNodes(place);
                place.done = true;
            }
        }
        //
        if (Event.current.type == EventType.KeyUp)
        {
            place.done = false;
        }
        //
        RefreshNodeList();
        //

        //
        if (GUI.changed)
        {
            EditorUtility.SetDirty(place);
        }
    }
    //
    private void RefreshNodeList()
    {
        place.Nodelist = new List<Transform>(0);
        //
        Transform[] mainlist = place.GetComponentsInChildren<Transform>();

        if (mainlist.Length > 0)
        {
            for (int i = 0; i < mainlist.Length; ++i)
            {
                if (mainlist[i].transform != place.transform)
                {
                    place.Nodelist.Add(mainlist[i]);
                }
                //
            }
            //
            if (place.Nodelist.Count > 0)
                for (int i = 0; i < place.Nodelist.Count; ++i)
                {
                    if (place.Nodelist.Count > 1)
                    {
                        place.Nodelist[i].GetComponent<ESNodeIdentity>().arrowsize = place.ArrowSize;
                        //
                        if (i != (place.Nodelist.Count - 1))
                            place.Nodelist[i].transform.
                                                     GetComponent<ESNodeIdentity>().NextNode = place.Nodelist[i + 1].transform;
                        //
                        if (i != 0)
                        {
                            place.Nodelist[i].transform.
                                                    GetComponent<ESNodeIdentity>().PreNode = place.Nodelist[i - 1].transform;
                        }
                    }
                }
        }
    }
    //
    private void CallNodes(ESNodePlace es)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
        {
            //
            if (place.Node != null)
            {
                if (place.Nodelist == null)
                {
                    place.Nodelist = new List<Transform>();
                }
                GameObject newnode = Instantiate(place.Node.gameObject, hit.point, Quaternion.identity);
                newnode.transform.parent = place.transform;
                newnode.AddComponent<ESNodeIdentity>();
                RefreshNodeList();
                /*
                if (place.Nodelist.Count > 1)
                {
                    //Debug.Log("HHH");
                    place.Nodelist[place.Nodelist.Count - 2].transform.
                    GetComponent<ESNodeIdentity>().NextNode = newnode.transform;
                    //
                    newnode.GetComponent<ESNodeIdentity>().PreNode = place.Nodelist[place.Nodelist.Count - 2].transform;
                }
                */
            }
            else
            {
                place.Node = place.Node == null ? Resources.Load("Node/Node") as GameObject : place.Node;
            }
            // Transform t = Instantiate(es.nodeprefab.transform, hit.point, Quaternion.identity);
            //creates a parent
            //GameObject go = new GameObject("Node");
            // Create a custom game object
            //go.AddComponent<MeshFilter>();
            //go.AddComponent<MeshRenderer>();
            //go.AddComponent<ESNodePlaceManager>();
            /*
            Transform[] nodes = place.GetComponentsInChildren<Transform>();
            List<Transform> nodelist = new List<Transform>();
            for (int i = 0; i < nodes.Length; ++i)
            {
                if (nodes[i] != place.transform)
                {
                    nodelist.Add(nodes[i]);
                }
            }
            //
            /*
            if (nodelist.Count > 0)
            {
                scripts.LastcreatedNode = nodelist[nodelist.Count - 1];
            }

            if (scripts.LastcreatedNode != null)
            {
                scripts.nodelist[scripts.nodelist.Count - 1].GetComponent<ESNodeManager>().NextNode = go.transform;
            }
            go.GetComponent<MeshFilter>().sharedMesh = es.nodeprefab.GetComponent<MeshFilter>().sharedMesh;
            go.GetComponent<MeshRenderer>().sharedMaterial = es.nodeprefab.GetComponent<MeshRenderer>().sharedMaterial;
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            go.transform.position = hit.point;
            switch (scripts.GetAlign)
            {
                case ESNodeSystem.AlignAxis.X:
                    Vector3 v = go.transform.position;

                    v.x = scripts.lastnodepos.x;
                    go.transform.position = v;
                    break;
                case ESNodeSystem.AlignAxis.Z:
                    Vector3 v1 = go.transform.position;

                    v1.z = scripts.lastnodepos.z;
                    go.transform.position = v1;
                    break;

            }
            go.transform.parent = scripts.transform;
            */
        }
    }
}

