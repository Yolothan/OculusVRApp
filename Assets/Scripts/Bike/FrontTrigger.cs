using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTrigger : MonoBehaviour
{
    [SerializeField]
    private WaypointsRoute route;    
    private WaypointCar waypointCar;        
    private WaypointsRoute otherWaypoint;       

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BikePrefab"))
        {            
            otherWaypoint = other.GetComponent<WaypointsRoute>();
            route.movementSpeed = otherWaypoint.movementSpeed;
            if (route.movementSpeed < 0.1f)
            {
                route.stopCycling = true;
            }
            else
            {
                route.stopCycling = false;
            }
        }       
        else if (other.CompareTag("CarPrefab"))
        {            
            waypointCar = other.GetComponent<WaypointCar>();
            route.movementSpeed = waypointCar.movementSpeed;
            if (route.movementSpeed < 0.1f)
            {
               route.stopCycling = true;
            }
            else
            {
                route.stopCycling = false;
            }
        }
        else if(other.CompareTag("Player"))
        {
            route.stopCycling = true;
        }
    }
}
