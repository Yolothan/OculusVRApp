using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddToEnviro : MonoBehaviour
{
    EnviroSky enviroSky;    
    GameObject player;
    Camera mainCamera;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = Camera.main;
        enviroSky = gameObject.GetComponent<EnviroSky>();
        if (enviroSky.Player == null)
        {
            enviroSky.Player = player;
            enviroSky.PlayerCamera = mainCamera;
        }
    }
}
