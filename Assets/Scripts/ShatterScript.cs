using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterScript : MonoBehaviour
{
    public GameObject activate;
    int i = 0;
    public void DivideMesh ()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Mesh mesh = meshFilter.mesh;
        Vector3[] verts = mesh.vertices;
        Vector3[] normals = mesh.normals;
        Vector2[] uvs = mesh.uv;

        for(int submesh = 0; submesh < mesh.subMeshCount; submesh ++)
        {
            int[] indices = mesh.GetTriangles(submesh);
            for(int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index] * this.transform.localScale.x;
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }
                Mesh dmesh = new Mesh();
                dmesh.vertices = newVerts;
                dmesh.normals = newNormals;
                dmesh.uv = newUvs;

                dmesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                GameObject tri = new GameObject("Tri" + (i / 3));
                tri.transform.position = transform.position;
                tri.transform.rotation = transform.rotation;
                tri.AddComponent<MeshRenderer>().material = meshRenderer.materials[submesh];
                tri.AddComponent<MeshFilter>().mesh = dmesh;
                tri.AddComponent<BoxCollider>();
                tri.AddComponent<Rigidbody>().AddExplosionForce(100, transform.position, 100);
                tri.GetComponent<Rigidbody>().AddTorque(transform.up * Random.Range(-10, 10));
                tri.GetComponent<Rigidbody>().useGravity = false;
                tri.layer = 12;
                if (indices.Length > 300)
                {
                    Debug.Log(indices.Length);
                }
            }
              
        }
        
        Destroy(gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        DivideMesh();
    }
    private void FixedUpdate()
    {
        i = i + 1;
        
        if(i == 139)
        {
            activate.SetActive(true);
        }
    }
    
}
