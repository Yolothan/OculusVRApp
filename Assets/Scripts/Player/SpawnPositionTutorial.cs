using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionTutorial : MonoBehaviour
{
    // Set the player postion equal to the postion of this spawnposition object
    void Start()
    {
        SpawnPlayerBegin();
    }

    public void SpawnPlayerBegin()    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 positionSpawnPoint = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        Vector3 rotationSpawnPoint = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
        player.transform.position = positionSpawnPoint;
        player.transform.rotation = Quaternion.Euler(rotationSpawnPoint);

        GameObject assistantCanvas = GameObject.Find("AssistantCanvas");
        assistantCanvas.transform.parent = player.transform;

        UIScript uiScript = player.GetComponent<UIScript>();
        uiScript.demoUI.enabled = true;

    }
}
