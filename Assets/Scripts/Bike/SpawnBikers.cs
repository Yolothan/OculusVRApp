using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBikers : MonoBehaviour
{
    [SerializeField]
    private WaypointController bikeController;    
    [SerializeField]
    private int spawnRoute;
    private WaypointsRoute waypointBike;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BikePrefab"))
        {            
            //if (!detectOccupied.occupied)
            //{
                bikeController.InstantiateBikes(bikeController.spawnRoutes[spawnRoute].waypoints[0].transform);
                waypointBike = bikeController.bike.GetComponent<WaypointsRoute>();
                waypointBike.routeManager = bikeController.spawnRoutes[spawnRoute];                
            //}
        }
    }
}
