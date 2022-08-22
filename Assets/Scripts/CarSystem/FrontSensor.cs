using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontSensor : MonoBehaviour
{
    [SerializeField]
    private WaypointCar waypointCar;
    private WaypointsRoute route;    
    private WaypointCar otherWaypoint;
   
     
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BikePrefab"))
        {            
            route = other.GetComponent<WaypointsRoute>();
            waypointCar.movementSpeed = route.movementSpeed;
            if(waypointCar.movementSpeed < 0.1f)
            {
                waypointCar.stopDriving = true;
            }
            else
            {
                waypointCar.stopDriving = false;
            }
        }        
        else if (other.CompareTag("CarPrefab"))
        {            
            otherWaypoint = other.GetComponent<WaypointCar>();
            if (otherWaypoint.routeManager == waypointCar.routeManager)
            {
                waypointCar.movementSpeed = otherWaypoint.movementSpeed;
                if (waypointCar.movementSpeed < 0.1f)
                {
                    waypointCar.stopDriving = true;
                }
                else
                {
                    waypointCar.stopDriving = false;
                }
            }
        }
        else if (other.CompareTag("Player"))
        {
            waypointCar.stopDriving = true;
        }
    }
   
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BikePrefab"))
        {
            waypointCar.stopDriving = false;
        }
        else if (other.CompareTag("CarPrefab"))
        {
            waypointCar.stopDriving = false;
        }

        else if (other.CompareTag("Player"))
        {
            waypointCar.stopDriving = false;
        }
    }
}
