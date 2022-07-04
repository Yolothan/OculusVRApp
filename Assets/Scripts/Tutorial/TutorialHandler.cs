using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Mathematics;

public class TutorialHandler : MonoBehaviour
{
    private GameObject player;
    private GameObject[] aiTrafficCars;
    private DeviceBasedSnapTurnProvider snapTurnProvider;
    private int i;
    private Vector3 previousRot, nextRot;
    [HideInInspector]
    public bool invokeOnce = true;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        snapTurnProvider = player.GetComponent<DeviceBasedSnapTurnProvider>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
