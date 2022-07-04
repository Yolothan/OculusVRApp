using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterPool : MonoBehaviour
{       
    TurnTheGameOn.SimpleTrafficSystem.AITrafficCar trafficCars;


        private void OnTriggerEnter(Collider other)
        {
           if (other.gameObject.CompareTag("Player"))
          {
               trafficCars = other.GetComponent<TurnTheGameOn.SimpleTrafficSystem.AITrafficCar>();
               trafficCars.MoveCarToPool();
           }
     }
}
