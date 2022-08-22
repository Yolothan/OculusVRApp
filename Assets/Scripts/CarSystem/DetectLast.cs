using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectLast : MonoBehaviour
{
    [HideInInspector]
    public bool reachedLast = false;
    private WaypointCar waypointCar, otherWaypointCar;
    [SerializeField]
    CarController controllerComponent;
    private WaypointRouteManager routeManager;    


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("CarPrefab"))
        {
            reachedLast = true;
            waypointCar = other.gameObject.GetComponent<WaypointCar>();
            waypointCar.routeManager.listCars.Remove(other.gameObject);

            routeManager = other.gameObject.GetComponent<WaypointCar>().firstManager;                      

            if (controllerComponent.useMaxPrefabs && !routeManager.occupied)
            {
                routeManager.InstantiateCarsMaxPrefabs(controllerComponent.spawnRoutes[waypointCar.chosenInt].waypoints[0]);
                otherWaypointCar = routeManager.car.GetComponent<WaypointCar>();
                otherWaypointCar.routeManager = controllerComponent.spawnRoutes[waypointCar.chosenInt];
            }

            Destroy(other.gameObject);
        }
    }   
}
