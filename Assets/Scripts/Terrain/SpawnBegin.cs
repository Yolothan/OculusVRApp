using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBegin : MonoBehaviour
{
    private SpawnPosition spawnPosition;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {        
        spawnPosition = FindObjectOfType<SpawnPosition>();
        spawnPosition.SpawnPlayerBegin();
    }
}
