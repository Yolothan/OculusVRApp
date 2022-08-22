using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftWaypointsBike : MonoBehaviour
{
    private WaypointsRoute waypointBike;
    [SerializeField]
    private WaypointRouteManagerBikes[] nextRouteManagers;
    private int newRouteInt;
    [SerializeField]
    private int changeOdds = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BikePrefab"))
        {
            newRouteInt = Random.Range(0, nextRouteManagers.Length + changeOdds);
            if (newRouteInt <= nextRouteManagers.Length)
            {
                waypointBike = other.GetComponent<WaypointsRoute>();
                waypointBike.routeManager = nextRouteManagers[newRouteInt];               
                waypointBike.RedoStart();
            }
            else
            {
                return;
            }

        }
    }
}
