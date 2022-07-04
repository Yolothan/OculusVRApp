using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteConnectorBike : MonoBehaviour
{
    private WaypointsRoute waypointBike;
    [SerializeField]
    private WaypointRouteManagerBikes managerNextRoute;
    [SerializeField]
    private int newRouteInt = 0;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("BikePrefab"))
        {
            waypointBike = other.GetComponent<WaypointsRoute>();
            waypointBike.routeManager = managerNextRoute;            
            waypointBike.ConnectRoute(newRouteInt);
        }
    }
}
