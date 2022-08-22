using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ESNodeIdentity : MonoBehaviour
{
    // 
    public Transform NextNode;
    public List<Transform> ZebraCrossNodes = new List<Transform>();
    public Transform PreNode;
    public GameObject mesh;
    public float arrowsize;
    public bool CrossingPeds, Occupied, Stop;

    public int Ccount;

    private void Start()
    {
        Material NodeMat = Resources.Load("Node/Trans") as Material;
        this.GetComponent<MeshRenderer>().sharedMaterial = NodeMat;
    }


    //
    private void OnDrawGizmosSelected()
    {
        showArrow();
    }
    //
    private void showArrow()
    {
        if (mesh == null)
        {
            mesh = mesh == null ? Resources.Load("Node/Arrow") as GameObject : mesh;
        }
        if (NextNode != null)
        {
            this.transform.LookAt(NextNode);
            Gizmos.color = Color.red;
            Gizmos.DrawMesh(mesh.GetComponent<MeshFilter>().sharedMesh, -1, (this.transform.position + NextNode.position) * 0.5f, this.transform.rotation, new Vector3(1, 1, 1f) * arrowsize);

            // Vector4 d = new Vector4(1, 1, 1, 0.035f);
            Gizmos.color = Color.grey;
            Debug.DrawLine(this.transform.position, NextNode.position);
        }
    }
}
