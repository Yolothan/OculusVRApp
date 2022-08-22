using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yield : MonoBehaviour
{
    public Transform[] yieldWayPoint;
    private WaypointCar route, otherWaypointCar;
    [SerializeField]
    private WaypointRouteManager[] managers;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CarPrefab"))
        {
            otherWaypointCar = other.GetComponent<WaypointCar>();           
            for (int i = 0; i < yieldWayPoint.Length; i++)
            {
                if (yieldWayPoint[i].GetComponentInChildren<WaypointCar>() != null)
                {
                    route = yieldWayPoint[i].GetComponentInChildren<WaypointCar>();
                    for (int j = 0; j < managers.Length; j++)
                    {
                        if (otherWaypointCar.firstManager == managers[j])
                        {
                            route.stopDriving = true;
                        }

                    }                            
                }
            }
        }
        else if (other.CompareTag("BikePrefab"))
        {
            for (int i = 0; i < yieldWayPoint.Length; i++)
            {
                if (yieldWayPoint[i].GetComponentInChildren<WaypointCar>() != null)
                {
                    route = yieldWayPoint[i].GetComponentInChildren<WaypointCar>();

                    route.stopDriving = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CarPrefab"))
        {
            otherWaypointCar = other.GetComponent<WaypointCar>();
            for (int i = 0; i < yieldWayPoint.Length; i++)
            {
                if (yieldWayPoint[i].GetComponentInChildren<WaypointCar>() != null)
                {
                    route = yieldWayPoint[i].GetComponentInChildren<WaypointCar>();
                    for (int j = 0; j < managers.Length; j++)
                    {
                        if (otherWaypointCar.firstManager == managers[j])
                        {
                            route.stopDriving = false;
                        }

                    }
                }
            }
        }
        else if (other.CompareTag("BikePrefab"))
        {
            for (int i = 0; i < yieldWayPoint.Length; i++)
            {
                if (yieldWayPoint[i].GetComponentInChildren<WaypointCar>() != null)
                {
                    route = yieldWayPoint[i].GetComponentInChildren<WaypointCar>();

                    route.stopDriving = false;
                }
            }
        }

    }
}
