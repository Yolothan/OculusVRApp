using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWaypointCar : MonoBehaviour
{
    [SerializeField]
    private CarController carController;       
    [SerializeField]
    private int spawnRoute;
    private WaypointCar waypointCar;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CarPrefab"))
        {
            if (carController.useMaxPrefabs)
            {
                if (!carController.spawnRoutes[spawnRoute].occupied)
                {
                    carController.spawnRoutes[spawnRoute].InstantiateCarsMaxPrefabs(carController.spawnRoutes[spawnRoute].waypoints[0].transform);
                    waypointCar = carController.car.GetComponent<WaypointCar>();
                    waypointCar.routeManager = carController.spawnRoutes[spawnRoute];
                }
            }
            else
            {
                if (!carController.spawnRoutes[spawnRoute].occupied)
                {
                    carController.spawnRoutes[spawnRoute].InstantiateCars(carController.spawnRoutes[spawnRoute].waypoints[0].transform);
                    waypointCar = carController.car.GetComponent<WaypointCar>();
                    waypointCar.routeManager = carController.spawnRoutes[spawnRoute];
                }
            }
        }
    }   
}
