using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnTheGameOn.SimpleTrafficSystem;

public class YieldTriggerForCars : MonoBehaviour
{
    [SerializeField]
    private YieldTriggerForDetectingBikes bikesDetection;
    private WaypointCar carTraffic;

    



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CarPrefab"))
        {
            carTraffic = other.GetComponent<WaypointCar>();
            if (bikesDetection.bikerInBox)
            {                
                carTraffic.stopDriving = true;
            }
            if (!bikesDetection.bikerInBox)
            {                
                carTraffic.stopDriving = false;
            }
            if (!bikesDetection.carInBox)
            {                
                carTraffic.stopDriving = false;
            }
            if (bikesDetection.carInBox)
            {                
                carTraffic.stopDriving = true;
            }
        }
    }  
}
