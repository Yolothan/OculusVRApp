using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMinimap : MonoBehaviour
{
    GameObject player;
    Transform spawnpoint;
    Vector3 positionSpawnPoint;
    public void SpawnPosition()
    {        
        spawnpoint = GameObject.Find("SpawnPointsMinimap/" + gameObject.name).transform;        
        player = GameObject.FindGameObjectWithTag("Player");
        positionSpawnPoint = new Vector3(spawnpoint.position.x, player.transform.position.y, spawnpoint.transform.position.z);
        player.transform.position = positionSpawnPoint;
    }
}
