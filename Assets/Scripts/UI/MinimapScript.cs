using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MinimapScript : MonoBehaviour
{    
    public GameObject player, spawnMesh;    
    
    // Update is called once per frame
    public void TeleportPlayerMinimap()
    {
        player.transform.position = spawnMesh.transform.position;   
    }    
}
