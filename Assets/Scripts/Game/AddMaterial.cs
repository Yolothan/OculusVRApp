using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMaterial : MonoBehaviour
{
    private Renderer meshRenderer;
    [SerializeField]
    private Material material;
    // Start is called before the first frame update
    

    // Update is called once per frame
    public void Material()
    {
        meshRenderer = GetComponent<Renderer>();
        meshRenderer.material = material;
    }
}
