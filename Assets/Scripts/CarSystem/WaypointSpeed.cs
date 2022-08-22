using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSpeed : MonoBehaviour
{
    [HideInInspector]
    public float waypointSpeed = 4.0f;
    [SerializeField]
    private float speedInKmPerUur = 15.0f;
    

    private void Start()
    {
        waypointSpeed = speedInKmPerUur / 3.6f;
        GetComponent<MeshRenderer>().enabled = false;
    }
}
