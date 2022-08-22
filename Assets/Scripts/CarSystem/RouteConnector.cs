using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteConnector : MonoBehaviour
{
    private WaypointCar waypointCar;
    [SerializeField]
    private WaypointRouteManager manager;
    [SerializeField] 
    private WaypointRouteManager[] firstManager;
    [SerializeField]
    private int newRouteInt = 0;  
    

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("CarPrefab"))
        {
            for (int i = 0; i < firstManager.Length; i++)
            {
                waypointCar = other.GetComponent<WaypointCar>();
                if (waypointCar.firstManager == firstManager[i])
                {
                    waypointCar.routeManager = manager;
                    waypointCar.ConnectRoute(newRouteInt);
                }
            }
        }
    }
}
