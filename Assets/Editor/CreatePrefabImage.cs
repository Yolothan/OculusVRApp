using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

public class CreatePrefabImage : MonoBehaviour
{
    [SerializeField]
    private GameObject spawnObjectPrefab;    
    private RawImage spawnImagePrefab;    
    private Texture2D texture;
    private void Start()
    {
        spawnImagePrefab = GetComponent<RawImage>();
        spawnImagePrefab.texture = AssetPreview.GetAssetPreview(spawnObjectPrefab);
        texture = (Texture2D)spawnImagePrefab.texture;
        byte[] bytes = texture.EncodeToPNG();

        var dirPath = Application.dataPath + "/Imports/Icons/Fotos/" + spawnObjectPrefab.name;
        
        File.WriteAllBytes(dirPath + ".png", bytes);
        Debug.Log(bytes.Length / 1024 + "Kb was saved as: " + dirPath);

    }
}
