using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeSpawnEndGame : MonoBehaviour
{
    TurnTheGameOn.SimpleTrafficSystem.AITrafficCar trafficCars;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            trafficCars = other.gameObject.GetComponent<TurnTheGameOn.SimpleTrafficSystem.AITrafficCar>();
            trafficCars.MoveCarToPool();
        }
    }
}
