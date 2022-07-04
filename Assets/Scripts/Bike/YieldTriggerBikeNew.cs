using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YieldTriggerBikeNew : MonoBehaviour
{
    public Transform[] yieldWayPoint;
    private WaypointsRoute route, otherWaypointBike;
    [SerializeField]
    private WaypointRouteManagerBikes[] firstManagers;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BikePrefab"))
        {
            otherWaypointBike = other.GetComponent<WaypointsRoute>();
            for (int i = 0; i < yieldWayPoint.Length; i++)
            {
                if (yieldWayPoint[i].GetComponentInChildren<WaypointsRoute>() != null)
                {
                    route = yieldWayPoint[i].GetComponentInChildren<WaypointsRoute>();
                    for (int j = 0; j < firstManagers.Length; j++)
                    {
                        if (otherWaypointBike.firstManager == firstManagers[j])
                        {
                            route.stopCycling = true;
                        }

                    }
                }
            }
        }
        else if (other.CompareTag("CarPrefab"))
        {
            for (int i = 0; i < yieldWayPoint.Length; i++)
            {
                if (yieldWayPoint[i].GetComponentInChildren<WaypointsRoute>() != null)
                {
                    route = yieldWayPoint[i].GetComponentInChildren<WaypointsRoute>();

                    route.stopCycling = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("BikePrefab"))
        {
            otherWaypointBike = other.GetComponent<WaypointsRoute>();
            for (int i = 0; i < yieldWayPoint.Length; i++)
            {
                if (yieldWayPoint[i].GetComponentInChildren<WaypointsRoute>() != null)
                {
                    route = yieldWayPoint[i].GetComponentInChildren<WaypointsRoute>();
                    for (int j = 0; j < firstManagers.Length; j++)
                    {
                        if (otherWaypointBike.firstManager == firstManagers[j])
                        {
                            route.stopCycling = false;
                        }

                    }
                }
            }
        }
        else if (other.CompareTag("CarPrefab"))
        {
            for (int i = 0; i < yieldWayPoint.Length; i++)
            {
                if (yieldWayPoint[i].GetComponentInChildren<WaypointsRoute>() != null)
                {
                    route = yieldWayPoint[i].GetComponentInChildren<WaypointsRoute>();

                    route.stopCycling = false;
                }
            }
        }

    }

}
